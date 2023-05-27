using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;

    public void Hit()
    {
        particle.Play();
        GetComponent<MeshRenderer>().enabled = false;
    }
}
