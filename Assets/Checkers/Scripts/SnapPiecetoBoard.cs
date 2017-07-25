using Leap.Unity.Interaction;
using UnityEngine;

public class SnapPiecetoBoard : MonoBehaviour {

    private GameBoard gameBoard;

    // Set board height, grid scale
    // Get board position
    private float boardPosY = 0.0f; // TODO link somehow
    private float checkerHeight = 0.01f;
    private float grid_scale = (0.5f/8);
    private int row = -1, column = -1;

    private InteractionBehaviour intObj;

    // Use this for initialization
    void Start () {
        intObj = GetComponent<InteractionBehaviour>();
        if (intObj != null) {
            intObj.OnGraspEnd += SnaptoSquare;
        }

        gameBoard = FindObjectOfType<GameBoard>();
        SnaptoSquare();
    }

    // Update is called once per frame
    void Update () {
    }

    // Snap checker piece to nearest square
    void SnaptoSquare() {

        float halfGrid = grid_scale / 2f;
        Vector3 posRelativeToBoard = transform.position - gameBoard.transform.position;

        // If position relative to board is outside board, constrain to board
        if (posRelativeToBoard.x >= (grid_scale * 8f - halfGrid)) {
            posRelativeToBoard.x = (grid_scale * 8f - halfGrid);
        }

        if (posRelativeToBoard.x <= (grid_scale * 0f + halfGrid)) {
            posRelativeToBoard.x = (grid_scale * 0f + halfGrid);
        }

        if (posRelativeToBoard.z >= (grid_scale * 8f - halfGrid))
        {
            posRelativeToBoard.z = (grid_scale * 8f - halfGrid);
        }

        if (posRelativeToBoard.z <= (grid_scale * 0f + halfGrid))
        {
            posRelativeToBoard.z = (grid_scale * 0f + halfGrid);
        }

        Vector3 snappedPosition;

        int new_row = 7 - Mathf.RoundToInt((posRelativeToBoard.z - halfGrid) / grid_scale);
        int new_column = Mathf.RoundToInt((posRelativeToBoard.x - halfGrid) / grid_scale);
        CheckersBoard.CheckersManager manager = FindObjectOfType<CheckersBoard.CheckersManager>();
        if (row != -1) {
            Debug.Log("checking: old_row = " + row + ", old_column = " + column + ", new_row = " + new_row + ", new_column = " + new_column);
            manager.SetMove(row, column, new_row, new_column);
            if (new_row < 0 || new_column < 0 || new_row > 7 || new_column > 7 || !manager.CheckMove())
            {
                Debug.Log("checkmove failed, snapping back");
                transform.position = gameBoard.transform.position + new Vector3(column * grid_scale + halfGrid,
                    boardPosY + checkerHeight * 0.7f,
                    (7 - row) * grid_scale + halfGrid);
                transform.rotation = Quaternion.identity;
                // ignore new_row, new_column
                return;
            }
            else
            {
                Debug.Log("checkmove succeeded");
            }
        }

        if (row != -1) {
            Debug.Log("calling MakeMove()");
            manager.MakeMove();
        }

        if (row == -1)
        {
            Debug.Log("initialization: row = " + new_row + " column = " + new_column);
        }

        row = new_row;
        column = new_column;

        snappedPosition = new Vector3(Mathf.Round((posRelativeToBoard.x - halfGrid)/ grid_scale)*grid_scale + halfGrid,
                                      boardPosY + checkerHeight*.7f,
                                      Mathf.Round((posRelativeToBoard.z - halfGrid) / grid_scale) *grid_scale + halfGrid);
        snappedPosition += gameBoard.transform.position;

        transform.position = snappedPosition;

        transform.rotation = Quaternion.identity;
    }

    // TODO - add a check for whether square is open

    // TODO include a distance check that piece is not too far from board

}
