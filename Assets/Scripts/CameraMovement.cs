using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 5f;
    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - player.position;
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = player.position + offset;
        targetPosition.y = transform.position.y; // Eðer yükseklik deðiþmemesini istiyorsanýz y eksenini sabit tutun
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}
