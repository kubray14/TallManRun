using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] private AudioClip diamondClip;
    [SerializeField] private AudioClip doorClip;
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayDiamondSound()
    {
        audioSource.PlayOneShot(diamondClip);
    }
    public void PlayDoorSound()
    {
        audioSource.PlayOneShot(doorClip);
    }
}
