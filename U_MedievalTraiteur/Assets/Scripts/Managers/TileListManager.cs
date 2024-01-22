using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileListManager : MonoBehaviour
{
    [SerializeField] private List<TileScriptableObject> _tileList;
    [SerializeField] private GameObject _tileUIPrefab;
    [SerializeField] private Transform _tileUIParent;

    private int currentTileIndex = 0;

    private void Start()
    {
        ShuffleList(_tileList);
        PresentNextTile();
    }

    private void PresentNextTile()
    {
        if (currentTileIndex < _tileList.Count)
        {
            TileScriptableObject tileScriptableObject = _tileList[currentTileIndex];
            GameObject newUITile = Instantiate(_tileUIPrefab, _tileUIParent);
            newUITile.GetComponent<TileUIObject>().SetupTileUIObject(tileScriptableObject);
            currentTileIndex++;
        }
        else
        {
            // Reshuffle and reset the index to 0
            ShuffleList(_tileList);
            currentTileIndex = 0;
            PresentNextTile();
        }
    }

    // Call this method when the player places a tile to present the next one
    public void OnTilePlaced()
    {
        PresentNextTile();
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
