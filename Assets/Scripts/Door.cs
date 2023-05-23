using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float value;
    [SerializeField] private bool isHeight;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerController playerController))
        {
            if (isHeight)
            {
                playerController.GetTallOrShort(value);
            }
            else
            {
                playerController.GetFatOrSlim(value);
            }

        }
    }
}
