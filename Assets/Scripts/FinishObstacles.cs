using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishObstacles : MonoBehaviour
{
    private FinishController cards;
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cards = FindAnyObjectByType<FinishController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            float randNum = Random.value < 0.5f ? 1 : -1;
            rb.AddForce(new Vector3(randNum * 2, 2, 2) * 1.5f, ForceMode.Impulse);
            rb.useGravity = true;
            player.GetFatOrSlim(-0.01f, false);
            player.StabilUpbody(-0.01f);
            cards.changeColor();
            gameObject.GetComponent<MeshCollider>().enabled = false;
            enabled = false;
        }
    }
}
