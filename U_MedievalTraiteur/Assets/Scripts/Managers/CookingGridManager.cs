using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Numerics;
using UnityEngine.UI;
using Random = System.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CookingGridManager : MonoBehaviour
{
    private Vector2 _defaultGridSize;
    private List<Vector2> _unlockedUpgradeSlots = new List<Vector2>();
    private Dictionary<Vector2,bool> _finalGridSlots;
    private Vector2 _actualGridSize;

    [SerializeField] private MinigameGridTile _tilePrefab;
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;
    
    private Dictionary<Vector2, TileScriptableObject> _tileData;
    private Dictionary<Vector2, MinigameGridTile> _tileObjects;

    private Vector2 _selectedTileKey;

    public static CookingGridManager Instance;

    private void Awake()
    {
        PlayerPrefs.DeleteAll();
        _selectedTileKey = new Vector2(-1, -1);
        Instance = this;
    }



    public void SetupCookingGrid(Vector2 defaultgridsize, List<Vector2> unlockedupgradeslots)
    {
        _defaultGridSize = defaultgridsize;
        _unlockedUpgradeSlots = unlockedupgradeslots;
        
        GenerateFinalGridSlotsDictionary();
        SetGridLayoutGroupCellSize();
        GenerateGrid();
    }

    #region Setup Grid

    private void GenerateFinalGridSlotsDictionary()
    {
        List<Vector2> availablefinalgridslots = new List<Vector2>();
        
        availablefinalgridslots.Clear();
        
        //add default rows and columns to _finalGridSlots
        for (int y = 0; y < _defaultGridSize.y; y++)
        {
            for (int x = 0; x < _defaultGridSize.x; x++)
            {
                availablefinalgridslots.Add(new Vector2(x,y));
            }
        }
        
        //add unlocked upgrade slots to _finalGridSlots
        foreach (Vector2 unlockedupgradeslot in _unlockedUpgradeSlots)
        {
            availablefinalgridslots.Add(unlockedupgradeslot);
        }

        // Initialize variables to store maximum and minimum X and Y values
        int maxX = int.MinValue;
        int maxY = int.MinValue;
        int minX = int.MaxValue;
        int minY = int.MaxValue;
        
        // Find the maximum and minimum X and Y values
        foreach (Vector2 key in availablefinalgridslots)
        {
            maxX = Mathf.Max(maxX, (int)key.x);
            maxY = Mathf.Max(maxY, (int)key.y);
            minX = Mathf.Min(minX, (int)key.x);
            minY = Mathf.Min(minY, (int)key.y);
        }
        
        Debug.Log("Max X: " + maxX + " Max Y: " + maxY + " Min X: " + minX + " Min Y: " + minY);

        // Create a new dictionary<vector2,bool> to store the available final grid slots as well as empty slots
        Dictionary<Vector2, bool> finalgridslots = new Dictionary<Vector2, bool>();
        
        // Initialize the availableFinalGridSlots dictionary
        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                finalgridslots.Add(new Vector2(x,y), false);
            }
        }

        foreach(Vector2 slot in availablefinalgridslots)
        {
            finalgridslots[slot] = true;
        }
        
        _finalGridSlots = finalgridslots;
        _actualGridSize = new Vector2(maxX - minX, maxY - minY);
    }
    
    private void GenerateGrid()
    {
        _tileData = new Dictionary<Vector2, TileScriptableObject>();
        _tileObjects = new Dictionary<Vector2, MinigameGridTile>();
        for (int y = _finalGridSlots.Keys.Min(k => (int)k.y); y <= _finalGridSlots.Keys.Max(k => (int)k.y); y++)
        {
            for (int x = _finalGridSlots.Keys.Min(k => (int)k.x); x <= _finalGridSlots.Keys.Max(k => (int)k.x); x++)
            {
                MinigameGridTile tile = Instantiate(_tilePrefab, _gridLayoutGroup.transform);
                tile.setAvailable(_finalGridSlots[new Vector2(x, y)]);
                tile.name = $"Tile {x} {y}" + " is available: " + _finalGridSlots[new Vector2(x, y)];

                _tileData[new Vector2(x, y)] = null;
                _tileObjects[new Vector2(x, y)] = tile;
            }
        }
    }
    private void SetGridLayoutGroupCellSize()
    {
        float cellSize = 10;
        float spacing;
        
        //calculate cell size and spacing based on size of Recttransform
        float width = _gridLayoutGroup.GetComponent<RectTransform>().rect.width;
        float height = _gridLayoutGroup.GetComponent<RectTransform>().rect.height;
        
        //set spacing to be 2% of the smallest dimension
        spacing = Mathf.Min(width, height) * 0.00f;
        
        float cellSizeX = (width-(2*_gridLayoutGroup.padding.left) - spacing) / (_actualGridSize.x + 1) - spacing;
        float cellSizeY = (height-(2*_gridLayoutGroup.padding.top) - spacing) / (_actualGridSize.y + 1) - spacing;

        //take the smallest cell size
        if (cellSizeX < cellSizeY)
        {
            cellSize = cellSizeX;
        }
        else
        {
            cellSize = cellSizeY;
        }
        
        Debug.Log("Cell size: " + cellSize);
        
        _gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
        _gridLayoutGroup.spacing = new Vector2(spacing, spacing);
        _gridLayoutGroup.constraintCount = (int)_actualGridSize.x+1;
        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;

    }

    #endregion"

    #region TileDataManipulation
    
    public void SetSelectedTile(MinigameGridTile tile)
    {
        if (tile == null)
        {
            _selectedTileKey = new Vector2(-1, -1);
            return;
        }
        
        //find key of this tile in _tileObjects
        _selectedTileKey = _tileObjects.FirstOrDefault(x => x.Value == tile).Key;
    }
    
    public bool TryTileUpdate(TileScriptableObject tileScriptableObject)
    {
        if (_selectedTileKey == new Vector2(-1, -1))
        {
            Debug.LogWarning("No tile selected");
            return false;
        }
        if (_tileData[_selectedTileKey] != null)
        {
            Debug.LogWarning("Tile already filled");
            return false;
        }
        
        SetTileType(_selectedTileKey, tileScriptableObject);
        return true;
    }

    public void SetTileType(Vector2 key, TileScriptableObject tileScriptableObject)
    {
        if (key == new Vector2(-1, -1))
        {
            return;
        }

        if (_tileObjects.TryGetValue(key, out MinigameGridTile tile))
        {
            _tileObjects[key].SetTileType(tileScriptableObject);
            _tileData[key] = tileScriptableObject;
        }

        if (tileScriptableObject == null)
        {
            return;
        }
        
        //todo check grid for patterns

        List<ValidFoundPattern> validPatterns = new List<ValidFoundPattern>();
        
        //Check for patterns
        foreach (PatternDefinitionScriptableObject pattern in PatternManager.Instance.GetAllPatterns())
        {
            foreach (ValidFoundPattern validfoundpattern in CheckForPatterns(pattern))
            {
                validPatterns.Add(validfoundpattern);
            }
        }
        
        //eliminate exact duplicates from validPatterns
        
        validPatterns = FilterDuplicatePatterns(validPatterns);

        if (validPatterns.Count <= 0)
        {
            //check if field is full and do gameover
            if (_tileData.Values.All(x => x != null))
            {
                Debug.Log("Game over");
            }
            return;
        }

        //clear all tiles in the patterns

        List<MinigameGridTile> tilesThatHaveBeenReplacedByOutputTiles = new List<MinigameGridTile>();
        
        //add output tile to player's hand

        for (var index = 0; index < validPatterns.Count; index++)
        {
            ValidFoundPattern validPattern = validPatterns[index];
            
            //todo add output tile to completed dishes list
            Debug.Log("output added to dish bench: " + validPattern.Pattern.OutputDish.name);
            
            DishBenchManager.Instance.AddDish(validPattern.Pattern.OutputDish);
            
            //if last iteration set done adding tiles
        }

        //do a punch scale tween on each tile in the validpatterns
        foreach (ValidFoundPattern validPattern in validPatterns)
        {
            foreach (Vector2 validpatternkey in validPattern.ValidPatternTiles)
            {
                if (_tileObjects.TryGetValue(validpatternkey, out MinigameGridTile tweentile))
                {
                    // Debug.Log("Tweening tile" + tweentile.name);
                    LeanTween.cancel(tweentile.gameObject);
                    tweentile.gameObject.transform.localScale = new Vector3(1, 1, 1);
                    
                    LeanTween.rotate(tweentile.GetComponent<RectTransform>(), 360f, 0.4f).setEaseInOutSine().setOnComplete(
                        () =>
                        {
                            SetTileType(validpatternkey,null);
                        });

                    //do 360 degree rotation for ui object
                    // LeanTween.rotateAroundLocal(tweentile.gameObject, Vector3.forward, 360, 0.4f);
                }
            }
        }
    }

    #endregion
    
    #region Pattern Matching
   List<ValidFoundPattern> CheckForPatterns(PatternDefinitionScriptableObject pattern)
{
    int gridRows = _tileObjects.Keys.Max(k => (int)k.y) + 1;
    int gridCols = _tileObjects.Keys.Max(k => (int)k.x) + 1;
    
    //debug grid sizes
    Debug.Log("Grid rows: " + gridRows + " Grid cols: " + gridCols);
    
    List<ValidFoundPattern> foundPatterns = new List<ValidFoundPattern>();

    for (int rotation = 0; rotation < 4; rotation++) // 0, 90, 180, 270 degrees
    {
        for (int i = -(gridRows - 1); i < gridRows; i++)
        {
            for (int j = -(gridCols - 1); j < gridCols; j++)
            {
                List<Vector2> validPatternTiles = CheckPatternAtPosition(new Vector2(j, i), pattern.InputTiles, gridRows, gridCols, rotation);
                if (validPatternTiles != null)
                {
                    Debug.Log("Pattern found!" + pattern.name);
                    ValidFoundPattern validpattern = new ValidFoundPattern();
                    validpattern.Pattern = pattern;
                    validpattern.ValidPatternTiles = validPatternTiles;
                    
                    foundPatterns.Add(validpattern);
                }
            }
        }
    }

    return foundPatterns;
}

    List<Vector2> CheckPatternAtPosition(Vector2 startPosition, TileScriptableObject[,] pattern, int gridRows, int gridCols, int rotation)
{
    int patternRows = pattern.GetLength(0);
    int patternCols = pattern.GetLength(1);
    
    //debug pattern sizes

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
                return null; // Pattern exceeds grid boundaries or position already visited
            }

            // If this tile is not null, add it to the list of valid pattern tiles
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
    
    public bool IsFieldFilled()
    {
        foreach (var tile in _tileData)
        {
            if (tile.Value == null)
            {
                return false;
            }
        }
        return true;
    }
    #endregion

    #region Pattern Match Comparing
    List<ValidFoundPattern> FilterDuplicatePatterns(List<ValidFoundPattern> patterns)
    {
        for (int i = patterns.Count-1; i >=0; i--)
        {
            bool isDuplicate = false;
            
            for (int j = patterns.Count-1; j >=0; j--)
            {
                if (i == j)
                {
                    continue;
                }
                HashSet<Vector2> set1 = new HashSet<Vector2>(patterns[i].ValidPatternTiles);
                HashSet<Vector2> set2 = new HashSet<Vector2>(patterns[j].ValidPatternTiles);
                if (set1.SetEquals(set2) == false)
                {
                    continue;
                }
                else
                {
                    isDuplicate = true;
                }
            }
            
            if (isDuplicate)
            {
                patterns.RemoveAt(i);
            }
        }
        
        return patterns;
        
    }
    

    #endregion
    public class ValidFoundPattern
    {
        public List<Vector2> ValidPatternTiles;
        public PatternDefinitionScriptableObject Pattern;
    }
}
