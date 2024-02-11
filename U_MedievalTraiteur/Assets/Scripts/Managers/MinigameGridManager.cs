using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Random = System.Random;

public class MinigameGridManager : MonoBehaviour
{
    [SerializeField] private int _widthTileAmount;
    [SerializeField] private int _heightTileAmount;
    [SerializeField] private MinigameGridTile _tilePrefab;
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;
    
    private Dictionary<Vector2, TileScriptableObject> _tileData;
    private Dictionary<Vector2, MinigameGridTile> _tileObjects;

    private Vector2 _selectedTileKey;
    
    private List<PatternDefinitionScriptableObject> _patternDefinitions;
    
    public static MinigameGridManager Instance;

    private void Awake()
    {
        PlayerPrefs.DeleteAll();
        _selectedTileKey = new Vector2(-1, -1);
        Instance = this;
        LoadPatterns();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetGridLayoutGroupCellSize();
        GenerateGrid();
        
        //todo commented to rework the logic, should this really be here?
        //TileListManager.Instance.TryToPresentNextTile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region LoadResources

    private void LoadPatterns()
    {
        _patternDefinitions = Resources.LoadAll<PatternDefinitionScriptableObject>("Patterns").ToList();
    }

    #endregion

    #region Setup Grid

    private void GenerateGrid()
    {
        _tileData = new Dictionary<Vector2, TileScriptableObject>();
        _tileObjects = new Dictionary<Vector2, MinigameGridTile>();
        for (int y = 0; y < _heightTileAmount; y++)
        {
            for (int x = 0; x < _widthTileAmount; x++)
            {
                MinigameGridTile tile = Instantiate(_tilePrefab, _gridLayoutGroup.transform);
                tile.name = $"Tile {x} {y}";

                _tileData[new Vector2(x, y)] = null;
                _tileObjects[new Vector2(x, y)] = tile;
            }
        }
        
        // Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + (_width / 2f),Camera.main.transform.position.y+ 0, Camera.main.transform.position.z +(_height / 2f));
    }
    private void SetGridLayoutGroupCellSize()
    {
        float cellSize = 100;
        float spacing = 10;
        
        //calculate cell size and spacing based on size of Recttransform
        float width = _gridLayoutGroup.GetComponent<RectTransform>().rect.width;
        float height = _gridLayoutGroup.GetComponent<RectTransform>().rect.height;
        
        //set spacing to be 2% of the smallest dimension
        spacing = Mathf.Min(width, height) * 0.02f;

        float cellSizeX = (width - (spacing * (_widthTileAmount - 1))) / _widthTileAmount;
        float cellSizeY = (height - (spacing * (_heightTileAmount - 1))) / _heightTileAmount;
        
        //take the smallest cell size
        if (cellSizeX < cellSizeY)
        {
            cellSize = cellSizeX;
        }
        else
        {
            cellSize = cellSizeY;
        }
        
        _gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
        _gridLayoutGroup.spacing = new Vector2(spacing, spacing);

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
        foreach (PatternDefinitionScriptableObject pattern in _patternDefinitions)
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
            TileListManager.Instance.DoneAddingTiles();
            return;
        }
        
        //todo properly resolve overlap in patterns if multiple patterns are found
        
        //clear all tiles in the patterns

        List<MinigameGridTile> tilesThatHaveBeenReplacedByOutputTiles = new List<MinigameGridTile>();
        
        //add output tile to player's hand

        for (var index = 0; index < validPatterns.Count; index++)
        {
            ValidFoundPattern validPattern = validPatterns[index];
            Debug.Log("output added to hand: " + validPattern.Pattern.OutputStructure.name);
            TileListManager.Instance.AddNewTileToHand(validPattern.Pattern.OutputStructure);
            EventManager.Instance.ShowPopup(validPattern.Pattern);
            //if last iteration set done adding tiles
            if (index == validPatterns.Count - 1)
            {
                TileListManager.Instance.DoneAddingTiles();
            }
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
                    
                    LeanTween.scale(tweentile.gameObject, new Vector3(0.8f, 0.8f, 0.8f), 0.4f).setEasePunch().setOnComplete(
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
