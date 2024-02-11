using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    private TileScriptableObject _tileScriptableObject;
    [SerializeField] private GameObject _emptyTilePrefab;
    [SerializeField] private Transform _modelParent;

    public TileScriptableObject GetTileScriptableObject()
    {
        return _tileScriptableObject;
    }

    public void SetupTile()
    {
        //logic to setup the tile if tile is empty
        if (_tileScriptableObject == null)
        {
            SpawnEmptyTileModel();
        }
        //logic to setup the tile if tile is not empty
        else
        {
            SpawnTileModel();
        }
    }

    private void SpawnEmptyTileModel()
    {
        Instantiate(_emptyTilePrefab, _modelParent);
        LeanTween.cancel(_modelParent.gameObject);
        _modelParent.localScale = Vector3.zero;
        LeanTween.scale(_modelParent.gameObject, Vector3.one * 0.95f, 0.25f).setEaseOutBack();
    }
    
    private void SpawnTileModel()
    {
        //delete any existing model
        foreach (Transform child in _modelParent)
        {
            Destroy(child.gameObject);
        }
        
        Instantiate(_tileScriptableObject.TilePrefab, _modelParent);
        LeanTween.cancel(_modelParent.gameObject);
        _modelParent.localScale = Vector3.zero;
        LeanTween.scale(_modelParent.gameObject, Vector3.one, 0.25f).setEaseOutBack();
    }
    
    public void FillTile(TileScriptableObject tileScriptableObject)
    {
        _tileScriptableObject = tileScriptableObject;
    }

    // public void OnPointerEnter(PointerEventData eventData)
    // {
    //     GridManager.Instance.SetSelectedTile(this);
    // }

    // public void OnPointerExit(PointerEventData eventData)
    // {
    //     GridManager.Instance.DeselectTile(this);
    // }
}
