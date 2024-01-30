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

    [SerializeField] private PatternDefinition _testPattern;
    
    private Dictionary<Vector2, TileScriptableObject> _tileData;
    private Dictionary<Vector2, Tile> _tileObjects;

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
        //SetStartingTiles();
    }

    
    //todo update or remove this.
    private void SetStartingTiles()
    {

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
        
        //todo check grid for patterns
        List<Vector2> validPatternTiles = CheckForPatterns();

        if (validPatternTiles != null)
        {

            //Replace tiles with normal tiles and the output tile
            foreach (var validpatterntile in validPatternTiles)
            {
                if (_tileObjects.TryGetValue(validpatterntile, out Tile validtile))
                {
                    Destroy(validtile.gameObject);
                    _tileObjects[validpatterntile] = Instantiate(_unlockedTile.TilePrefab, new Vector3(validpatterntile.x, 0, validpatterntile.y), Quaternion.identity);
                    _tileData[validpatterntile] = _unlockedTile;
                }
            }
            
            //replace one random tile with the output tile
            int randomTile = UnityEngine.Random.Range(0, validPatternTiles.Count);
            if (_tileObjects.TryGetValue(validPatternTiles[randomTile], out Tile tileToReplace))
            {
                Destroy(tileToReplace.gameObject);
                _tileObjects[validPatternTiles[randomTile]] = Instantiate(_testPattern.OutputStructure.TilePrefab, new Vector3(validPatternTiles[randomTile].x, 0, validPatternTiles[randomTile].y), Quaternion.identity);
                _tileData[validPatternTiles[randomTile]] = _testPattern.OutputStructure;
            }

            foreach (var validpatterntile in validPatternTiles)
            {
                if (_tileObjects.TryGetValue(validpatterntile, out Tile validtile))
                {
                    //Do punch tween
                    LeanTween.scale(_tileObjects[validpatterntile].gameObject, new Vector3(0.8f, 0.8f, 0.8f), 0.5f).setEasePunch();
                    //Do 360 spin tween
                    LeanTween.rotateAround(_tileObjects[validpatterntile].gameObject, Vector3.up, 360f, 0.5f).setEaseInOutSine();

                }
            }
        }
    }

    public void CheckForPattern(PatternDefinition pattern)
    {
        //check each tile in the pattern against the grid


        //if pattern is found debug log pattern found
        Debug.Log("Pattern Found");
    }
    
    public void SetSelectedTilePosition(Vector2 position)
    {
        _selectedTilePosition = position;
    }

    List<Vector2> CheckForPatterns()
    {
        int gridRows = _tileData.Keys.Max(k => (int)k.x) + 1;
        int gridCols = _tileData.Keys.Max(k => (int)k.y) + 1;

        Debug.Log(gridRows);
        Debug.Log(gridCols);
        
        for (int rotation = 0; rotation < 4; rotation++) // 0, 90, 180, 270 degrees
        {
            for (int i = 0; i < gridRows; i++)
            {
                for (int j = 0; j < gridCols; j++)
                {
                    List<Vector2> validPatternTiles = CheckPatternAtPosition(new Vector2(i, j), _testPattern.InputTiles, gridRows, gridCols, rotation);
                    if (validPatternTiles != null)
                    {
                        Debug.Log("Pattern found!" + _testPattern.name);
                        return validPatternTiles; // Pattern matched with the specified rotation
                    }
                }
            }
        }

        return null; // Pattern not found in any rotation or position
    }

    List<Vector2> CheckPatternAtPosition(Vector2 startPosition, TileScriptableObject[,] pattern, int gridRows, int gridCols, int rotation)
    {
        int patternRows = pattern.GetLength(0);
        int patternCols = pattern.GetLength(1);

        List<Vector2> validPatternTiles = new List<Vector2>();
        
        for (int i = 0; i < patternRows; i++)
        {
            for (int j = 0; j < patternCols; j++)
            {
                Vector2 rotatedPosition = RotatePosition(startPosition, i, j, patternRows, patternCols, rotation);

                if (rotatedPosition.x >= 0 && rotatedPosition.x < gridRows &&
                    rotatedPosition.y >= 0 && rotatedPosition.y < gridCols)
                {
                    TileScriptableObject patternTile = pattern[i, j];

                    if (patternTile != null)
                    {
                        if (!_tileData.TryGetValue(rotatedPosition, out TileScriptableObject tileScriptableObject))
                        {
                            return null; // Rotated position not found in the dictionary
                        }

                        if (tileScriptableObject == null || tileScriptableObject != patternTile)
                        {
                            return null; // Pattern doesn't match at this rotated position
                        }
                    }
                    // If patternTile is null, treat it as a wildcard and accept any tile at this position
                }
                else if (pattern[i, j] != null)
                {
                    return null; // Pattern exceeds grid boundaries
                }
                
                //if this tile is not null add it to the list of tiles to tween
                if (pattern[i, j] != null)
                {
                    validPatternTiles.Add(rotatedPosition);
                }
            }
        }

        return validPatternTiles; // Pattern matched with the specified rotation
    }

    Vector2 RotatePosition(Vector2 position, int i, int j, int patternRows, int patternCols, int rotation)
    {
        switch (rotation)
        {
            case 0: return new Vector2(position.x + j, position.y - i);
            case 1: return new Vector2(position.x - i, position.y - j);
            case 2: return new Vector2(position.x - j, position.y + i);
            case 3: return new Vector2(position.x + i, position.y + j);
            default: return position;
        }
    }
}
