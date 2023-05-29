using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalJumper : MonoBehaviour
{
    [SerializeField] private float jumpForce = 7;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerController playerController))
        {
            playerController.FinalJump(jumpForce);
            playerController.StopMovement();
        }
    }
}
