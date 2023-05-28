using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    [SerializeField] private float jumpForce = 7;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerController playerController))
        {
            other.transform.rotation = Quaternion.Euler(0, 0, 0);
            other.transform.position = new Vector3(transform.position.x, other.transform.position.y, transform.position.z);
            playerController.Jump(jumpForce);
            playerController.StopMovement();
        }
    }
}
