using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 1f;
    [SerializeField] private float rotateSensitivity = 15f;
    [SerializeField] private float bound = 1f;
    [SerializeField] private float rotateBound = 0.5f;
    [SerializeField] float rotateValue = 0;
    [SerializeField] float temp = 0;
    private bool canMove = false;
    private Touch theTouch;

    public float targetValue;
    public float timeThreshold = 2f; // Minimum ayný deðerde kalma süresi
    private float stableTime = 0f;

    private void FixedUpdate()
    {
        Movement();
    }
    private void Movement()
    {
        if (Input.touchCount > 0)
        {
            theTouch = Input.GetTouch(0);
            if (theTouch.phase == TouchPhase.Began)
            {
                canMove = true;
            }
            if (theTouch.phase == TouchPhase.Stationary || theTouch.phase == TouchPhase.Moved)
            {
                float coefficient = (theTouch.deltaPosition.x / Screen.width);

                rotateValue = coefficient * rotateSensitivity;
                if (temp + rotateValue >= -rotateBound && temp + rotateValue <= rotateBound)
                {
                    temp += rotateValue;
                    transform.eulerAngles += new Vector3(0, rotateValue, 0);

                }


                if ((transform.position + transform.forward * forwardSpeed * Time.deltaTime).x < bound)
                {
                    if ((transform.position + transform.forward * forwardSpeed * Time.deltaTime).x > -bound)
                    {
                        transform.position += transform.forward * forwardSpeed * Time.deltaTime;
                    }
                }

                CalcStable(coefficient);



            }
            if (theTouch.phase == TouchPhase.Ended)
            {
                canMove = false;
            }
        }
    }

    private void CalcStable(float value)
    {
        if (value == 0)
        {
            stableTime += Time.deltaTime;
            if (stableTime >= timeThreshold)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
        else
        {
            stableTime = 0f;
        }
    }

}
