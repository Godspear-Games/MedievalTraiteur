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

    public event Action<PatternDefinitionScriptableObject> OnShowPopup;

    public void ShowPopup(PatternDefinitionScriptableObject patternDefinitionScriptableObject)
    {
        OnShowPopup?.Invoke(patternDefinitionScriptableObject);
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

    public event Action<PatternDefinitionScriptableObject> OnAddHintPattern;
    public void AddHintPattern(PatternDefinitionScriptableObject patternDefinitionScriptableObject)
    {
        OnAddHintPattern?.Invoke(patternDefinitionScriptableObject);
    }
    

}
