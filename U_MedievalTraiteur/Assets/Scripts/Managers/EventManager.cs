using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    
    public static EventManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    
    public event Action<int, int> OnUpdateScore;
    
    public void UpdateScore(int score, int maxScore)
    {
        OnUpdateScore?.Invoke(score, maxScore);
    }
    
    public event Action<int, int> OnUpdateTurnCounter;
    
    public void UpdateTurnCounter(int turn, int maxTurns)
    {
        OnUpdateTurnCounter?.Invoke(turn, maxTurns);
    }

    public event Action<TileScriptableObject> OnShowPopup;

    public void ShowPopup(TileScriptableObject tile)
    {
        OnShowPopup?.Invoke(tile);
    }
    
    public event Action<int> OnGameOver;
    
    public void GameOver(int totalscore)
    {
        OnGameOver?.Invoke(totalscore);
    }
    
    public event Action<int> OnMilestoneReached;
    public void MilestoneReached(int nextmilestone)
    {
        OnMilestoneReached?.Invoke(nextmilestone);
    }

}
