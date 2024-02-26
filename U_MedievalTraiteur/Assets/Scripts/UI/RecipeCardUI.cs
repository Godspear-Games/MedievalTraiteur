using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeCardUI : MonoBehaviour
{
    [SerializeField] private Image _tileImage;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private RecipeDisplayUI _recipeDisplayUI;

    public void SetupRecipeCard(PatternDefinitionScriptableObject patternDefinitionScriptableObject)
    {
        _recipeDisplayUI.ShowRecipe(patternDefinitionScriptableObject);
        _tileImage.sprite = patternDefinitionScriptableObject.OutputStructure.UISprite;
        _name.text = patternDefinitionScriptableObject.OutputStructure.Name;
    }
    
}
