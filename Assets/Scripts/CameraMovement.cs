using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 5f;
    public Vector3 offset;
    public bool isFinished = false;
    public bool isFinishedLast = false;

    private void Start()
    {
        offset = transform.position - player.position;
    }

    private void LateUpdate()
    {
        if (!isFinishedLast)
        {
            if (!isFinished)
            {
                Vector3 targetPosition = player.position + offset;
                transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            }
            else
            {
                Vector3 targetPosition = player.position + offset;
                targetPosition.x = transform.position.x;
                transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            }
        }
    }

    public void calcOffset()
    {
        offset = transform.position - player.position;
        isFinished = false;
    }

    public void FinalMovement()
    {
        isFinishedLast = true;
        float tweenTime = 2f;
        transform.DOMove(new Vector3(0.3f, 6.2f, 56.5f), tweenTime).SetEase(Ease.Linear).SetUpdate(true);
        transform.DORotateQuaternion(Quaternion.Euler(75, Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z), tweenTime).SetEase(Ease.Linear).SetUpdate(true);
    }
}
