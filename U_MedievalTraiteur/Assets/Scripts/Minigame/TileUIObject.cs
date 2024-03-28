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
        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        
        //check if tile is dropped on ui minigame grid

        if (CookingGridManager.Instance.TryTileUpdate(_tileType))
        {
            Debug.Log("Tile dropped on grid " + _tileType.name);
            IngredientBenchManager.Instance.RemoveTileFromHand(_tileType);
            Destroy(gameObject);
            IngredientBenchManager.Instance.RefillHand();
            IngredientBenchManager.Instance.RefreshVisualTileList();
        }
        else if (OrderManager.Instance.TryCompleteOrder(_tileType))
        {
            Debug.Log("Tile dropped on order " + _tileType.name);
            DishBenchManager.Instance.RemoveDishFromBench(_tileType);
            Destroy(gameObject);
            DishBenchManager.Instance.RefreshVisualTileList();
        }
        //if tile is dropped on invalid location return to starting position
        else
        {
            transform.position = _startingPosition;
            _uiImage.raycastTarget = true;
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position + _offsetToMousePointer;
    }
}
