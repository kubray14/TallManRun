using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 1f;
    [SerializeField] private float rotateSensitivity = 15f;
    [SerializeField] private float bound = 1f;
    [SerializeField] private float rotateBound = 0.5f;
    [SerializeField] float rotateValue = 0;
    [SerializeField] float temp = 0;
    private float minSize = 0.05f;
    private float minSizeTorso = 0.5f;
    private bool canMove = false;
    private Touch theTouch;
    #region 
    public float targetValue;
    public float timeThreshold = 2f; // Minimum ayný deðerde kalma süresi
    private float stableTime = 0f;
    #endregion
    [SerializeField] private List<GameObject> bodyParts;
    [SerializeField] private GameObject upBody;
    [SerializeField] private GameObject torso;
    [SerializeField] private GameObject hips;

    [SerializeField] private GameObject bodyPiece;

    private bool isNearToDead = false;
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
        value = value / 100;

        if (bodyParts[0].transform.localScale.x + value < minSize)
        {
            print("1");
            foreach (GameObject obj in bodyParts)
            {
                obj.transform.DOScale(new Vector3(minSize, obj.transform.localScale.y, minSize), 1f);
            }
            hips.transform.DOScale(new Vector3(minSize * 2, hips.transform.localScale.y, minSize * 2), 1f);
        }
        else if (bodyParts[0].transform.localScale.x == minSize)
        {
            print("2");
            foreach (GameObject obj in bodyParts)
            {
                obj.transform.DOScale(Vector3.zero, 1f);
            }
            hips.transform.DOScale(Vector3.zero, 1f).OnComplete(() => { print("Game Over"); });
        }
        else
        {
            print("3");
            foreach (GameObject obj in bodyParts)
            {
                obj.transform.DOScale(obj.transform.localScale + new Vector3(value, 0, value), 1f);
            }
            hips.transform.DOScale(hips.transform.localScale + new Vector3(value, value * 0.5f, value), 1f);
        }
    }

    public bool GetTallOrShort(float value)
    {
        value = value / 100;
        if (torso.transform.localScale.y + value <= minSizeTorso)
        {
            GetFatOrSlim(value * 100);
            return false;
        }
        else
        {
            torso.transform.DOScaleY(torso.transform.localScale.y + value, 1f);
            upBody.transform.DOMoveY(upBody.transform.position.y + value, 1f);
            return true;
        }
    }

    public void Hit(Transform hitPoint)
    {
        if (GetTallOrShort(-25))
        {
            Vector3 spawnPoint = new Vector3(transform.position.x, hitPoint.position.y, transform.position.z);
            GameObject bodyPieceClone = Instantiate(bodyPiece, spawnPoint, Quaternion.identity);
            Rigidbody pieceRb = bodyPieceClone.GetComponent<Rigidbody>();
            pieceRb.AddForce(new Vector3(0, 1, -0.05f));
            pieceRb.AddTorque(Random.insideUnitSphere.normalized * 100);
            Destroy(bodyPieceClone, 3f);
        }
    }

}
