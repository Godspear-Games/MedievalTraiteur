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
    public string Name;
    public string Description;

    public int SoulValue;
    [AssetSelector(Paths = "Assets/Prefabs/TilePrefabs")]

    public Tile TilePrefab;
    public Sprite UISprite;

    [FormerlySerializedAs("_uiColor")] public Color UIColor;
}
