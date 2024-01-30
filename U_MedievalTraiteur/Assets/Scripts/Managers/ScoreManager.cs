using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    [SerializeField] private int _totalScore = 0;

    private void Awake()
    {
        Instance = this; 
    }

    public void AddToScore(int score)
    {
        _totalScore += score;
        // send UpdateScore event
        EventManager.Instance.UpdateScore(_totalScore);
    }

    public int GetScore()
    {
        return _totalScore;
    }
}
