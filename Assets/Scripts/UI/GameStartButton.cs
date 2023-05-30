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
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
