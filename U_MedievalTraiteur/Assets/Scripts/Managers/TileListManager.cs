using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileListManager : MonoBehaviour
{
    [SerializeField] private List<TileScriptableObject> _tileList;
    [SerializeField] private GameObject _tileUIPrefab;
    [SerializeField] private Transform _tileUIParent;

    private Queue<TileScriptableObject> _shuffledTiles; // Use a queue to keep track of the order
    
    private List<GameObject> _currentTileUIObjects = new List<GameObject>();
    
    public static TileListManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ShuffleList(_tileList);
        _shuffledTiles = new Queue<TileScriptableObject>(_tileList);
    }

    private void PresentNextTile()
    {
        foreach (GameObject currenttileuiobject in _currentTileUIObjects)
        {
            Destroy(currenttileuiobject);
        }
        Debug.Log("PresentNextTile called");
        if (_shuffledTiles.Count > 0)
        {
            TileScriptableObject tileScriptableObject = _shuffledTiles.Dequeue();
            GameObject newUITile = Instantiate(_tileUIPrefab, _tileUIParent);
            newUITile.GetComponent<TileUIObject>().SetupTileUIObject(tileScriptableObject);
            _currentTileUIObjects.Add(newUITile);
        }
        else
        {
            // Reshuffle and refill the queue when all tiles are used
            ShuffleList(_tileList);
            foreach (var tile in _tileList)
            {
                _shuffledTiles.Enqueue(tile);
            }
        }
    }

    // Call this method when the player places a tile to present the next one
    public void OnTilePlaced()
    {
        if (_shuffledTiles.Count > 0)
        {
            PresentNextTile();
        }
        else
        {
            // Reshuffle and refill the queue when all tiles are used
            ShuffleList(_tileList);
            _shuffledTiles = new Queue<TileScriptableObject>(_tileList);
            PresentNextTile();
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
