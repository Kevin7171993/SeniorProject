using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this scripte is going into the SliderBar main object
public class Slider_Horizontal : MonoBehaviour
{
    public RectTransform leftend, rightend, knobTransform; //buffers keep track of the transformation information of left and right end;
    public GameObject mKnob; //Buffer to reference the knob
    public float minValue, maxValue;
    public float currentValue;

    private float rawValue, rawRight, rawKnob;
    private bool isDragging, isOver;
    // Start is called before the first frame update
    void Start()
    {
        knobTransform = mKnob.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //If the knob somehow went outside of the bar, set it's position to the minimum/maximum position of the bar
        if(knobTransform.position.x <= leftend.position.x)
        {
            Vector3 v;
            v = Vector3.zero;
            v.x = leftend.position.x;
            v.y = knobTransform.position.y;
            knobTransform.position = v;
        }
        if (knobTransform.position.x >= rightend.position.x)
        {
            Vector3 v;
            v = Vector3.zero;
            v.x = rightend.position.x;
            v.y = knobTransform.position.y;
            knobTransform.position = v;
        }

        //If playing is dragging the mouse
        if(isDragging)
        {
            //Check if mouse position is outside the bar, if not outside the bar, move the knob to mouse's x position;
            if (!(Input.mousePosition.x <= leftend.position.x) && !(Input.mousePosition.x >= rightend.position.x))
            {
                Vector3 v;
                v = Vector3.zero;
                v.x = Input.mousePosition.x;
                v.y = knobTransform.position.y;
                mKnob.transform.position = v;
            }
            else if(Input.mousePosition.x >= rightend.position.x)
            {
                Vector3 v;
                v = Vector3.zero;
                v.x = rightend.position.x;
                v.y = knobTransform.position.y;
                mKnob.transform.position = v;
            }
            else if (Input.mousePosition.x <= leftend.position.x)
            {
                Vector3 v;
                v = Vector3.zero;
                v.x = leftend.position.x;
                v.y = knobTransform.position.y;
                mKnob.transform.position = v;
            }

            if (Input.GetMouseButtonUp(0)) //isDragging is false if player release the mouse button, AKA mouse button up
            {
                isDragging = false;
            }
        }

        rawKnob = (knobTransform.position.x - leftend.position.x); //this is the x value of the knob, when the leftend of the bar is at (0, 0)
        rawRight = (rightend.position.x - leftend.position.x); //right - left is the total length of the bar
        rawValue = rawKnob / rawRight; //self explanatory, when rawknob position / length of the bar, it gives you a percentage of how far right the knob is


        currentValue = (rawValue * (maxValue - minValue)) + minValue; //calculate final value between min and max
    }

    public void OnMouseDown()
    {
        if (isOver)
        {
            isDragging = true;
        }
    }
    public void OnMouseUp()
    {
        isDragging = false;
    }

    public void OnMouseOver()
    {
        isOver = true;
    }

    public void OnMouseExit()
    {
        isOver = false;
    }

}
