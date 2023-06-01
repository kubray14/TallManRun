using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject levelSuccessBossPanel;
    [SerializeField] private GameObject levelSuccessPanel;
    [SerializeField] private GameObject levelFailPanel;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text finalScoreBossText;
    [SerializeField] private TMP_Text finalScoreBonusBossText;
    private float scoreMultiplier = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void LevelSuccessBoss(float score)
    {
        levelSuccessBossPanel.SetActive(true);
        finalScoreBonusBossText.text = "x " + ((int)(score * scoreMultiplier)).ToString();
        finalScoreBossText.text = "x " + "500";
        //finalScoreBossText.text =
    }

    public void LevelSuccess(float score)
    {
        levelSuccessPanel.SetActive(true);
        finalScoreText.text = "x " + ((int)(score * scoreMultiplier)).ToString();
    }

    public void LevelFail()
    {
        levelFailPanel.SetActive(true);
        levelFailPanel.GetComponent<Image>().DOFade(0.75f, 1.5f);

    }

    public void IncreaseScoreMultiplier(int multiplierValue)
    {
        scoreMultiplier = 1 + (multiplierValue * 0.2f);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
