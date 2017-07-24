using Leap.Unity.Interaction;
using UnityEngine;

public class SnapPiecetoBoard : MonoBehaviour {

    private GameBoard gameBoard;

    // Set board height, grid scale
    // Get board position
    private float boardPosY = 0.0f; // TODO link somehow
    private float checkerHeight = 0.01f;
    private float grid_scale = (0.5f/8);

    private InteractionBehaviour intObj;

    // Use this for initialization
    void Start () {
        intObj = GetComponent<InteractionBehaviour>();
        if (intObj != null) {
            intObj.OnGraspEnd += SnaptoSquare;
        }

        gameBoard = FindObjectOfType<GameBoard>();
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

        snappedPosition = new Vector3(Mathf.Round((posRelativeToBoard.x - halfGrid)/ grid_scale)*grid_scale + grid_scale/2f,
                                      boardPosY + checkerHeight*.7f,
                                      Mathf.Round((posRelativeToBoard.z - halfGrid) / grid_scale) *grid_scale + grid_scale/2f);
        snappedPosition += gameBoard.transform.position;

        transform.position = snappedPosition;

        transform.rotation = Quaternion.identity;
    }

    // TODO - add a check for whether square is open

    // TODO include a distance check that piece is not too far from board

}
