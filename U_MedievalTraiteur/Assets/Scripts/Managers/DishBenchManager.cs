using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

public class DishBenchManager : MonoBehaviour
{
    public static DishBenchManager Instance = null;
    private List<GameObject> _currentTileUIObjects = new List<GameObject>();
    [SerializeField] private GameObject _tileUIPrefab;
    [SerializeField] private Transform _tileUIParent;
    private List<TileScriptableObject> _dishBenchQueue = new List<TileScriptableObject>();
    [FormerlySerializedAs("_maxTilesInHand")] [SerializeField] private int _maxDishesOnBench = 5;

    private void Awake()
    {
        Instance = this;
    }
    
    public void RefreshVisualTileList()
    {
        for (int i = _currentTileUIObjects.Count-1; i >=0 ; i--)
        {
            Destroy(_currentTileUIObjects[i]);
        }
        _currentTileUIObjects.Clear();
        
        foreach (TileScriptableObject dish in _dishBenchQueue)
        {
            GameObject newUITile = Instantiate(_tileUIPrefab, _tileUIParent);
            newUITile.GetComponent<TileUIObject>().SetupTileUIObject(dish);
            _currentTileUIObjects.Add(newUITile);
        }
    }

    public void AddDish(TileScriptableObject newdish)
    {
        //if bench will exceed max dishes, remove the oldest dish and add the new one
        if (_dishBenchQueue.Count >= _maxDishesOnBench)
        {
            _dishBenchQueue.RemoveAt(0);
        }
        _dishBenchQueue.Add(newdish);
        RefreshVisualTileList();
    }
    
    public void RemoveDishFromBench(TileScriptableObject dish)
    {
        //todo change this to an index based removal to avoid issues with multiple of the same dish
        _dishBenchQueue.Remove(dish);
        RefreshVisualTileList();
    }
}
