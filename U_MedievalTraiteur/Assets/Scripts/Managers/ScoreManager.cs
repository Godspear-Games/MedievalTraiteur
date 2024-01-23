using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public int TotalScore { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optionally, keep the score manager between scenes.
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddToScore(int score)
    {
        TotalScore += score;
    }
}
