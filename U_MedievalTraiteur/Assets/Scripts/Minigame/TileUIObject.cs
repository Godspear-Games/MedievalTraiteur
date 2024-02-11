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

        if (MinigameGridManager.Instance.TryTileUpdate(_tileType))
        {
            TileListManager.Instance.RemoveTileFromHand(_tileType);
            TileListManager.Instance.DoneAddingTiles();
            TileListManager.Instance.OnTilePlaced();
            ScoreManager.Instance.TurnCompleted();
        }
        else if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag("SceneGridTile") && _tileType.IsStructure)
        {
            Debug.Log("Tile dropped on grid " + _tileType.name);
                
            ScoreManager.Instance.AddToScore(_tileType.SoulValue);
            GridManager.Instance.FillTile(hit.transform.gameObject.GetComponentInParent<Tile>(), _tileType);
            TileListManager.Instance.RemoveTileFromHand(_tileType);
            TileListManager.Instance.DoneAddingTiles();
            TileListManager.Instance.OnTilePlaced();
            ScoreManager.Instance.TurnCompleted(false);
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
