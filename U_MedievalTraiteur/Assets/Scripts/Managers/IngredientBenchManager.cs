using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class IngredientBenchManager : MonoBehaviour
{
    [FormerlySerializedAs("_tileDeck")] [SerializeField] private List<TileDeckElement> _tileDeckElements;
    private List<TileScriptableObject> _tileDeck = new List<TileScriptableObject>();
    private List<TileScriptableObject> _tileHand = new List<TileScriptableObject>();
    [SerializeField] private int _maxTilesInHand = 5;
    [SerializeField] private GameObject _tileUIPrefab;
    [SerializeField] private Transform _tileUIParent;

    private Queue<TileScriptableObject> _shuffledTiles; // Use a queue to keep track of the order
    
    private List<GameObject> _currentTileUIObjects = new List<GameObject>();
    
    public static IngredientBenchManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SetupIngredients()
    {
        GenerateDeck();
        RefillDeck();
        RefillHand();
        RefreshVisualTileList();
    }

    private void GenerateDeck()
    {
        foreach (TileDeckElement tileDeckElement in _tileDeckElements)
        {
            for (int i = 0; i < tileDeckElement.Amount; i++)
            {
                _tileDeck.Add(tileDeckElement.Tilescriptableobject);
            }
        }
    }
    
    public void RefillHand()
    {
        for (int i = _tileHand.Count; i < _maxTilesInHand; i++)
        {
            TileScriptableObject tileScriptableObject = _shuffledTiles.Dequeue();
            AddNewTileToHand(tileScriptableObject);
        }
        if (_shuffledTiles.Count <= 0)
        {
            RefillDeck();
        }
    }

    public void RefillDeck()
    {
        ShuffleList(_tileDeck);
        _shuffledTiles = new Queue<TileScriptableObject>(_tileDeck);
    }

    public void RefreshVisualTileList()
    {
        for (int i = _currentTileUIObjects.Count-1; i >=0 ; i--)
        {
            Destroy(_currentTileUIObjects[i]);
        }
        _currentTileUIObjects.Clear();
        
        foreach (TileScriptableObject tile in _tileHand)
        {
            GameObject newUITile = Instantiate(_tileUIPrefab, _tileUIParent);
            newUITile.GetComponent<TileUIObject>().SetupTileUIObject(tile);
            _currentTileUIObjects.Add(newUITile);
        }
    }

    public void RemoveTileFromHand(TileScriptableObject tile)
    {
        _tileHand.Remove(tile);
    }
    
    public void AddNewTileToHand(TileScriptableObject tile)
    {
        if (tile != null)
        {
            _tileHand.Add(tile);
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
    
    [Serializable]
    public class TileDeckElement
    {
        public TileScriptableObject Tilescriptableobject;
        
        //only show this field if the tilescriptableobject isn't null using odin
        [ShowIf("tileisnotnull")]
        public int Amount;
        
        private bool tileisnotnull => Tilescriptableobject != null;
    }

}
