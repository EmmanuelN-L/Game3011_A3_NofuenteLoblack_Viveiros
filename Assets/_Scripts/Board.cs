using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    wait,
    move
}

public class Board : MonoBehaviour
{
    public int maxCandy = 0;
    public int score = 0;
    public int scoreToReach;
    public GameState currentState = GameState.move;
    public int width;
    public int height;
    public int offset;
    public GameObject tilePrefab;
    public GameObject[] dots;
    private BackgroundTile [,] allTiles;
    public GameObject[,] allDots;
    private FindMatches findMatches;
    private UIScript UI;

    

    // Start is called before the first frame update
    void Start()
    {
        findMatches = FindObjectOfType<FindMatches>();
        UI = FindObjectOfType<UIScript>();
        //Creating the width and height of board
        allTiles = new BackgroundTile[width, height];
        allDots = new GameObject[width, height];
        //Setup();
        
    }


    public void Setup()
    {
        for (int i =0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPosition = new Vector2(i, j + offset);
                GameObject backgrounTile = Instantiate(tilePrefab, tempPosition, Quaternion.identity) as GameObject;
                backgrounTile.transform.parent = this.transform;
                backgrounTile.name = "( " + i +", " + j + " )";
                int dotToUse = Random.Range(0, maxCandy);
                int maxIterations = 0;

                while(matchesAt(i, j, dots[dotToUse]) && maxIterations < 100)
                {
                    dotToUse = Random.Range(0, maxCandy);
                    maxIterations++;
                }
                maxIterations = 0;

                GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                dot.GetComponent<Candy>().row = j;
                dot.GetComponent<Candy>().column = i;

                dot.transform.parent = this.transform;
                dot.name = "( " + i + ", " + j + " )";

                allDots[i, j] = dot;

            }
        }
    }
    //Prevents matches at th beginning of the game
    private bool matchesAt(int column, int row, GameObject piece)
    {
        if(column > 1 && row > 1)
        {
            if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
            {
                return true;
            }
            if (allDots[column , row - 1].tag == piece.tag && allDots[column , row - 2].tag == piece.tag)
            {
                return true;
            }
        }
        else if(column <= 1 || row <= 1)
        {
            if(row > 1)
            {
                if(allDots[column, row -1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
                {
                    return true;
                }
            }
            if (column > 1)
            {
                if (allDots[column - 1, row ].tag == piece.tag && allDots[column- 2, row].tag == piece.tag)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void destroyMatchesAt(int column, int row)
    {
        if(allDots[column, row].GetComponent<Candy>().isMatched)
        {
            findMatches.currentMatches.Remove(allDots[column, row]);
            score += 100;
            if (score >= scoreToReach)
            {
                UI.winCondition();
            }
            Destroy(allDots[column, row]);
            allDots[column, row] = null;
        }
    }
    //Destroys the candies that were a match
    public void destroyMatches()
    {
       for (int i = 0; i< width; i++)
        {
            for (int j = 0; j< height; j++)
            {
                if(allDots[i, j] !=null)
                {
                    destroyMatchesAt(i, j);
                }
            }
        }
        StartCoroutine(decreasedRowCo());
    }
    //makes candies above fall into the empty spaces
    private IEnumerator decreasedRowCo()
    {
        int nullCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null)
                {
                    nullCount++;
                }
                else if(nullCount > 0)
                {
                    allDots[i, j].GetComponent<Candy>().row -= nullCount;
                    allDots[i, j] = null;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(FillBoardCo());

    }

    private void RefillBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null)
                {
                    Vector2 tempPosition = new Vector2(i, j + offset);
                    int dotToUse = Random.Range(0, maxCandy);
                    GameObject piece = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                    allDots[i, j] = piece;
                    piece.transform.parent = this.transform;
                    piece.name = "( " + i + ", " + j + " )";
                    piece.GetComponent<Candy>().row = j;
                    piece.GetComponent<Candy>().column = i;
                }
            }
        }
    }

    private bool matchesOnBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    if(allDots[i,j].GetComponent<Candy>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private IEnumerator FillBoardCo()
    { 
        RefillBoard();
        yield return new WaitForSeconds(0.5f);

        while(matchesOnBoard())
        {
            yield return new WaitForSeconds(0.5f);
            destroyMatches();
        }
        yield return new WaitForSeconds(0.5f);
        currentState = GameState.move;
    }

}
