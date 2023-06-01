using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Transform hitPoint;
    private bool isHitted = false;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isHitted)
        {
            if (other.gameObject.transform.root.TryGetComponent(out PlayerController playerController))
            {
                if(!transform.CompareTag("Wall"))
                {
                    float randNum = Random.value < 0.5f ? 1 : -1;
                    rb.AddForce(new Vector3(randNum * 2, 2, 2) * 1.5f, ForceMode.Impulse);
                    rb.useGravity = true;
                    Destroy(gameObject, 3f);
                }
                playerController.Hit(hitPoint);
                isHitted = true;
            }
        }
    }
}
