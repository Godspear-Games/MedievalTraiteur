using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridQuest : MonoBehaviour
{
    private TileScriptableObject _questTileType;
    [SerializeField] private Image _questTileImage;
    [SerializeField] private Transform _questTileModel;
    [SerializeField] private RecipeDisplayUI _recipeDisplayUI;
    
    public void SetQuestTile(PatternDefinitionScriptableObject patternDefinitionScriptableObject)
    {
        _questTileType = patternDefinitionScriptableObject.OutputStructure;
        _questTileImage.sprite = patternDefinitionScriptableObject.OutputStructure.UISprite;
        _questTileModel.localScale = Vector3.zero;
        _recipeDisplayUI.ShowRecipe(patternDefinitionScriptableObject);
        LeanTween.scale(_questTileModel.gameObject, Vector3.one, 0.25f).setEaseOutBack();
    }

    public TileScriptableObject GetQuestTileType()
    {
        return _questTileType;
    }
    
    public void DestroyThisQuestTile()
    {
        LeanTween.cancel(_questTileModel.gameObject);
        LeanTween.scale(_questTileModel.gameObject, Vector3.zero, 0.25f).setEaseInBack().setOnComplete(() => Destroy(gameObject));
    }
}
