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
            SoundManager.instance.PlayObstacleSound();
            float randNum = Random.value < 0.5f ? 1 : -1;
            rb.AddForce(new Vector3(randNum * 2, 2, 2) * 1.5f, ForceMode.Impulse);
            rb.useGravity = true;
            float value = -0.1f;
            player.GetTallOrShort(value, false);
            cards.changeColor();
            Destroy(gameObject, 2f);
            gameObject.GetComponent<MeshCollider>().enabled = false;
            enabled = false;
            Destroy(gameObject, 2f);
        }
    }
}
