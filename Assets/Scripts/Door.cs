using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float value;
    [SerializeField] private bool isHeight;
    [SerializeField] private bool isMultiplier;
    private TMP_Text text;

    private void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
        if (value < 0)
        {
            if (isMultiplier)
            {
                text.text = (value).ToString();
            }
            else
            {
                text.text = (value * -100).ToString();
            }

        }
        else
        {
            if (isMultiplier)
            {
                text.text = (value).ToString();
            }
            else
            {
                text.text = (value * 100).ToString();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerController playerController))
        {
            if (isHeight)
            {
                playerController.GetTallOrShort(value, isMultiplier);
            }
            else
            {
                playerController.GetFatOrSlim(value, isMultiplier);
            }

        }
    }
}
