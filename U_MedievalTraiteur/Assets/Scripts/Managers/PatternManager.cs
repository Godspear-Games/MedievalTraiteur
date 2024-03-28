using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PatternManager : MonoBehaviour
{
    public static PatternManager Instance = null;
    
    [SerializeField] private string _patternFolder = "Patterns";
    private List<PatternDefinitionScriptableObject> _allPatterns = new List<PatternDefinitionScriptableObject>();
    

    private void Awake()
    {
        Instance = this;
        LoadPatterns();
    }

    //load all patterns from the resources folder
    public void LoadPatterns()
    {
        //load all patterns that have _isActive set to true
        _allPatterns = Resources.LoadAll<PatternDefinitionScriptableObject>(_patternFolder).Where(x => x.IsActive).ToList();
    }
    
    //return all patterns
    public List<PatternDefinitionScriptableObject> GetAllPatterns()
    {
        return _allPatterns;
    }

    //return all patterns that aren't hint patterns
    public List<PatternDefinitionScriptableObject> GetPossibleOrderPatterns()
    {
        return _allPatterns.ToList();
    }

    private PatternDefinitionScriptableObject GetRandomPattern()
    {
        Debug.Log("GetRandomPattern() called");
        return _allPatterns[Random.Range(0, _allPatterns.Count)];
    }
}
