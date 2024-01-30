using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Serialization;

public class ScoreDisplayManager: MonoBehaviour
{
    public static ScoreDisplayManager Instance { get; private set; }
    [SerializeField] private TMP_Text _scoreText;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        EventManager.Instance.OnUpdateScore += UpdateScoreText;
        UpdateScoreText(ScoreManager.Instance.GetScore());
    }

    public void UpdateScoreText(int score)
    {
        _scoreText.text = score.ToString();
    }
}
