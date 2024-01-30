using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PatternDefinition", menuName = "Medieval Traiteur/CustomPattern", order = 1)]

public class PatternDefinition : SerializedScriptableObject
{
    public string PatternName; 
    public string PatternDescription;
    
    [TableMatrix(HorizontalTitle = "Input Tiles", SquareCells = true)]
    public TileScriptableObject[,] InputTiles = new TileScriptableObject[4,4]; //2D array of tiles
    
    //public List<List<TileScriptableObject>> InputTiles = new List<List<TileScriptableObject>>(); //List of rows consisting of tiles

    public TileScriptableObject OutputStructure; //structure
}
