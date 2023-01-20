using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTouchOfficer : InputAbstractOfficer
{
    private void Start()
    {
        getInputOfficer.InputTouch += InputTouchProcess;
    }

    void InputTouchProcess(bool touchStart, bool touchMoved, bool touchEnded, Vector2 touchPos)
    {
        if (touchStart)
        {
            
        }
        else if (touchMoved)
        {

        }
        else if (touchEnded)
        {

        }
    }
}
