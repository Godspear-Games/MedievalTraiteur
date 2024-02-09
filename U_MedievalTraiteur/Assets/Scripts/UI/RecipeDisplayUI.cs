using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeDisplayUI : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup _recipegrid;
    
    public void ShowRecipe(PatternDefinitionScriptableObject patternDefinitionScriptableObject)
    {
        //remove empty rows and columns
        List<List<TileScriptableObject>> inputTiles = RemoveEmptyRowsAndColumns(patternDefinitionScriptableObject);
        
        //set tile size and spacing relative to panel size
        float panelWidth = _recipegrid.GetComponent<RectTransform>().rect.width;
        float panelHeight = _recipegrid.GetComponent<RectTransform>().rect.height;
        
        //calculate tile size and spacing, take smallest grid dimension
        float tileSize = panelWidth / inputTiles[0].Count;
        if (tileSize * inputTiles.Count > panelHeight)
        {
            tileSize = panelHeight / inputTiles.Count;
        }

        //set spacing to 5%
        _recipegrid.spacing = new Vector2(tileSize * 0.05f, tileSize * 0.05f);
        
        //update tilesize to account for spacing
        tileSize = tileSize - _recipegrid.spacing.x;
        
        //set cell size to tile size
        _recipegrid.cellSize = new Vector2(tileSize, tileSize);

        //set constraint type to smallest dimension of inputtiles
        if (inputTiles.Count >= inputTiles[0].Count)
        {
            _recipegrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            // set constraint count to inputtiles count
            _recipegrid.constraintCount = inputTiles.Count;
        }
        else
        {
            _recipegrid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            // set constraint count to inputtiles[0] count
            _recipegrid.constraintCount = inputTiles[0].Count;
        }
        

        //clear grid
        foreach (Transform child in _recipegrid.transform)
        {
            Destroy(child.gameObject);
        }

        //instantiate tile images in grid, if tile is null instantiate clear image
        for (int i = 0; i < inputTiles.Count; i++)
        {
            for (int j = 0; j < inputTiles[i].Count; j++)
            {
                GameObject tile = new GameObject();
                tile.AddComponent<Image>();
                tile.transform.SetParent(_recipegrid.transform);
                //set scale to 1
                tile.transform.localScale = new Vector3(1, 1, 1);

                if (inputTiles[i][j] != null)
                {
                    tile.GetComponent<Image>().sprite = inputTiles[i][j].UISprite;
                }
                else
                {
                    tile.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                }
                
                //tween punch tile
                LeanTween.scale(tile, new Vector3(1.1f, 1.1f, 1.1f), 0.5f).setEasePunch();
                
            }
        }

    }

    private List<List<TileScriptableObject>> RemoveEmptyRowsAndColumns(PatternDefinitionScriptableObject patternDefinitionScriptableObject)
    {
        //make a copy of the 2D array patterndefinitionscriptableobject.inputtiles as a list
        List<List<TileScriptableObject>> inputTiles = new List<List<TileScriptableObject>>();
        for (int i = 0; i < patternDefinitionScriptableObject.InputTiles.GetLength(0); i++)
        {
            List<TileScriptableObject> row = new List<TileScriptableObject>();
            for (int j = 0; j < patternDefinitionScriptableObject.InputTiles.GetLength(1); j++)
            {
                row.Add(patternDefinitionScriptableObject.InputTiles[i, j]);
            }
            inputTiles.Add(row);
        }
        
        //remove empty rows
        for (int i = inputTiles.Count - 1; i >= 0; i--)
        {
            bool empty = true;
            for (int j = 0; j < inputTiles[i].Count; j++)
            {
                if (inputTiles[i][j] != null)
                {
                    empty = false;
                    break;
                }
            }
            if (empty)
            {
                inputTiles.RemoveAt(i);
            }
        }
        
        //remove empty columns
        for (int j = inputTiles[0].Count - 1; j >= 0; j--)
        {
            bool empty = true;
            for (int i = 0; i < inputTiles.Count; i++)
            {
                if (inputTiles[i][j] != null)
                {
                    empty = false;
                    break;
                }
            }
            if (empty)
            {
                for (int i = 0; i < inputTiles.Count; i++)
                {
                    inputTiles[i].RemoveAt(j);
                }
            }
        }

        return inputTiles;
    }
}
