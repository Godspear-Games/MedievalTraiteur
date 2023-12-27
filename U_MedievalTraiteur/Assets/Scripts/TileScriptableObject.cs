using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Tile", menuName = "Medieval Traiteur/Tile", order = 0)]
public class TileScriptableObject : ScriptableObject
{
    public int Cost = 0;
    [AssetSelector(Paths = "Assets/Prefabs/TilePrefabs")]
    public Tile TilePrefab;
    public Sprite _uiSprite;
    [FormerlySerializedAs("_uiColor")] public Color UIColor;
}
