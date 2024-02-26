using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Tile", menuName = "Medieval Traiteur/Tile", order = 0)]
[Serializable]
public class TileScriptableObject : ScriptableObject
{
    public bool IsStructure;
    public string Name;
    public string Description;
    
    [PreviewField(120, ObjectFieldAlignment.Left)]
    public Sprite UISprite;
    
    public int SoulValue;
    public int QuestBonusValue;
    [AssetSelector(Paths = "Assets/Prefabs/TilePrefabs")]

    public GameObject TilePrefab;
    [FormerlySerializedAs("_uiColor")] public Color UIColor;

    public List<TileScriptableObject> QuestsThatCanBeSpawned;
}
