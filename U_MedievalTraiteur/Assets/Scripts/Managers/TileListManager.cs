using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileListManager : MonoBehaviour
{
    [SerializeField] private List<TileScriptableObject> _tileList;
    [SerializeField] private GameObject _tileUIPrefab;
    [SerializeField] private Transform _tileUIParent;

    private void Start()
    {
        GenerateList();
    }

    private void GenerateList()
    {
        foreach (TileScriptableObject tileScriptableObject in _tileList)
        {
            GameObject newUITile = Instantiate(_tileUIPrefab, _tileUIParent);
            newUITile.GetComponent<TileUIObject>().SetupTileUIObject(tileScriptableObject);
        }
    }
}
