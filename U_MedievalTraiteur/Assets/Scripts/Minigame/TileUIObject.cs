using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileUIObject : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Image _uiImage;
    private Vector2 _startingPosition;
    private Vector2 _offsetToMousePointer;
    private TileScriptableObject _tileType;
    

    public void SetupTileUIObject(TileScriptableObject tileScriptableObject)
    {
        _uiImage.sprite = tileScriptableObject.UISprite;
        _tileType = tileScriptableObject;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        _startingPosition = transform.position;
        _offsetToMousePointer = (Vector2)transform.position - eventData.position;
        _uiImage.raycastTarget = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = _startingPosition;
        _uiImage.raycastTarget = true;
        
        if (MinigameGridManager.Instance.TryTileUpdate(_tileType))
        {
            TileListManager.Instance.OnTilePlaced();
        }

        // Add the SoulValue to the total score
        // if (_tileType != null)
        // {
        //     ScoreManager.Instance.AddToScore(_tileType.SoulValue);
        // }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position + _offsetToMousePointer;
    }
}
