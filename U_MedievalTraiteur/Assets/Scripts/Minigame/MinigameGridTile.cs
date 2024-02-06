using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinigameGridTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TileScriptableObject _tileType;
    [SerializeField] private Image _tileImage;
    [SerializeField] private GameObject _highlight;


    private void Start()
    {
        if (_highlight)
        {
            _highlight.SetActive(false);
            _tileImage.color = Color.gray;
        }
    }

    public void SetTileType(TileScriptableObject tileType)
    {
        _tileType = tileType;
        if (_tileType != null)
        {
            _tileImage.sprite = tileType.UISprite;
            _tileImage.color = Color.white;
        }
        else
        {
            _tileImage.sprite = null;
            _tileImage.color = Color.gray;
        }

    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        MinigameGridManager.Instance.SetSelectedTile(this);
        _highlight.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MinigameGridManager.Instance.SetSelectedTile(null);
        _highlight.SetActive(false);
    }
}
