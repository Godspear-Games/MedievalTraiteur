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
        ShuffleList(_tileList);
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

    // Fisher-Yates shuffle algorithm to shuffle the list
    private void ShuffleList(List<TileScriptableObject> list)
    {
        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            TileScriptableObject temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
