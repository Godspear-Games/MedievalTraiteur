using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UIElements.Experimental;

public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler
{
    [SerializeField] private RectTransform _button = null;
    [SerializeField] private LeanTweenType _easeTypeOnHover = LeanTweenType.easeOutSine;
    [SerializeField] private LeanTweenType _easeTypeOnClick = LeanTweenType.easeOutSine;
    [SerializeField] private LeanTweenType _easeTypeOnExit = LeanTweenType.easeOutSine;

    //event to bind functions to in inspector
    [SerializeField] private UnityEvent _onClick = null;

    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.cancel(_button.gameObject);
        LeanTween.scale(_button.gameObject, Vector3.one*1.1f, 0.5f).setEase(_easeTypeOnHover);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(_button.gameObject);
        LeanTween.scale(_button.gameObject, Vector3.one, 0.5f).setEase(_easeTypeOnExit);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        LeanTween.cancel(_button.gameObject);
        LeanTween.scale(_button.gameObject, Vector3.one*0.9f, 0.5f).setEase(_easeTypeOnClick);
        _onClick.Invoke();
    }
}
