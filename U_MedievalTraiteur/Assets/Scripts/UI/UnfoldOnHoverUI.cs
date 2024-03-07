using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class UnfoldOnHoverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _objectToFold = null;
    [SerializeField] private float _unfoldTime = 0.25f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer enter");
        LeanTween.cancel(_objectToFold);
        LeanTween.scaleY(_objectToFold, 1, _unfoldTime).setEaseOutBack();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer exit");
        LeanTween.cancel(_objectToFold);
        LeanTween.scaleY(_objectToFold, 0, _unfoldTime).setEaseInBack();
    }
}
