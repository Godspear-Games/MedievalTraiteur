using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private int _availableTiles;

    [SerializeField] private TileScriptableObject _lockedTile;
    [SerializeField] private TileScriptableObject _unlockedTile;
    [SerializeField] private TileScriptableObject _palaceTile;
    [SerializeField] private TileScriptableObject _fieldTile;

    private Dictionary<Vector2, Tile> _tiles;

    private Vector2 _selectedTilePosition;
    
    public static GridManager Instance;

    private TileListManager _tileListManager; 
    
    private void Awake()
    {
        _tileListManager = TileListManager.Instance;
        Instance = this;
    }

    private void Start()
    {
        GenerateGrid();

        // Request the TileListManager to present the first tile
        TileListManager.Instance.OnTilePlaced();
    }

    private void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Tile tile = Instantiate(_lockedTile.TilePrefab, new Vector3(x, 0, y), Quaternion.identity);
                tile.name = $"Tile {x} {y}";

                _tiles[new Vector2(x, y)] = tile;
            }
        }
        
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + (_width / 2f),Camera.main.transform.position.y+ 0, Camera.main.transform.position.z +(_height / 2f));
        SetStartingTiles();
    }

    private void SetStartingTiles()
    {
        Vector2 tileposition = new Vector2(_width / 2, _height / 2);
        Destroy(_tiles[tileposition].gameObject);
        _tiles[tileposition] = Instantiate(_palaceTile.TilePrefab, new Vector3(tileposition.x, 0, tileposition.y), Quaternion.identity);
        
        for (int i = 0; i < _availableTiles; i++)
        {
            tileposition = GetSurroundingTiles(new Vector2(_width / 2, _height / 2))[i];
            Destroy(_tiles[tileposition].gameObject);
            _tiles[tileposition] = Instantiate(_unlockedTile.TilePrefab, new Vector3(tileposition.x, 0, tileposition.y), Quaternion.identity);
        }
    }
    
    public List<Vector2> GetSurroundingTiles(Vector2 position)
    {
        List<Vector2> adjacentTiles = new List<Vector2>();

        // Loop through neighboring positions
        for (int xOffset = -1; xOffset <= 1; xOffset++)
        {
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                // Skip the central tile (0, 0)
                if (xOffset == 0 && yOffset == 0)
                    continue;

                // Add neighboring tiles
                adjacentTiles.Add(new Vector2(position.x + xOffset, position.y + yOffset));
            }
        }

        return adjacentTiles;
    }
    
    public Tile GetTileAtPosition(Vector2 position)
    {
        if (_tiles.TryGetValue(Vector2.positiveInfinity, out Tile tile))
        {
            return tile;
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

        if (_tiles.TryGetValue(position, out Tile tile))
        {
            Destroy(tile.gameObject);
            _tiles[position] = Instantiate(tileScriptableObject.TilePrefab, new Vector3(position.x, 0, position.y), Quaternion.identity);

            // Increment the score when a tile is successfully placed
            if (tileScriptableObject == _fieldTile)
            {
                ScoreManager.instance.IncreaseScore(); // Call the method in ScoreManager to increase the score
            }
        }
    }

    public void SetSelectedTilePosition(Vector2 position)
    {
        _selectedTilePosition = position;
    }
}
