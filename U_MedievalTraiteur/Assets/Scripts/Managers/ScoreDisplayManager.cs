using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreDisplayManager: MonoBehaviour
{
    public static ScoreDisplayManager Instance { get; private set; }
    public TMP_Text scoreText;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateScoreText(ScoreManager.Instance.TotalScore);
    }

    public void UpdateScoreText(int score)
    {
        scoreText.text = score.ToString();
    }
}
