using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFinishUI : MonoBehaviour
{
    [SerializeField] private GameObject levelSuccessPanel;
    [SerializeField] private GameObject levelFailPanel;

    private void Start()
    {
        levelSuccessPanel.SetActive(false);
        levelFailPanel.SetActive(false);
    }
    public void LevelSuccess()
    {
        levelSuccessPanel.SetActive(true);
    }
    public void LevelFail()
    {
        levelFailPanel.SetActive(true);
    }
}
