using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public int TotalScore { get; private set; }

    private void Awake()
    {
        Instance = this; 
    }

    public void AddToScore(int score)
    {
        TotalScore += score;
        // Update the ScoreDisplayManager to display the updated score
        ScoreDisplayManager.Instance.UpdateScoreText(TotalScore);
    }
}
