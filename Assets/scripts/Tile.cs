using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    // state when tile is active (you can see its front)
    public bool Uncovered = true;
    // tile is currently on board
    public bool Active = true;
    // front texture of the tile
    public Sprite frontFace;


	// Use this for initialization
	void Start () {
        // initialize rotation state
        transform.rotation = GetTargetRotation();

        //search for front object in tile
        var frontObject = transform.Find("front");
        var spriteRenderer = frontObject.transform.GetComponent<SpriteRenderer>();
        // change sprite to frontFace obj
        spriteRenderer.sprite = frontFace;
	}
	
	// Update is called once per frame
	void Update () {

        // get current tile location
        var targetRotation = GetTargetRotation();
        // transform rotation using Lerp() - three parameters: - 1. start state = transform.rotation 2. finish state = targetRotation 3. animation duration
        transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                //to speed up animation multiply by 5
                Time.deltaTime * 5f
            );

        if (Active == false)
        {
            Destroy(gameObject);
        }

    }


    //quaternion function for rotating tile
    Quaternion GetTargetRotation()
    {
        // if tile is active vector is zero, if not active rotate it by 180 deg
        var rotation = Uncovered ? Vector3.zero : (Vector3.up * 180f);
        return Quaternion.Euler(rotation);
    }

    private void OnMouseDown()
    {
        var board = FindObjectOfType<Board>();

        if (board.CanMove == false) return;
        Uncovered = !Uncovered;
        board.CheckPair();
    }
}
