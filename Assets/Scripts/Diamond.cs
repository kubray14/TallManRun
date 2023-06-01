using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    public bool isFinished = false;

    public void Hit()
    {
        isFinished = true;
        particle.Play();
        GetComponent<MeshRenderer>().enabled = false;
    }
}
