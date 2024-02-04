using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

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
        Instance = this;
        LoadPatterns();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetGridLayoutGroupCellSize();
        GenerateGrid();
        TileListManager.Instance.PresentNextTile();
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
        for (int x = 0; x < _widthTileAmount; x++)
        {
            for (int y = 0; y < _heightTileAmount; y++)
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

        //todo check grid for patterns
        
        //optimization to only check patterns that include the most recently placed tile
        // List<PatternDefinitionScriptableObject> possiblycreatedpatterns = new List<PatternDefinitionScriptableObject>();
        // foreach (PatternDefinitionScriptableObject pattern in _patternDefinitions)
        // {
        //     //check if pattern.inputtiles contains a tilescriptableobject that is the same as the tilescriptableobject that was just placed
        //     if (pattern.PatternContainsTile(tileScriptableObject))
        //     {
        //         possiblycreatedpatterns.Add(pattern);
        //         Debug.Log("Pattern " + pattern.name + " contains tile " + tileScriptableObject.name);
        //     }
        // }
        
        List<ValidFoundPattern> validPatterns = new List<ValidFoundPattern>();
        
        //Check for patterns
        foreach (PatternDefinitionScriptableObject pattern in _patternDefinitions)
        {
            List<Vector2> validPatternTiles = CheckForPatterns(pattern);
            if (validPatternTiles != null)
            {
                ValidFoundPattern validFoundPattern = new ValidFoundPattern();
                validFoundPattern.Pattern = pattern;
                validFoundPattern.ValidPatternTiles = validPatternTiles;
                
                validPatterns.Add(validFoundPattern);
            }
        }

        if (validPatterns.Count <= 0)
        {
            return;
        }
        
        //todo properly resolve overlap in patterns if multiple patterns are found
        
        //clear all tiles in the patterns
        foreach (ValidFoundPattern validPattern in validPatterns)
        {
            //clear all tiles in the pattern
            foreach (Vector2 validpatternkey in validPattern.ValidPatternTiles)
            {
                SetTileType(validpatternkey,null);
            }
        }
        
        List<MinigameGridTile> tilesThatHaveBeenReplacedByOutputTiles = new List<MinigameGridTile>();
        //replace one random tile in each validpattern with the output tile never replacing the same tile twice
        foreach (ValidFoundPattern validPattern in validPatterns)
        {
            MinigameGridTile tileToReplace = null;
            //replace one random tile with the output tile
            do
            {
                int randomTile = UnityEngine.Random.Range(0, validPattern.ValidPatternTiles.Count);
                _tileObjects.TryGetValue(validPattern.ValidPatternTiles[randomTile], out tileToReplace);
            } while (tileToReplace == null || tilesThatHaveBeenReplacedByOutputTiles.Contains(tileToReplace));
            
            //get ket of tile to replace
            Vector2 tileToReplaceKey = _tileObjects.FirstOrDefault(x => x.Value == tileToReplace).Key;
            SetTileType(tileToReplaceKey, validPattern.Pattern.OutputStructure);
            tilesThatHaveBeenReplacedByOutputTiles.Add(tileToReplace);
        }

        //do a punch scale tween on each tile in the validpatterns
        foreach (ValidFoundPattern validPattern in validPatterns)
        {
            foreach (Vector2 validpatternkey in validPattern.ValidPatternTiles)
            {
                if (_tileObjects.TryGetValue(validpatternkey, out MinigameGridTile tweentile))
                {
                    Debug.Log("Tweening tile" + tweentile.name);
                    
                    LeanTween.scale(tweentile.gameObject, new Vector3(0.8f, 0.8f, 0.8f), 0.4f).setEasePunch();
                    
                    //do 360 degree rotation for ui object
                    LeanTween.rotateAroundLocal(tweentile.gameObject, Vector3.forward, 360, 0.4f);
                }
            }
        }
    }

    #endregion
    
    #region Pattern Matching
    List<Vector2> CheckForPatterns(PatternDefinitionScriptableObject pattern)
    {
        
        int gridRows = _tileObjects.Keys.Max(k => (int)k.x) + 1;
        int gridCols = _tileObjects.Keys.Max(k => (int)k.y) + 1;

        for (int rotation = 0; rotation < 4; rotation++) // 0, 90, 180, 270 degrees
        {
            for (int i = 0; i < gridRows; i++)
            {
                for (int j = 0; j < gridCols; j++)
                {

                    List<Vector2> validPatternTiles = CheckPatternAtPosition(new Vector2(i, j), pattern.InputTiles, gridRows, gridCols, rotation);
                    if (validPatternTiles != null)
                    {
                        Debug.Log("Pattern found!" + pattern.name);
                        return validPatternTiles; // Pattern matched with the specified rotation
                    }
                }
            }
        }

        Debug.Log("Pattern not found: " + pattern.name);
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
    #endregion
    
    
    public class ValidFoundPattern
    {
        public List<Vector2> ValidPatternTiles;
        public PatternDefinitionScriptableObject Pattern;
    }
}
