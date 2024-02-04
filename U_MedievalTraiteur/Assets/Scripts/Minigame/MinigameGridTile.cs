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
    
    private Vector2 _startingPosition;
    private Vector2 _offsetToMousePointer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SetTileType(TileScriptableObject tileType)
    {
        _tileType = tileType;
        if (_tileType != null)
        {
            _tileImage.sprite = tileType.UISprite;
        }
        else
        {
            _tileImage.sprite = null;
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
