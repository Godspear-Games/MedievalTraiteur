using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoulCollectionUI : MonoBehaviour
{
    [SerializeField] private GameObject _continueButton;
    [SerializeField] private GameObject _restartButton;
    [SerializeField] private GameObject _panel;
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _milestoneText;

    // Start is called before the first frame update
    void Start()
    {
        _panel.SetActive(false);
        EventManager.Instance.OnGameOver += ShowGameOver;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnGameOver -= ShowGameOver;
    }
    
    private void OnDestroy()
    {
        EventManager.Instance.OnGameOver -= ShowGameOver;
    }
    
    private void ShowMilestoneReached(int nextMilestone)
    {
        _continueButton.SetActive(true);
        _restartButton.SetActive(false);
        _panel.SetActive(true);
        _titleText.text = "Well done. Keep giving me souls and perhaps I'll give you eternal life.";
        _milestoneText.text = "Next time, I want " + nextMilestone + " souls";
    }

    private void ShowGameOver(int score)
    {
        _continueButton.SetActive(false);
        _restartButton.SetActive(true);
        _panel.SetActive(true);
        _titleText.text = "Only " +score + " souls?! Back to the void you go.";
        if (score == -1)
        {
            _titleText.text =
                "Seems like you got a little stuck there... I'll send you back to the void so you can ponder about your mistake.";
        }
        
        _milestoneText.text = "Maybe the next lucky soul will be able to help me...";
    }
    
    public void ContinueButton()
    {
        _panel.SetActive(false);
    }
    
    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
