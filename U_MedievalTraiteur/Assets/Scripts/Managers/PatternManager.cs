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

    private List<PatternDefinitionScriptableObject> _hintPatterns = new List<PatternDefinitionScriptableObject>();

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadPatterns();
        //draw 3 hint patterns
        for (int i = 0; i < 3; i++)
        {
            DrawPatternHint();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //load all patterns from the resources folder
    private void LoadPatterns()
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
    public List<PatternDefinitionScriptableObject> GetPossibleQuestPatterns()
    {
        return _allPatterns.Except(_hintPatterns).ToList();
    }
    
    //add a pattern to the hint patterns
    private void DrawPatternHint()
    {
        PatternDefinitionScriptableObject newhintpattern = GetRandomPattern();
        if (_hintPatterns.Contains(newhintpattern))
        {
            DrawPatternHint();
        }
        else
        {
            _hintPatterns.Add(newhintpattern);
            //send event to update the UI
            EventManager.Instance.AddHintPattern(newhintpattern);
        }
    }
    
    private PatternDefinitionScriptableObject GetRandomPattern()
    {
        return _allPatterns[Random.Range(0, _allPatterns.Count)];
    }
}
