using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PencilMoveOfficer : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    public float smoothTime = 0.2f; 
    private Vector3 velocity = Vector3.zero;

    public void MovePencil() 
    {
        Vector2 speed = moveSpeed * InputManager.instance.inputSlideOfficer.moveRateVector2;
        //print("SPEED : "+speed);
        //transform.Translate(speed);
        transform.position = Vector3.SmoothDamp(transform.position, transform.position + new Vector3(speed.x, speed.y, 0f), ref velocity, smoothTime);

    }
}
