using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public float force;
    [SerializeField] private float forwardSpeed = 1f;
    [SerializeField] private float rotateSensitivity = 15f;
    [SerializeField] private float bound = 1f;
    [SerializeField] private float rotateBound = 0.5f;
    [SerializeField] float rotateValue = 0;
    [SerializeField] float temp = 0;
    private bool canMove = false;
    private Touch theTouch;
    #region 
    public float targetValue;
    public float timeThreshold = 2f; // Minimum ayný deðerde kalma süresi
    private float stableTime = 0f;
    #endregion
    [SerializeField] private GameObject generalBody;
    [SerializeField] private GameObject upBody;
    [SerializeField] private GameObject torso;
    [SerializeField] private GameObject head;

    [SerializeField] private GameObject bodyPiece;
    private void Start()
    {
        //  rb = GetComponent<Rigidbody>(); 
    }
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

    public void GetFatOrSlim(float value)
    {
        value = value / 150;
        generalBody.transform.DOScale(generalBody.transform.localScale + new Vector3(value, 0, value), 1f);
    }

    public void GetTallOrShort(float value)
    {
        value = value / 100;
        float temp = (value) / 0.5f * 0.2f;
        print(temp);
        torso.transform.DOScale(new Vector3(torso.transform.localScale.x, torso.transform.localScale.y + value, torso.transform.localScale.z), 1f);
        head.transform.DOMoveY(head.transform.position.y + temp, 1f);
        upBody.transform.DOMoveY(upBody.transform.position.y + temp, 1f);
    }

    public void Hit(Transform hitPoint)
    {
        GetTallOrShort(-50);
        Vector3 spawnPoint = new Vector3(transform.position.x, hitPoint.position.y, hitPoint.position.z - (generalBody.transform.localScale.x * 0.2f * 2));
        GameObject bodyPieceClone = Instantiate(bodyPiece, spawnPoint, Quaternion.identity);
        bodyPieceClone.GetComponent<Rigidbody>().AddForce(new Vector3(0, 2, -1) * force);
        Destroy(bodyPieceClone, 3f);

    }

}
