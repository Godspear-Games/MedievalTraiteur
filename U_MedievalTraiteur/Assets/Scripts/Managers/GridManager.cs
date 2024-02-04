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
        
        // //todo check grid for patterns
        // List<Vector2> validPatternTiles = CheckForPatterns();

        // if (validPatternTiles != null)
        // {
        //
        //     //Replace tiles with normal tiles and the output tile
        //     foreach (var validpatterntile in validPatternTiles)
        //     {
        //         if (_tileObjects.TryGetValue(validpatterntile, out Tile validtile))
        //         {
        //             Destroy(validtile.gameObject);
        //             _tileObjects[validpatterntile] = Instantiate(_unlockedTile.TilePrefab, new Vector3(validpatterntile.x, 0, validpatterntile.y), Quaternion.identity);
        //             _tileData[validpatterntile] = _unlockedTile;
        //         }
        //     }
        //     
        //     //replace one random tile with the output tile
        //     int randomTile = UnityEngine.Random.Range(0, validPatternTiles.Count);
        //     if (_tileObjects.TryGetValue(validPatternTiles[randomTile], out Tile tileToReplace))
        //     {
        //         Destroy(tileToReplace.gameObject);
        //         _tileObjects[validPatternTiles[randomTile]] = Instantiate(_testPattern.OutputStructure.TilePrefab, new Vector3(validPatternTiles[randomTile].x, 0, validPatternTiles[randomTile].y), Quaternion.identity);
        //         _tileData[validPatternTiles[randomTile]] = _testPattern.OutputStructure;
        //     }
        //
        //     foreach (var validpatterntile in validPatternTiles)
        //     {
        //         if (_tileObjects.TryGetValue(validpatterntile, out Tile validtile))
        //         {
        //             //Do punch tween
        //             LeanTween.scale(_tileObjects[validpatterntile].gameObject, new Vector3(0.8f, 0.8f, 0.8f), 0.5f).setEasePunch();
        //             //Do 360 spin tween
        //             LeanTween.rotateAround(_tileObjects[validpatterntile].gameObject, Vector3.up, 360f, 0.5f).setEaseInOutSine();
        //
        //         }
        //     }
        // }
    }

    public void CheckForPattern(PatternDefinitionScriptableObject pattern)
    {
        //check each tile in the pattern against the grid


        //if pattern is found debug log pattern found
        Debug.Log("Pattern Found");
    }
    
    public void SetSelectedTilePosition(Vector2 position)
    {
        _selectedTilePosition = position;
    }
}
