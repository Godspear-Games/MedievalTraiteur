using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinigameGridTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TileScriptableObject _tileType;
    [SerializeField] private Image _tileImage;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private Image _ingredientImage;

    private Vector3 _initialScale;
    
    private bool _isAvailable = true;
    
    public void setAvailable(bool available)
    {
        _highlight.SetActive(false);
        _isAvailable = available;
        _tileImage.color = available ? Color.white : Color.clear;
    }

    public void SetTileType(TileScriptableObject tileType)
    {
        _initialScale = _ingredientImage.rectTransform.localScale;
        _tileType = tileType;
        if (_tileType != null)
        {
            _ingredientImage.rectTransform.localScale = _initialScale*1.2f;
            LeanTween.scale(_ingredientImage.rectTransform, _initialScale, 0.3f).setEase(LeanTweenType.easeInQuart).setOnComplete(
                () =>
                {
                    LeanTween.scale(_tileImage.rectTransform, Vector3.one * 0.8f, 0.4f).setEasePunch();
                });
            _ingredientImage.sprite = tileType.UISprite;
            _ingredientImage.color = Color.white;
        }
        else
        {
            _ingredientImage.sprite = null;
            _ingredientImage.color = Color.clear;
        }

    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isAvailable)
        {
            CookingGridManager.Instance.SetSelectedTile(this);
            _highlight.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isAvailable)
        {
            CookingGridManager.Instance.SetSelectedTile(null);
            _highlight.SetActive(false);
        }

    }
}
