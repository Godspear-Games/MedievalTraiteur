using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PatternDefinition", menuName = "Medieval Traiteur/CustomPattern", order = 1)]

public class PatternDefinition : ScriptableObject
{
    public string patternName;
    public List<Vector2> tilePositions; // List of relative positions for tiles in the pattern
    public int points; // Points to award for completing this pattern
}
