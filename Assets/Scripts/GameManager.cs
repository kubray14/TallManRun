using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void StartGame()
    {
        
    }
    public void LevelSuccess()
    {
        FindObjectOfType<GameFinishUI>().LevelSuccess();
    }
    public void LevelFail()
    {
        FindObjectOfType<GameFinishUI>().LevelFail();
    }

    public void LevelFailed()
    {

    }
}
