using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputJoystickOfficer : InputAbstractOfficer
{
    [SerializeField] Vector2 joystickAnchorPos, moveVector;
    [SerializeField] float moveVectorMaxMagnitude;
    [SerializeField] GameObject joyStick;
    
    private void Start()
    {
        //getInputOfficer.InputJoystick += InputJoystickProcess;
    }

    void InputJoystickProcess(bool touchStart, bool touchMoved, bool touchEnded, Vector2 touchPos)
    {
        if (touchStart)
        {
            joystickAnchorPos = new Vector2(touchPos.x, touchPos.y);
            CalculateTheMoveVector(touchPos);
            MoveThePlayer();
            JoystickHandler(touchStart, touchMoved, touchEnded);
        }
        else if (touchMoved)
        {
            CalculateTheMoveVector(touchPos);
            MoveThePlayer();
            JoystickHandler(touchStart, touchMoved, touchEnded);
        }
        else if (touchEnded)
        {
            moveVector = Vector2.zero;
            MoveThePlayer();
            JoystickHandler(touchStart, touchMoved, touchEnded);
        }
    }

    void CalculateTheMoveVector(Vector2 touchPos)
    {
        Vector2 unNormalizedMoveVector = touchPos - joystickAnchorPos;
        moveVector = NormalizeTheMoveVector(unNormalizedMoveVector);
    }

    Vector2 NormalizeTheMoveVector(Vector2 unNormalizedMoveVector)
    {
        Vector2 normalizedVector = Vector2.ClampMagnitude(unNormalizedMoveVector, moveVectorMaxMagnitude);
        return normalizedVector;
    }

    void JoystickHandler(bool touchStart, bool touchMoved, bool touchEnded)
    {
        

        if (touchStart)
        {
            joyStick.GetComponent<JoystickActor>().ActivateTheJoystick(joystickAnchorPos);
        }
        else if (touchMoved)
        {
            Vector2 JoytstickWheelPosition = joystickAnchorPos + moveVector;
            joyStick.GetComponent<JoystickActor>().UseTheJoystick(JoytstickWheelPosition);
        }
        else if (touchEnded)
        {
            joyStick.GetComponent<JoystickActor>().DeactivateTheJoystick();
        }
        
    }

    void MoveThePlayer()
    {
        Vector3 moveVector3 = new Vector3(moveVector.x, 0f, moveVector.y);
        //PlayerManager.instance.playerActor.playerMoveOfficer.MoveTheCharacter(moveVector3);
    }
}
