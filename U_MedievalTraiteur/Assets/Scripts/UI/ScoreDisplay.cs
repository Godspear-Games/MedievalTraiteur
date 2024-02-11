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
        string text = score + "/" + maxScore;
        if(_scoreText.text != text)
        {
            LeanTween.scale(_scoreText.gameObject, new Vector3(1.2f, 1.2f, 1.2f), 0.5f).setEasePunch();
            _scoreText.text = score +"/" + maxScore;
        }
    }
}
