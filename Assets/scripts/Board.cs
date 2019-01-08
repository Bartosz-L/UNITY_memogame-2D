using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Board : MonoBehaviour {

    public GameObject TilePrefab;

    public Sprite[] sprites;

    private Tile[] Tiles;

    public Vector2 TilesOffset = Vector2.one;

    public int Width = 6;
    public int Height = 4;

    public bool CanMove = false;

    public TextMesh WinText;


	// Use this for initialization
	IEnumerator Start () {

        WinText.GetComponent<Renderer>().enabled = false;
        // generate tiles
        CreateTiles();
        // shuffle tiles before placing them
        ShuffleTiles();
        // place tiles
        PlaceTiles();

        CanMove = false;
        yield return new WaitForSeconds(2f);
        HideTiles();
        CanMove = true;
    }

    void CreateTiles()
    {
        // loop for generating board (size: Width by Height)
        var length = Width * Height;
        Tiles = new Tile[length];

        for(int i=0; i<length; i++)
        {
            var sprite = sprites[i / 2];
            Tiles[i] = CreateSingleTile(sprite);
        }
    }

    Tile CreateSingleTile(Sprite faceSprite)
    {
        // create single tile
        var gameObject = Instantiate(TilePrefab);

        // give tiles new parent (transform from Board obj)
        gameObject.transform.parent = transform;
        // change front face of the tile
        var tile = gameObject.GetComponent<Tile>();
        tile.frontFace = faceSprite;
        tile.Uncovered = true;

        return tile;
    }
    void ShuffleTiles()
    {
        // shuffle x times for better randomization
        for(int i=0; i<100; i++)
        {
            // random index
            int index1 = Random.Range(0, Tiles.Length);
            int index2 = Random.Range(0, Tiles.Length);
            // reference to object
            var tile1 = Tiles[index1];
            var tile2 = Tiles[index2];
            // change place of tiles
            Tiles[index1] = tile2;
            Tiles[index2] = tile1;
        }
    }

    void PlaceTiles()
    {
        for (int i = 0; i < Width * Height; i++)
        {
            int x = i % Width;
            int y = i / Width;

            Tiles[i].transform.localPosition = new Vector3(x * TilesOffset.x, y * TilesOffset.y, 0);

        }
    }

    void HideTiles()
    {
        Tiles.ToList().ForEach(tile => tile.Uncovered = false);
    }

    private bool CheckIfEnd()
        // check if all tiles are inactive
    {
        return Tiles.All(tile => !tile.Active);
    }

    public void CheckPair()
    {
        StartCoroutine(CheckPairCorouitine());
    }
	
    private IEnumerator CheckPairCorouitine()
    {
        var tilesUncovered = Tiles.Where(tile => tile.Active).Where(tile => tile.Uncovered).ToArray();

        if (tilesUncovered.Length != 2) yield break;

        var tile1 = tilesUncovered[0];
        var tile2 = tilesUncovered[1];

        CanMove = false;
        yield return new WaitForSeconds(1f);
        CanMove = true;

        if (tile1.frontFace == tile2.frontFace)
        {
            tile1.Active = false;
            tile2.Active = false;
        } 
        else
        {
            tile1.Uncovered = false;
            tile2.Uncovered = false;
        }

        // end of the game
        if (CheckIfEnd())
        {
            CanMove = false;
            Debug.Log("The end.");
            WinText.GetComponent<Renderer>().enabled = true;


            yield return new WaitForSeconds(3f);
            Application.Quit();
        }
    }

}
