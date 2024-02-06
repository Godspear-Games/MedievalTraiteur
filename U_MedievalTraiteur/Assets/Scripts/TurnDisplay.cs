using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnDisplay : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _turnText;
    
    void Start()
    {
        EventManager.Instance.OnUpdateTurnCounter += UpdateTurnText;
    }
    
    private void OnDestroy()
    {
        EventManager.Instance.OnUpdateTurnCounter -= UpdateTurnText;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnUpdateTurnCounter -= UpdateTurnText;
    }
    
    private void UpdateTurnText(int turn, int maxTurns)
    {
        _turnText.text = "Turn " + turn + "/" + maxTurns;
    }
}
