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
            TouchStartProcess(touchPos);
        }
        else if (touchMoved)
        {
            TouchMoveProcess(touchPos);
        }
        else if (touchEnded)
        {
            TouchEndProcess(touchPos);
        }
    }

    void TouchStartProcess(Vector2 touchPos) 
    {
        LevelActor currentLevel = LevelManager.instance.levelCreateOfficer.currentLevel.GetComponent<LevelActor>();
        if (currentLevel.drawImageActor.currentDrawState == DrawImageActor.DrawState.Outlining)
        {
            currentLevel.pencilActor.pencilDrawProcessOfficer.StartOutlining();
        }
    }

    void TouchMoveProcess(Vector2 touchPos) 
    {
        LevelActor currentLevel = LevelManager.instance.levelCreateOfficer.currentLevel.GetComponent<LevelActor>();
        if (currentLevel.drawImageActor.currentDrawState == DrawImageActor.DrawState.Outlining)
        {
            currentLevel.pencilActor.pencilDrawProcessOfficer.DrawOutline();
        }
    }

    void TouchEndProcess(Vector2 touchPos) 
    {
        LevelActor currentLevel = LevelManager.instance.levelCreateOfficer.currentLevel.GetComponent<LevelActor>();
        if (currentLevel.drawImageActor.currentDrawState == DrawImageActor.DrawState.Outlining)
        {
            currentLevel.pencilActor.pencilDrawProcessOfficer.StopOutlining();
        }
    }
}
