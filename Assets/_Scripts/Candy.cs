using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{

    private Vector2 firstClickPosition;
    private Vector2 finalClickPosition;
    public float swipeAngle = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDown()
    {
        firstClickPosition = Camera.main.WorldToScreenPoint(Input.mousePosition);
        //Debug.Log(firstClickPosition);
    }

    private void OnMouseUp()
    {
        finalClickPosition = Camera.main.WorldToScreenPoint(Input.mousePosition);
        //Debug.Log(finalClickPosition);
        CalculateAngle();
        //Debug.Log("Calculate");
    }

    void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalClickPosition.y - firstClickPosition.y, finalClickPosition.x - firstClickPosition.x) * 180/Mathf.PI;
        Debug.Log(swipeAngle);
    }

}
