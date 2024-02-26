using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "PatternDefinition", menuName = "Medieval Traiteur/CustomPattern", order = 1)]

public class PatternDefinitionScriptableObject : SerializedScriptableObject
{
    public bool IsActive = true;
    
    public string PatternName; 
    public string PatternDescription;
    
    [TableMatrix(HorizontalTitle = "Input Tiles", SquareCells = true, DrawElementMethod = ("DrawElement"))]
    public TileScriptableObject[,] InputTiles = new TileScriptableObject[4,4]; //2D array of tiles
    
    //public List<List<TileScriptableObject>> InputTiles = new List<List<TileScriptableObject>>(); //List of rows consisting of tiles

    [InlineEditor(InlineEditorObjectFieldModes.Foldout, Expanded = true)]
    public TileScriptableObject OutputStructure; //structure
    
    public bool PatternContainsTile(TileScriptableObject tile)
    {
        foreach (var inputTile in InputTiles)
        {
            if (inputTile == tile)
            {
                return true;
            }
        }
        return false;
    }

    private TileScriptableObject DrawElement(Rect rect, TileScriptableObject tileScriptableObject)
    {
#if UNITY_EDITOR

        EditorGUI.BeginChangeCheck();

        if (tileScriptableObject != null)
        {
            // Draw the UnityPreviewObjectField as an overlay
            var newTileScriptableObject = SirenixEditorFields.UnityPreviewObjectField(rect, GUIContent.none, tileScriptableObject,tileScriptableObject.UISprite.texture, typeof(TileScriptableObject), false) as TileScriptableObject;

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(this, "Assign TileScriptableObject");
                tileScriptableObject = newTileScriptableObject;
            }
        }
        else
        {
            // Draw a default UnityPreviewObjectField if tileScriptableObject is null
            var newTileScriptableObject = SirenixEditorFields.UnityPreviewObjectField(rect, GUIContent.none, null, typeof(TileScriptableObject), false) as TileScriptableObject;

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(this, "Assign TileScriptableObject");
                tileScriptableObject = newTileScriptableObject;
            }
        }

        return tileScriptableObject;
        
#else
    return tileScriptableObject;
#endif
    }
}
