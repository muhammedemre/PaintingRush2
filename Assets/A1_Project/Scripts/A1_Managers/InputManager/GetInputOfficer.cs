using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class GetInputOfficer : SerializedMonoBehaviour
{
    public bool touchable = false;
    
    private bool previousMouseState = false;
    [SerializeField] private bool isFixedUpdate = false;
    [SerializeField] bool clickedOnUI = false;

    public ActiveInputType activeInputType = ActiveInputType.TouchInput;

    public enum ActiveInputType 
    {
        TouchInput, JoystickInput, SlideInput
    }
    
    public delegate void InputTypeProcess(bool touchStart, bool touchMoved, bool touchEnded, Vector2 touchPos);

    //public event InputTypeProcess InputDrag;
    //public event InputTypeProcess InputHold;
    //public event InputTypeProcess InputSwipe;
    public event InputTypeProcess InputSlide;
    public event InputTypeProcess InputTouch;
    public event InputTypeProcess InputJoystick;

    [SerializeField] int UILayer;
    
    private void Update()
    {
        if (!isFixedUpdate && touchable)
        {
            InputControl();
        }
    }

    void FixedUpdate()
    {
        if (isFixedUpdate && touchable)
        {
            InputControl();
        }
    }
    void InputControl()
    {
        if (Application.isMobilePlatform)
        {
            TouchInput();
        }
        else 
        {
            MouseInput();
        }
    }

    void MouseInput()
    {
        if (Input.GetMouseButton(0))
        {
            if (previousMouseState == false)
            {
                previousMouseState = true;
                InputManagerInjector(true, false, false, Input.mousePosition);
            }
            else
            {
                InputManagerInjector(false, true, false, Input.mousePosition);
            }
        }
        else
        {
            if (previousMouseState == true)
            {
                previousMouseState = false;
                InputManagerInjector(false, false, true, Vector2.zero);
            }
        }
    }
    void TouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                InputManagerInjector(true, false, false, touch.position);
            }
            else if (touch.phase == TouchPhase.Moved && !clickedOnUI)
            {
                InputManagerInjector(false, true, false, touch.position);
            }
            else if (touch.phase == TouchPhase.Stationary && !clickedOnUI)
            {
                InputManagerInjector(false, true, false, touch.position);
            }
            else if (touch.phase == TouchPhase.Ended && !clickedOnUI)
            {                
                InputManagerInjector(false, false, true, touch.position);
            }
        }
    }

    void InputManagerInjector(bool touchStart, bool touchMoved, bool touchEnded, Vector2 touchPos)
    {
        if (touchStart)
        {
            clickedOnUI = CheckOnUIClick();
            print("clickedOnUI: " + clickedOnUI);
            if (clickedOnUI)
            {
                return;
            }
        }
        else if (touchEnded)
        {
            clickedOnUI = false;
        }


        switch (activeInputType)
        {
            case ActiveInputType.TouchInput:
                InputTouch(touchStart, touchMoved, touchEnded, touchPos);
                break;
            case ActiveInputType.JoystickInput:
                InputJoystick(touchStart, touchMoved, touchEnded, touchPos);
                break;
            case ActiveInputType.SlideInput:
                InputSlide(touchStart, touchMoved, touchEnded, touchPos);
                break;
            default:
                break;
        }

        
    }

    bool CheckOnUIClick()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (IsPointerOverUIObject())
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.tag == "NoPaintUI")
            {               
                return true;
            }
        }
        return false;
    }

    public void ChangeInputType(ActiveInputType newInputType) 
    {
        activeInputType = newInputType;
    }

}
