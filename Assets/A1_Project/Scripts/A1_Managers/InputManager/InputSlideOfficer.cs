using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSlideOfficer : MonoBehaviour
{
    float screenWidth, previousXPos, previousYPos;
    public float moveRate = 0f, moveBoostRate = 1;
    public Vector2 moveRateVector2 = new Vector2(0f, 0f);
    private void Start()
    {
        InputManager.instance.getInputOfficer.InputSlide += InputSlideProcess;
        screenWidth = Screen.width;
    }

    void InputSlideProcess(bool touchStart, bool touchMoved, bool touchEnded, Vector2 touchPos)
    {
        //if (LevelManager.instance.levelCreateOfficer.currentLevel.GetComponent<LevelActor>().drawImageActor.currentDrawState == DrawImageActor.DrawState.Completed)
        //{
        //    return;
        //}

        LevelActor currentLevel = LevelManager.instance.levelCreateOfficer.currentLevel.GetComponent<LevelActor>();
        if (touchStart)
        {
            previousXPos = touchPos.x;
            previousYPos = touchPos.y;
            MoveRate(touchPos.x, touchPos.y);

            currentLevel.pencilActor.pencilDrawProcessOfficer.StartColoring();
        }
        else if (touchMoved)
        {
            MoveRate(touchPos.x, touchPos.y);
        }
        else if (touchEnded)
        {
            moveRate = 0f;
            currentLevel.pencilActor.pencilDrawProcessOfficer.StopColoring();
        }
    }
    void MoveRate(float touchPosX, float touchPosY)
    {
        float differenceBetweenPreviousAndCurrentPosX = touchPosX - previousXPos;
        float differenceBetweenPreviousAndCurrentPosY = touchPosY - previousYPos;
        //moveRate = differenceBetweenPreviousAndCurrentPosX / screenWidth * moveBoostRate;
        //moveRate = Mathf.Clamp((differenceBetweenPreviousAndCurrentPosX / screenWidth * moveBoostRate),-1, 1);
        float moveRateX = Mathf.Clamp((differenceBetweenPreviousAndCurrentPosX / screenWidth * moveBoostRate), -1, 1);
        float moveRateY = Mathf.Clamp((differenceBetweenPreviousAndCurrentPosY / screenWidth * moveBoostRate), -1, 1);
        moveRateVector2 = new Vector2(moveRateX, moveRateY);
        previousXPos = touchPosX;
        previousYPos = touchPosY;

        LevelManager.instance.levelCreateOfficer.currentLevel.GetComponent<LevelActor>().pencilActor.pencilMoveOfficer.MovePencil();

        PaintProcess();

    }

    void PaintProcess() 
    {
        LevelActor currentLevel = LevelManager.instance.levelCreateOfficer.currentLevel.GetComponent<LevelActor>();
        currentLevel.pencilActor.pencilDrawProcessOfficer.Coloring();
    }
}
