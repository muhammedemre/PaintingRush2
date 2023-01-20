using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickActor : MonoBehaviour
{
    [SerializeField] Transform body, wheel;

    public void ActivateTheJoystick(Vector2 anchorPosition)
    {
        transform.position = anchorPosition;
        gameObject.SetActive(true);
    }

    public void DeactivateTheJoystick()
    {
        gameObject.SetActive(false);
    }

    public void UseTheJoystick(Vector2 wheelPos)
    {
        wheel.position = wheelPos;
    }
}
