using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject headPrefab;
    [SerializeField] private Transform upbodyPivot;
    #region Movement
    [SerializeField] private float tweenTime = 1f;
    [SerializeField] private float forwardSpeed = 1f;
    [SerializeField] private float rotateSensitivity = 15f;
    [SerializeField] private float bound = 1f;
    [SerializeField] private float rotateBound = 0.5f;
    [SerializeField] float rotateValue = 0;
    [SerializeField] float temp = 0;
    [SerializeField] private bool onGround;
    private Touch theTouch;
    public bool canMove = false;
    #endregion
    #region 
    public float targetValue;
    public float timeThreshold = 2f; // Minimum ayný deðerde kalma süresi
    private float stableTime = 0f;
    #endregion
    #region Scale 
    [SerializeField] private List<GameObject> bodyParts;
    [SerializeField] private GameObject upBody;
    [SerializeField] private GameObject torso;
    [SerializeField] private GameObject hips;
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject bodyPiecePrefab;
    private float minSize = 0.05f;
    private float minSizeTorso = 0.5f;
    private bool isNearToDead = false;
    #endregion
    private Rigidbody _rigidbody;
    private Animator anim;
    private int diamondScore = 0;
    [SerializeField] private GameObject finalUI;
    [SerializeField] private TMP_Text scoreText;
    private bool isCameFinalGround = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        canMove = true;
    }

    private void Start()
    {
        DOTween.defaultEaseType = Ease.Linear;
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
                anim.SetBool("Walk", true);
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
        }
        else
        {
            anim.SetBool("Walk", false);
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

    public void GetFatOrSlim(float value, bool isMultiplier)
    {
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
                    Death();
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

            if (isMultiplier)
            {
                foreach (GameObject obj in bodyParts)
                {
                    obj.transform.DOScale(new Vector3(obj.transform.localScale.x * value, obj.transform.localScale.y, obj.transform.localScale.z * value), tweenTime);
                }
                hips.transform.DOScale(new Vector3(hips.transform.localScale.x * value, hips.transform.localScale.y, hips.transform.localScale.z * value), tweenTime);
            }
            else
            {
                foreach (GameObject obj in bodyParts)
                {
                    obj.transform.DOScale(obj.transform.localScale + new Vector3(value, 0, value), tweenTime);
                }
                hips.transform.DOScale(hips.transform.localScale + new Vector3(value, 0, value), tweenTime);
            }

        }
    }

    public bool GetTallOrShort(float value, bool isMultiplier)
    {
        if (isMultiplier)
        {
            if (torso.transform.localScale.y * value < minSizeTorso)
            {
                if (torso.transform.localScale.y <= minSizeTorso + (minSizeTorso * 0.01f))
                {
                    GetFatOrSlim(value, isMultiplier);
                    return false;
                }
                else
                {
                    torso.transform.DOScaleY(minSizeTorso, tweenTime);
                    upBody.transform.DOMoveY(0.56f, tweenTime);
                    return true;
                }

            }
            else
            {
                float firstScale = torso.transform.localScale.y;
                float dif = torso.transform.localScale.y * value - firstScale;
                torso.transform.DOScaleY(torso.transform.localScale.y * value, tweenTime);
                upBody.transform.DOMoveY(upBody.transform.position.y + dif, tweenTime);
                return true;
            }
        }
        else
        {
            if (torso.transform.localScale.y + value < minSizeTorso)
            {
                GetFatOrSlim(value, isMultiplier);
                return false;
            }
            else
            {
                print(value);
                torso.transform.DOScaleY(torso.transform.localScale.y + value, tweenTime);
                upBody.transform.DOMoveY(upBody.transform.position.y + ((value * 2) * transform.localScale.y), tweenTime);
                return true;
            }
        }
    }
    public void StabilUpbody(float value)
    {
        //float factor = torso.transform.localScale.x / (torso.transform.localScale.x - value);
        //upBody.transform.DOScale(upBody.transform.localScale + new Vector3((1 - factor) * (1 / torso.transform.localScale.x), 0, (1 - factor) * (1 / torso.transform.localScale.x)), tweenTime);
        float factor = torso.transform.localScale.x / (torso.transform.localScale.x + value);
        print("factor " + factor);
        upBody.transform.DOScale(new Vector3((upBody.transform.localScale.x * factor), upBody.transform.localScale.y, (upBody.transform.localScale.z * factor)), tweenTime);
    }

    public void Hit(Transform hitPoint)
    {
        if (GetTallOrShort(-0.25f, false))
        {
            Vector3 spawnPoint = new Vector3(transform.position.x, hitPoint.position.y, transform.position.z);
            GameObject bodyPieceClone = Instantiate(bodyPiecePrefab, spawnPoint, Quaternion.identity);
            Rigidbody pieceRb = bodyPieceClone.GetComponent<Rigidbody>();
            pieceRb.AddForce(new Vector3(0, 1, -0.05f));
            pieceRb.AddTorque(Random.insideUnitSphere.normalized * 100);
            Destroy(bodyPieceClone, 3f);
        }
    }

    private void Death()
    {
        GetComponent<CapsuleCollider>().isTrigger = true;
        _rigidbody.useGravity = false;
        head.SetActive(false);
        headPrefab.transform.position = head.transform.position;
        headPrefab.SetActive(true);
        Rigidbody headRb = headPrefab.AddComponent<Rigidbody>();
        headRb.AddForce(new Vector3(0, 0, 3), ForceMode.Impulse);
        headPrefab.transform.parent = null;
        canMove = false;
        print("Game Over");
        DOTween.KillAll();
        _rigidbody.isKinematic = true;
        if (isCameFinalGround)
        {
            GameManager.Instance.LevelSuccess(diamondScore);
        }
        else
        {
            GameManager.Instance.LevelFail();
        }

    }

    public void Jump(float jumpForce)
    {
        if (onGround)
        {
            UpbodyStart();
            anim.SetBool("Jump", true);
            transform.forward = Vector3.forward;
            _rigidbody.AddForce((Vector3.forward + Vector3.up * 1.5f) * jumpForce, ForceMode.Impulse);
            onGround = false;
        }
    }

    public void FinalJump(float jumpForce)
    {
        upBody.transform.parent = transform;
        StartCoroutine(UpbodyFollow_Coroutine());
        Time.timeScale = 0.5f;
        DOTween.To(() => 1, x => Time.timeScale = x, 0.05f, 1f);
        upBody.GetComponent<Animator>().enabled = false;
        anim.SetBool("Kick", true);
        anim.SetBool("Run", false);
        transform.forward = Vector3.forward;
        Transform boss = GameObject.FindGameObjectWithTag("BossHead").transform;
        float jumpForceY = 3f;
        transform.DOJump(boss.transform.position, jumpForceY, 1, 1, false).SetEase(Ease.Linear).OnComplete(() =>
        {
            upBody.gameObject.GetComponent<Animator>().SetBool("Run", false);
            anim.SetBool("Kick", false);
            _rigidbody.isKinematic = true;
            finalUI.transform.parent = null;
            finalUI.SetActive(true);
            finalUI.transform.forward = -Camera.main.transform.forward;
            FindObjectOfType<Boss>().Die();
            StartCoroutine(OpenGravity_Coroutine());
            StartCoroutine(Finish_Coroutine());
            StartCoroutine(FinalUICameraLook());

        });
        //_rigidbody.AddForce((Vector3.forward + Vector3.up * 1.5f) * jumpForce, ForceMode.Impulse);
        onGround = false;

    }

    IEnumerator Finish_Coroutine()
    {
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.LevelSuccessBoss(diamondScore);

    }

    IEnumerator FinalUICameraLook()
    {
        while (true)
        {
            yield return null;
            finalUI.transform.LookAt(Camera.main.transform);
        }

    }

    IEnumerator UpbodyFollow_Coroutine()
    {
        while (true)
        {
            yield return null;
            upBody.transform.position = upbodyPivot.position - upbodyPivot.transform.up * 0.1f;
        }
    }

    public void StopMovement()
    {
        canMove = false;
    }

    public void StartMovement()
    {
        canMove = true;
    }

    private IEnumerator OpenGravity_Coroutine()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        _rigidbody.isKinematic = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (!onGround)
            {
                _rigidbody.velocity = Vector3.zero;
                anim.SetBool("Jump", false);
                StartMovement();
                onGround = true;
                UpbodyEnd();
            }
        }
        else if (collision.gameObject.CompareTag("FinalGround"))
        {
            isCameFinalGround = true;
            tweenTime = 0.08f;
            collision.gameObject.tag = "Untagged";
            UpbodyEnd();
            UpbodyStart();
            upBody.transform.localPosition = new Vector3(upBody.transform.localPosition.x, upBody.transform.localPosition.y, 0);
            upBody.GetComponent<Animator>().enabled = true;
            upBody.gameObject.GetComponent<Animator>().SetBool("Run", true);
            anim.SetBool("Run", true);
            canMove = false;
            transform.DOMove(new Vector3(0, transform.position.y, 50), 3.5f).SetEase(Ease.Linear).OnComplete(() => { FinalJump(6.4f); });
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Diamond diamond))
        {
            if (!diamond.isFinished)
            {
                diamondScore++;
                scoreText.text = diamondScore.ToString();
                diamond.Hit();
                //diamond toplama sesi 
            }

        }
        else if (other.gameObject.CompareTag("Boss"))
        {
            // Camera Up
            print("Camera Rotating Up");
            other.gameObject.tag = "Untagged";
            Camera.main.GetComponent<CameraMovement>().FinalMovement();
        }
    }

    public void UpbodyStart()
    {
        upBody.transform.parent = torso.transform;
    }
    public void UpbodyEnd()
    {
        upBody.transform.parent = transform;
        upBody.transform.position = new Vector3(transform.position.x, upBody.transform.position.y, transform.position.z);
    }

}
