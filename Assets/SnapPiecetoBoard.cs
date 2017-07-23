using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPiecetoBoard : MonoBehaviour {

    // Set board height, grid scale
    private float boardPosY = 0.0f; // TODO link somehow
    private float grid_scale = 0.06f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        SnaptoSquare();

        // Implement AI return move

    }

    // Snap checker piece to nearest square
    void SnaptoSquare() {

        Vector3 currentPos = transform.position;
        transform.position = new Vector3(Mathf.Round(currentPos.x / grid_scale)*grid_scale,
                                     boardPosY,
                                     Mathf.Round(currentPos.z / grid_scale)*grid_scale);
    }

    // TODO - add a check for whether square is open

    // TODO include a distance check that piece is not too far from board

}
