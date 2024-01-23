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

    private TileListManager _tileListManager;

    private void Awake()
    {
        _tileListManager = TileListManager.Instance;
    }

    public void SetupTileUIObject(TileScriptableObject tileScriptableObject)
    {
        _uiImage.color = tileScriptableObject.UIColor;
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
        GridManager.Instance.TryTileUpdate(_tileType);
        TileListManager.Instance.OnTilePlaced();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position + _offsetToMousePointer;
    }
}
