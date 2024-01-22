using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PatternDefinition", menuName = "Medieval Traiteur/CustomPattern", order = 1)]

public class PatternDefinition : ScriptableObject
{
    public string patternName; 
    public string patternDescription;

    public List<List<TileScriptableObject>> inputTiles = new List<List<TileScriptableObject>>(); //List of rows consisting of tiles

    public Tile outputStructure; //structure
}
