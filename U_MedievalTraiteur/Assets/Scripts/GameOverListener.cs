using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverListener : MonoBehaviour
{
    [SerializeField] private List<GameObject> _gameOverPanelTweenObjects = new List<GameObject>();
    [SerializeField] private RectTransform _gameOverPanelBackground;

    private Color _originalBackgroundColor;

    private void Start()
    {
        _gameOverPanelBackground.GetComponent<Image>().raycastTarget = false;
        _originalBackgroundColor = _gameOverPanelBackground.GetComponent<Image>().color;
        
        //set color of background to clear
        _gameOverPanelBackground.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        
        //set scale of every _gameOverPanelTweenObject to 0
        foreach (GameObject obj in _gameOverPanelTweenObjects)
        {
            obj.transform.localScale = new Vector3(1, 0, 1);
        }
        
        EventManager.Instance.OnShowEndScreen+= Show;
    }

    private void Hide()
    {
        //tween color of background to clear
        LeanTween.color(_gameOverPanelBackground, new Color(0, 0, 0, 0), 0.5f).setEaseOutSine();
        
        //tween scale of every _gameOverPanelTweenObject to 0, first X to 0.01 then Y to 0
        foreach (GameObject obj in _gameOverPanelTweenObjects)
        {
            LeanTween.scaleX(obj, 0.01f, 0.5f).setEaseOutSine().setOnComplete(() =>
            {
                LeanTween.scaleY(obj, 0, 0.5f).setEaseOutSine();
            });
        }
    }

    private void Show(int score)
    {
        //tween color of background to original color (only change color of background object)
        LeanTween.color(_gameOverPanelBackground, _originalBackgroundColor, 0.5f).setEaseOutSine();

        //tween scale of every _gameOverPanelTweenObject to 1, first Y to 1 then X to 1
        foreach (GameObject obj in _gameOverPanelTweenObjects)
        {
            LeanTween.scaleY(obj, 1, 0.5f).setEaseOutSine().setOnComplete(() =>
            {
                LeanTween.scaleX(obj, 1, 0.5f).setEaseOutSine();
            });
        }
    }
    
}
