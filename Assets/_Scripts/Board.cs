using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject tilePrefab;
    public GameObject[] dots;
    private BackgroundTile [,] allTiles;
    public GameObject[,] allDots;

    // Start is called before the first frame update
    void Start()
    {
        //Creating the width and height of board
        allTiles = new BackgroundTile[width, height];
        allDots = new GameObject[width, height];
        Setup();
    }


    private void Setup()
    {
        for (int i =0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPosition = new Vector2(i, j);
                GameObject backgrounTile = Instantiate(tilePrefab, tempPosition, Quaternion.identity) as GameObject;
                backgrounTile.transform.parent = this.transform;
                backgrounTile.name = "( " + i +", " + j + " )";

                int dotToUse = Random.Range(0, dots.Length);
                GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                dot.transform.parent = this.transform;
                dot.name = "( " + i + ", " + j + " )";

                allDots[i, j] = dot;

            }
        }
    }
}
