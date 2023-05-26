using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float value;
    [SerializeField] private bool isHeight;
    private TMP_Text text;

    private void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
        if (value < 1)
        {
            text.text = (value * -1).ToString();
        }
        else
        {
            text.text = value.ToString();
        }
    }
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
