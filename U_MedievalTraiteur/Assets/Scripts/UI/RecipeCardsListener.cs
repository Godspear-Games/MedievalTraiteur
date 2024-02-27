using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class RecipeCardsListener : MonoBehaviour
{
    [SerializeField] private GameObject _recipeCardPrefab = null;
    private Dictionary<GameObject, PatternDefinitionScriptableObject> _recipeCards = new Dictionary<GameObject, PatternDefinitionScriptableObject>();

    private void Start()
    {
        EventManager.Instance.OnAddHintPattern += AddHintPattern;
        //LowerCards(); //temporarily disabled this feature for testing purposes
    }

    private void OnDisable()
    {
        EventManager.Instance.OnAddHintPattern -= AddHintPattern;
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnAddHintPattern -= AddHintPattern;
    }

    private void AddHintPattern(PatternDefinitionScriptableObject patternDefinitionScriptableObject)
    {
        Debug.Log("Adding hint pattern");
        GameObject recipeCard = Instantiate(_recipeCardPrefab, transform);
        recipeCard.GetComponent<RecipeCardUI>().SetupRecipeCard(patternDefinitionScriptableObject);
        _recipeCards.Add(recipeCard, patternDefinitionScriptableObject);
        
        RefreshRecipeCardTransforms();
    }

    private void RemoveHintPattern(PatternDefinitionScriptableObject patternDefinitionScriptableObject)
    {
        foreach (var recipeCard in _recipeCards)
        {
            if (recipeCard.Value == patternDefinitionScriptableObject)
            {
                _recipeCards.Remove(recipeCard.Key);
                Destroy(recipeCard.Key);
                break;
            }
        }
        
        RefreshRecipeCardTransforms();
    }
    
    private void RefreshRecipeCardTransforms()
    {
        //update transforms to be 1/3 of parent width using anchors
        for (int i = 0; i < _recipeCards.Count; i++)
        {
            _recipeCards.Keys.ElementAt(i).GetComponent<RectTransform>().anchorMin = new Vector2(i / 3f, 0);
            _recipeCards.Keys.ElementAt(i).GetComponent<RectTransform>().anchorMax = new Vector2(i / 3f+1/3f, 1);
        }
    }
    
    //temporarily disbled this feature for testing purposes

    /*
    private void LowerCards()
    {
        LeanTween.cancel(gameObject);
        LeanTween.moveY(gameObject,-GetComponent<RectTransform>().rect.height*0.85f, 0.5f).setEaseOutCubic();
    }

    private void RaiseCards()
    {
        LeanTween.cancel(gameObject);
        LeanTween.moveY(gameObject,0, 0.5f).setEaseOutCubic();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        RaiseCards();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LowerCards();
    } */
}
