using Leap.Unity.Interaction;
using UnityEngine;

public class SnapPiecetoBoard : MonoBehaviour {

    // Set board height, grid scale
    private float boardPosY = 0.0f; // TODO link somehow
    private float checkerHeight = 0.04f;
    private float grid_scale = 0.06f;

    private InteractionBehaviour intObj;

    // Use this for initialization
    void Start () {
        intObj = GetComponent<InteractionBehaviour>();
        intObj.OnGraspEnd += SnaptoSquare;
    }

    // Update is called once per frame
    void Update () {
        
    }

    // Snap checker piece to nearest square
    void SnaptoSquare() {

        Vector3 currentPos = transform.position;
        transform.position = new Vector3(Mathf.Round(currentPos.x / grid_scale)*grid_scale + 0.03f,
                                     boardPosY + checkerHeight*.7f,
                                     Mathf.Round(currentPos.z / grid_scale)*grid_scale + 0.03f);
        transform.rotation = Quaternion.identity;
    }

    // TODO - add a check for whether square is open

    // TODO include a distance check that piece is not too far from board

}
