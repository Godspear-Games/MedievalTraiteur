using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System.Linq;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private int _availableTiles;

    [SerializeField] private TileScriptableObject _lockedTile;
    [SerializeField] private TileScriptableObject _unlockedTile;
    [SerializeField] private TileScriptableObject _palaceTile;
    [SerializeField] private TileScriptableObject _fieldTile;

    [SerializeField] private PatternDefinitionScriptableObject _testPattern;
    
    private Dictionary<Vector2, TileScriptableObject> _tileData;
    private Dictionary<Vector2, Tile> _tileObjects;

    private Vector2 _selectedTilePosition;
    
    public static GridManager Instance;
    
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GenerateGrid();

        // Request the TileListManager to present the first tile
        //todo temporarily commented to rework the logic, should this really be here?
        // TileListManager.Instance.OnTilePlaced();
    }

    private void GenerateGrid()
    {
        _tileData = new Dictionary<Vector2, TileScriptableObject>();
        _tileObjects = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Tile tile = Instantiate(_unlockedTile.TilePrefab, new Vector3(x, 0, y), Quaternion.identity);
                tile.name = $"Tile {x} {y}";

                _tileData[new Vector2(x, y)] = _unlockedTile;
                _tileObjects[new Vector2(x, y)] = tile;
            }
        }
        
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + (_width / 2f),Camera.main.transform.position.y+ 0, Camera.main.transform.position.z +(_height / 2f));
    }

    public TileScriptableObject GetTileAtPosition(Vector2 position)
    {
        if (_tileData.TryGetValue(Vector2.positiveInfinity, out TileScriptableObject tiledata))
        {
            return tiledata;
        }

        return null;
    }

    public void TryTileUpdate(TileScriptableObject tileScriptableObject)
    {
        SetTileType(_selectedTilePosition, tileScriptableObject);
    }

    public void SetTileType(Vector2 position, TileScriptableObject tileScriptableObject)
    {
        if (position == Vector2.zero)
        {
            return;
        }

        if (_tileObjects.TryGetValue(position, out Tile tile))
        {
            Destroy(tile.gameObject);
            _tileObjects[position] = Instantiate(tileScriptableObject.TilePrefab, new Vector3(position.x, 0, position.y), Quaternion.identity);
            _tileData[position] = tileScriptableObject;
        }
    }

    public void SetSelectedTilePosition(Vector2 position)
    {
        _selectedTilePosition = position;
    }
}
