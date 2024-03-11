using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    [SerializeField] private int _totalScore = 0;

    [SerializeField] private List<int> _scoreMilestones;
    [SerializeField] private int _amountOfTurns = 10;
    
    private int _turnCounter = 0;
    private int _currentMilestone = 0;
    private void Awake()
    {
        Instance = this; 
    }

    //call this when a tile is dropped on the cashout field
    public void AddToScore(int score)
    {
        _totalScore += score;
        EventManager.Instance.UpdateScore(_totalScore, _scoreMilestones[_currentMilestone]);
        //CheckMileStone is needed for the new idea
        //CheckMilestone();
    }

    public int GetScore()
    {
        return _totalScore;
    }
    
    //Instead of turns, you only need to reach milestones
    /*private void CheckMilestone()
    {
        if (_currentMilestone < _scoreMilestones.Count && _totalScore >= _scoreMilestones[_currentMilestone])
        {
            _currentMilestone++;
            EventManager.Instance.MilestoneReached(_scoreMilestones[_currentMilestone]);
        }

        if (_currentMilestone >= _scoreMilestones.Count)
        {
            EventManager.Instance.GameOver(_totalScore);
        }
    }*/

    public void TurnCompleted(bool isvalidturn = true)
    {
        if (isvalidturn)
        {
            _turnCounter++;
        }
        EventManager.Instance.UpdateTurnCounter(_turnCounter, _amountOfTurns);
        if (_turnCounter > _amountOfTurns)
        {
            if (_totalScore >= _scoreMilestones[_currentMilestone])
            {
                _currentMilestone++;
                _turnCounter = 0;
                // send milestone reached event
                EventManager.Instance.MilestoneReached(_scoreMilestones[_currentMilestone]);
            }
            else
            {
                // send game over event
                EventManager.Instance.GameOver(_totalScore);
            }
        }
    }
    
}
