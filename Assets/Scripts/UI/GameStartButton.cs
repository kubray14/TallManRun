using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameStartButton : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        StartGame();
    }

    public void StartGame()
    {
        GameManager.Instance.StartGame();
        gameObject.SetActive(false);
    }
}
