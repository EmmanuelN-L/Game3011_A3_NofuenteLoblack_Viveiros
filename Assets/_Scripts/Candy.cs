using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    [Header("Board Variables")]
    public int column;
    public int row;
    public int previousColumn;
    public int previousRow;
    public int targetX;
    public int targetY;
    public bool isMatched = false;

    private FindMatches findMatches;
    private Board board;
    private GameObject otherCandy;
    private Vector2 firstClickPosition;
    private Vector2 finalClickPosition;
    private Vector2 tempPosition;

    public float swipeAngle = 0;
    public float swipeResist = 1f;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatches>();
        //targetX = (int)transform.position.x;
        //targetY = (int)transform.position.y;
        //row = targetY;
        //column = targetX;
        //previousRow = row;
        //previousColumn = column;

    }

    // Update is called once per frame
    void Update()
    {
        //findMatches();
        
        if(isMatched)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = new Color(1f, 1f, 1f, .2f);
        }
        
        targetX = column;
        targetY = row;

        //for movement on the X
        if(Mathf.Abs(targetX - transform.position.x) > .1)
        {
            //Move Toward the target
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, 0.1f);
            if(board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
            }
            findMatches.findAllMatches();
        }
        else
        {
            //Directly set the position
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
        }

        //For movement on the Y
        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            //Move Toward the target
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, 0.1f);
            if (board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
            }
            findMatches.findAllMatches();
        }
        else
        {
            //Directly set the position
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
        }
    }

    public IEnumerator CheckMoveCo()
    {
        yield return new WaitForSeconds(1.0f);
        if(otherCandy != null)
        {
            if(!isMatched && !otherCandy.GetComponent<Candy>().isMatched)
            {
                otherCandy.GetComponent<Candy>().row = row;
                otherCandy.GetComponent<Candy>().column = column;
                row = previousRow;
                column = previousColumn;     
                yield return new WaitForSeconds(0.5f);
                board.currentState = GameState.move;
            }  
            else
            {
            board.destroyMatches();

            }
            otherCandy = null;
        }


    }

    private void OnMouseDown()
    {
        if(board.currentState == GameState.move)
        {
            firstClickPosition = Camera.main.WorldToScreenPoint(Input.mousePosition);
        }
        
        //Debug.Log(firstClickPosition);
    }

    private void OnMouseUp()
    {
        if (board.currentState == GameState.move)
        {
            finalClickPosition = Camera.main.WorldToScreenPoint(Input.mousePosition);
            //Debug.Log(finalClickPosition);
            CalculateAngle();
            //Debug.Log("Calculate");
        }
    }

    void CalculateAngle()
    {
        if(Mathf.Abs(finalClickPosition.y - firstClickPosition.y) > swipeResist || Mathf.Abs(finalClickPosition.x - firstClickPosition.x) > swipeResist)
        {  
            swipeAngle = Mathf.Atan2(finalClickPosition.y - firstClickPosition.y, finalClickPosition.x - firstClickPosition.x) * 180/Mathf.PI;
            //Debug.Log(swipeAngle);       
            movePieces();
            if (otherCandy != null)
            {
                board.currentState = GameState.wait;
            }
        }
        else
        {
            board.currentState = GameState.move;
        }

    }

    void movePieces()
    {
        if(swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1)
        {
            //Right Swipe
            otherCandy = board.allDots[column + 1, row];
            previousRow = row;
            previousColumn = column;
            otherCandy.GetComponent<Candy>().column -= 1;
            column += 1;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1)
        {
            //Up Swipe
            otherCandy = board.allDots[column , row + 1];
            previousRow = row;
            previousColumn = column;
            otherCandy.GetComponent<Candy>().row -= 1;
            row += 1;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            //Left Swipe
            otherCandy = board.allDots[column -1, row];
            previousRow = row;
            previousColumn = column;
            otherCandy.GetComponent<Candy>().column += 1;
            column -= 1;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            //Down Swipe
            otherCandy = board.allDots[column, row - 1];
            previousRow = row;
            previousColumn = column;
            otherCandy.GetComponent<Candy>().row += 1;
            row -= 1;
        }
        StartCoroutine(CheckMoveCo());
    }

    //void findMatches()
    //{
    //    if(column > 0 && column < board.width - 1)
    //    {
    //        GameObject leftDot1 = board.allDots[column - 1, row];
    //        GameObject rightDot1 = board.allDots[column + 1, row];
    //        if (leftDot1.tag == this.gameObject.tag && rightDot1.tag == this.gameObject.tag)
    //        {
    //            leftDot1.GetComponent<Candy>().isMatched = true;
    //            rightDot1.GetComponent<Candy>().isMatched = true;
    //            isMatched = true;
    //        }
    //    }

    //    if (row > 0 && row < board.height - 1)
    //    {
    //        GameObject upDot1 = board.allDots[column, row + 1];
    //        GameObject downDot1 = board.allDots[column, row - 1];
    //        if (upDot1.tag == this.gameObject.tag && downDot1.tag == this.gameObject.tag)
    //        {
    //            upDot1.GetComponent<Candy>().isMatched = true;
    //            downDot1.GetComponent<Candy>().isMatched = true;
    //            isMatched = true;
    //        }
    //    }
    //}

}
