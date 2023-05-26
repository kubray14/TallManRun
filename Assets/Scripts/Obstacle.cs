using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Transform hitPoint;
    private bool isHitted = false;  

    private void OnTriggerEnter(Collider other)
    {
        if (!isHitted)
        {
            if (other.gameObject.transform.root.TryGetComponent(out PlayerController playerController))
            {
                playerController.Hit(hitPoint);
                isHitted = true;
            }
        }
    }
}
