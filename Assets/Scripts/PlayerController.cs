using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float tweenTime = 1f;
    [SerializeField] private float forwardSpeed = 1f;
    [SerializeField] private float rotateSensitivity = 15f;
    [SerializeField] private float bound = 1f;
    [SerializeField] private float rotateBound = 0.5f;
    [SerializeField] float rotateValue = 0;
    [SerializeField] float temp = 0;
    private float minSize = 0.05f;
    private float minSizeTorso = 0.5f;
    private float diamondScore = 0;
    private bool canMove = false;
    private bool isNearToDead = false;
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
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject bodyPiecePrefab;
    private Rigidbody rigidbody;
    private Animator anim;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        canMove = true;
    }

    private void Update()
    {
        if (canMove)
        {
            Movement();
        }
    }

    private void Movement()
    {
        if (Input.touchCount > 0)
        {
            theTouch = Input.GetTouch(0);
            if (theTouch.phase == TouchPhase.Stationary || theTouch.phase == TouchPhase.Moved)
            {
                anim.SetBool("Run", true);
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
                anim.SetBool("Run", false);
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

        if ((bodyParts[0].transform.localScale.x + value) + value * 0.001f <= minSize)
        {
            if (isNearToDead)
            {
                foreach (GameObject obj in bodyParts)
                {
                    obj.transform.DOScale(Vector3.zero, tweenTime);
                }
                hips.transform.DOScale(Vector3.zero, tweenTime).OnComplete(() =>
                {
                    GetComponent<CapsuleCollider>().isTrigger = true;
                    rigidbody.useGravity = false;
                    head.GetComponent<Collider>().isTrigger = false;
                    Rigidbody headRb = head.AddComponent<Rigidbody>();
                    headRb.AddForce(new Vector3(0, 0, 3), ForceMode.Impulse);
                    head.transform.parent = null;
                    canMove = false;
                    print("Game Over");
                });
            }
            else
            {
                isNearToDead = true;
                foreach (GameObject obj in bodyParts)
                {
                    obj.transform.DOScale(new Vector3(minSize, obj.transform.localScale.y, minSize), tweenTime);
                }
                hips.transform.DOScale(new Vector3(minSize, hips.transform.localScale.y, minSize), tweenTime);
            }

        }
        else
        {
            isNearToDead = false;
            foreach (GameObject obj in bodyParts)
            {
                obj.transform.DOScale(obj.transform.localScale + new Vector3(value, 0, value), tweenTime);
            }
            hips.transform.DOScale(hips.transform.localScale + new Vector3(value, 0, value), tweenTime);
        }
    }

    public bool GetTallOrShort(float value)
    {
        value = value / 100;
        if (torso.transform.localScale.y + value < minSizeTorso)
        {
            GetFatOrSlim(value * 100);
            return false;
        }
        else
        {
            torso.transform.DOScaleY(torso.transform.localScale.y + value, tweenTime);
            upBody.transform.DOMoveY(upBody.transform.position.y + value, tweenTime);
            return true;
        }
    }

    public void Hit(Transform hitPoint)
    {
        if (GetTallOrShort(-25))
        {
            Vector3 spawnPoint = new Vector3(transform.position.x, hitPoint.position.y, transform.position.z);
            GameObject bodyPieceClone = Instantiate(bodyPiecePrefab, spawnPoint, Quaternion.identity);
            Rigidbody pieceRb = bodyPieceClone.GetComponent<Rigidbody>();
            pieceRb.AddForce(new Vector3(0, 1, -0.05f));
            pieceRb.AddTorque(Random.insideUnitSphere.normalized * 100);
            Destroy(bodyPieceClone, 3f);
        }
    }

    public void Jump(float jumpForce)
    {
        anim.SetBool("Jump", true);
        transform.forward = Vector3.forward;
        rigidbody.AddForce((Vector3.forward + Vector3.up * 1.5f) * jumpForce, ForceMode.Impulse);
    }

    public void StopMovement()
    {
        canMove = false;
    }

    public void StartMovement()
    {
        canMove = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            anim.SetBool("Jump", false);
            StartMovement();
        }
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Diamond"))
                {
                    diamondScore++;
                    other.gameObject.SetActive(false);
                    //diamond toplama sesi 
                }
    }

}
