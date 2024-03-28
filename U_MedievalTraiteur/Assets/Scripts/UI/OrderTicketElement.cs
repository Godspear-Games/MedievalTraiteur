using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class OrderTicketElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private OrderManager.Order _order = null;
    [SerializeField] private RectTransform _recipePanel = null;
    [SerializeField] private RecipeDisplayUI _recipeDisplayUI = null;
    [SerializeField] private Image _orderImage = null;
    
    public void SetOrder(OrderManager.Order order)
    {
        _order = order;
        _orderImage.sprite = order.OrderPattern.OutputDish.UISprite;
        _recipeDisplayUI.ShowRecipe(order.OrderPattern);
        
        _recipePanel.gameObject.transform.localScale = new Vector3(1, 0, 1);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        //show order
        LeanTween.cancel(_recipePanel.gameObject);
        LeanTween.scaleY(_recipePanel.gameObject, 1, 0.5f).setEaseOutSine();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(_recipePanel.gameObject);
        LeanTween.scaleY(_recipePanel.gameObject, 0, 0.5f).setEaseOutSine();
    }
}
