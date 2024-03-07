using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System.Linq;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    
    //grid data
    private Dictionary<Vector2, Tile> _grid = new Dictionary<Vector2, Tile>();
    [SerializeField] private Tile _tilePrefab;
    private Tile _selectedTile;
    [SerializeField] private AmountPopUp _amountPopUpPrefab;
    
    //quest data
    private Dictionary<Vector2,GridQuest> _questDictionary = new Dictionary<Vector2, GridQuest>();
    [SerializeField] private GridQuest _questPrefab;
    [SerializeField] private float _questSpawnChance = 0.1f;
    [SerializeField] private int _maxQuests = 3;
    [SerializeField] private int _questSpawnDistance = 3;
    [SerializeField] private bool _canDoRandomQuests = true;
    [SerializeField] private bool _canDoSpecificQuests = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetupGrid();
    }

    
    #region normal grid methods
    private void SetupGrid()
    {
        //instantiate first center tile at (0,0)
        SpawnTileSlot(Vector2.zero);
    }
    
    //method to calculate the positions of open tiles around all the already filled tiles
    private List<Vector2> GetTilePositionsAroundExistingCluster(List<Vector2> filledTilePositions)
    {
        List<Vector2> openTilePositions = new List<Vector2>();
        foreach (Vector2 filledTilePosition in filledTilePositions)
        {
            if (GetTileAtPosition(filledTilePosition).GetTileScriptableObject() != null)
            {
                List<Vector2> surroundingPositions = GetSurroundingPositions(filledTilePosition);
                foreach (Vector2 surroundingPosition in surroundingPositions)
                {
                    if (!_grid.ContainsKey(surroundingPosition) && !openTilePositions.Contains(surroundingPosition))
                    {
                        openTilePositions.Add(surroundingPosition);
                    }
                }
            }
        }
        return openTilePositions;
    }
    
    //method to refresh the grid by spawning new tile slots at all the new open positions
    private void RefreshGrid()
    {
        List<Vector2> newOpenTilePositions = GetTilePositionsAroundExistingCluster(_grid.Keys.ToList());
        foreach (Vector2 newOpenTilePosition in newOpenTilePositions)
        {
            SpawnTileSlot(newOpenTilePosition);
        }
    }

    
    //method to get the surrounding positions of a given position
    private List<Vector2> GetSurroundingPositions(Vector2 position)
    {
        List<Vector2> surroundingPositions = new List<Vector2>();
        surroundingPositions.Add(new Vector2(position.x + 1, position.y));
        surroundingPositions.Add(new Vector2(position.x - 1, position.y));
        surroundingPositions.Add(new Vector2(position.x, position.y + 1));
        surroundingPositions.Add(new Vector2(position.x, position.y - 1));
        return surroundingPositions;
    }
    
    //method to spawn a tile slot at a given position and add it to the grid data. This slot can then be filled with a tile
    public void SpawnTileSlot(Vector2 position)
    {
        Tile newTile = Instantiate(_tilePrefab, new Vector3(position.x, 0, position.y), Quaternion.identity);
        newTile.SetupTile();
        _grid.Add(position, newTile);
    }
    
    //method to fill a tile slot at a given position with a tile. This method will be called when a uitile is dragged and dropped onto a tile slot
    public void FillTileAtPosition(Vector2 position, TileScriptableObject tileScriptableObject)
    {
        GetTileAtPosition(position).FillTile(tileScriptableObject);
        GetTileAtPosition(position).SetupTile();
        RefreshGrid();
    }
    
    public void FillTile(Tile tile, TileScriptableObject tileScriptableObject)
    {
        Vector2 tileposition = _grid.FirstOrDefault(x => x.Value == tile).Key;
        
        CheckIfFilledTileCompletesQuestTile(tileposition, tileScriptableObject);
        tile.FillTile(tileScriptableObject);
        
        StartCoroutine(SpawnSoulPopUpCoroutine(tileposition, tileScriptableObject.SoulValue,0));

        tile.SetupTile();
        if (CheckIfQuestShouldSpawn())
        {
            DoQuestGeneration(tile);
        }
        RefreshGrid();
    }
    
    public Tile GetTileAtPosition(Vector2 position)
    {
        return _grid[position];
    }
    
    //Set selected tile
    public void SetSelectedTile(Tile tile)
    {
        _selectedTile = tile;
    }
    
    //set selected tile to null
    public void DeselectTile(Tile tile)
    {
        if (_selectedTile == tile)
        {
            _selectedTile = null;
        }
    }
    
    #endregion
    
    #region quest methods
    //Check if a quest should be spawned
    public bool CheckIfQuestShouldSpawn()
    {
        if (UnityEngine.Random.Range(0f,1f)<_questSpawnChance && _questDictionary.Count < _maxQuests)
        {
            return true;
        }
        return false;
    }
    
    public void DoQuestGeneration(Tile tile)
    {
        //Determined Quest Spawn
        if (_canDoSpecificQuests && Random.Range(0f,1f)<0.5f && tile.GetTileScriptableObject().QuestsThatCanBeSpawned.Count > 0)
        {
            List<Vector2> possibleQuestPositions = GetSurroundingPositions(_grid.FirstOrDefault(x => x.Value == tile).Key);
            //remove existing quest positions
            foreach (KeyValuePair<Vector2, GridQuest> quest in _questDictionary)
            {
                possibleQuestPositions.Remove(quest.Key);
            }
            //remove filled tile positions
            foreach (KeyValuePair<Vector2, Tile> filledTile in _grid)
            {
                possibleQuestPositions.Remove(filledTile.Key);
            }

            if (possibleQuestPositions.Count > 0)
            {
                Vector2 randomPosition = possibleQuestPositions[UnityEngine.Random.Range(0, possibleQuestPositions.Count)];
                //get all patterns from Tiles/T1_Buildings folder and spawn a quest with a random tile from that folder
                TileScriptableObject randomTile = tile.GetTileScriptableObject().QuestsThatCanBeSpawned[UnityEngine.Random.Range(0, tile.GetTileScriptableObject().QuestsThatCanBeSpawned.Count)];
                
                //get the pattern that has this tile as outputstructure
                PatternDefinitionScriptableObject randomPattern = PatternManager.Instance.GetAllPatterns().FirstOrDefault(x => x.OutputStructure == randomTile);
                SpawnQuest(randomPosition, randomPattern);
            }
            
        }
        //Random Quest Spawn
        else if (_canDoRandomQuests)
        {
            List<Vector2> possibleQuestPositions = GetTilePositionsXAwayFromExistingCluster(_questSpawnDistance);
            //remove existing quest positions
            foreach (KeyValuePair<Vector2, GridQuest> quest in _questDictionary)
            {
                possibleQuestPositions.Remove(quest.Key);
            }
        
            if (possibleQuestPositions.Count > 0)
            {
                Vector2 randomPosition = possibleQuestPositions[UnityEngine.Random.Range(0, possibleQuestPositions.Count)];
                //get random tile from PatternManager.Instance.GetPossibleQuestPatterns() and spawn a quest with that tile
                PatternDefinitionScriptableObject randomPattern = PatternManager.Instance.GetPossibleQuestPatterns()[Random.Range(0, PatternManager.Instance.GetPossibleQuestPatterns().Count)];
                SpawnQuest(randomPosition, randomPattern);
            }
        }
    }
    
    
    //spawn a quest at a given position and add it to the quest dictionary
    public void SpawnQuest(Vector2 position, PatternDefinitionScriptableObject pattern)
    {
        GridQuest newQuest = Instantiate(_questPrefab, new Vector3(position.x, 0, position.y), Quaternion.identity);
        newQuest.SetQuestTile(pattern);
        _questDictionary.Add(position, newQuest);
    }


    //Get tiles locations that are 2 tiles away from any filled tile
    public List<Vector2> GetTilePositionsXAwayFromExistingCluster(int amountoftiles)
    {
        List<Vector2> allinternalpositions = new List<Vector2>();
        allinternalpositions.AddRange(_grid.Keys.ToList());
        for (int i = 0; i < amountoftiles-1; i++)
        {
            allinternalpositions.AddRange(GetPositionsAroundThesePositions(allinternalpositions));
        }

        List<Vector2> allpositionsXtilesaway = GetPositionsAroundThesePositions(allinternalpositions);
        //remove allinternalpositions from allpositionsXtilesaway
        foreach (Vector2 position in allinternalpositions)
        {
            allpositionsXtilesaway.Remove(position);
        }
        
        
        return allpositionsXtilesaway;
    }

    private List<Vector2> GetPositionsAroundThesePositions(List<Vector2> positions)
    {
        List<Vector2> allpositions = new List<Vector2>();
        foreach (Vector2 position in positions)
        {
            allpositions.AddRange(GetSurroundingPositions(position));
        }
        //remove duplicates
        allpositions = allpositions.Distinct().ToList();
        
        return allpositions;
    }

    //method to remove a quest from the grid
    
    public void RemoveQuest(Vector2 position)
    {
        _questDictionary[position].DestroyThisQuestTile();
        _questDictionary.Remove(position);
    }
    
    private void CheckIfFilledTileCompletesQuestTile(Vector2 position, TileScriptableObject tileScriptableObject)
    {
        if (_questDictionary.ContainsKey(position))
        {
            if (tileScriptableObject != null)
            {
                if (tileScriptableObject == _questDictionary[position].GetQuestTileType())
                {
                    Debug.Log("Quest Completed");
                    StartCoroutine(SpawnSoulPopUpCoroutine(position,1, 0.5f));
                    ScoreManager.Instance.AddToScore(tileScriptableObject.QuestBonusValue);
                    RemoveQuest(position);
                }
                else
                {
                    Debug.Log("Quest Failed");
                    //todo: add quest failed logic
                    RemoveQuest(position);
                }
            }

        }
    }

    #endregion
    
    #region SoulPopUpMethods

    //SpawnSoulPopUp Coroutine
    IEnumerator SpawnSoulPopUpCoroutine(Vector2 position, int amount, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        AmountPopUp newSoulPopUp = Instantiate(_amountPopUpPrefab, new Vector3(position.x, 0, position.y), Quaternion.identity);
        newSoulPopUp.SetAmount(amount);
        yield return null;
    }

    #endregion
    
}
