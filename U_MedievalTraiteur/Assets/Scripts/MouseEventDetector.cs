using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class MouseEventDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Tile _tile;
    
    private void Awake()
    {
        //Get tile in parent
        _tile = GetComponentInParent<Tile>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GridManager.Instance.SetSelectedTilePosition(new Vector2(transform.parent.position.x, transform.parent.position.z));
        _tile.MouseEnterMethod();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GridManager.Instance.SetSelectedTilePosition(Vector2.zero);
        _tile.MouseExitMethod();
    }
}
