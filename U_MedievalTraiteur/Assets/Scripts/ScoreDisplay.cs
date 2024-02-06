using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Serialization;

public class ScoreDisplay: MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    void Start()
    {
        EventManager.Instance.OnUpdateScore += UpdateScoreText;
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnUpdateScore -= UpdateScoreText;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnUpdateScore -= UpdateScoreText;
    }

    private void UpdateScoreText(int score, int maxScore)
    {
        _scoreText.text = score.ToString() +"/" + maxScore.ToString();
    }
}
