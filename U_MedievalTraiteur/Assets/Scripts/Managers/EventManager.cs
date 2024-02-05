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
    
    public event Action<int> OnUpdateScore;
    
    public void UpdateScore(int score)
    {
        OnUpdateScore?.Invoke(score);
    }

    public event Action<TileScriptableObject> OnShowPopup;

    public void ShowPopup(TileScriptableObject tile)
    {
        OnShowPopup?.Invoke(tile);
    }
}
