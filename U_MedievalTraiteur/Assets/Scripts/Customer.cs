using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Customer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private SpriteRenderer _spriteRenderer = null;
    
    private TileScriptableObject _desiredOrder = null;
    private Vector3 _initialImageScale;

    private int _movetween;
    private List<int> _movetweens = new List<int>();
    private List<int> _scaleTweens = new List<int>();
    
    [SerializeField] private GameObject _balloon = null;
    [SerializeField] private GameObject _orderImage = null;
    [SerializeField] private SpriteRenderer _orderSpriteRenderer = null;
    
    int customerid = 0;
    
    CustomerSpawner _spawner = null;
    
    public void SetupCustomer(TileScriptableObject order, int id, CustomerSpawner spawner)
    {
        _initialImageScale = _spriteRenderer.transform.localScale;
        _desiredOrder = order;
        _balloon.transform.localScale = Vector3.zero;
        _orderSpriteRenderer.sprite = order.UISprite;
        customerid = id;
        _spawner = spawner;
    }

    public void MoveTo(Vector3 position)
    {
        //cancel all movetweens
        foreach (var tween in _movetweens)
        {
            LeanTween.cancel(tween);
        }
        
        _movetweens.Add(LeanTween.move(gameObject, position, 1f).id);

        _movetweens.Add(LeanTween
            .moveLocalY(_spriteRenderer.gameObject, _spriteRenderer.transform.localPosition.y + 0.3f, 1 / 8f)
            .setLoopPingPong(4).setEaseOutSine().id);
    }
    
    public void DestroyCustomer()
    {
        LeanTween.delayedCall(1f, () =>
        {
            Destroy(gameObject);
        });
    }

    public void SetState(CustomerState state)
    {
        switch (state)
        {
            case CustomerState.Waiting:
                _spriteRenderer.color = Color.white;
                break;
            case CustomerState.Impatient:
                _spriteRenderer.color = Color.yellow;
                break;
            case CustomerState.Happy:
                _spriteRenderer.color = Color.green;
                break;
            case CustomerState.Angry:
                _spriteRenderer.color = Color.red;
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OrderManager.Instance.SelectCustomer(_spawner.GetCustomerId(this));
        
        //cancel all scale tweens
        foreach (int tween in _scaleTweens)
        {
            LeanTween.cancel(tween);
        }
        
        _spriteRenderer.transform.localScale = _initialImageScale;
        _scaleTweens.Add(LeanTween.scaleY(_spriteRenderer.gameObject, _initialImageScale.y * 1.2f, 0.3f).setEaseOutQuad().id);
        _scaleTweens.Add(LeanTween.scale(_balloon, Vector3.one, 0.3f).setEaseOutBack().id);
        _scaleTweens.Add(LeanTween.scale(_orderImage, Vector3.one, 0.4f).setEaseOutBack().id);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OrderManager.Instance.DeselectCustomer(_spawner.GetCustomerId(this));
        
        foreach (int tween in _scaleTweens)
        {
            LeanTween.cancel(tween);
        }
        
        _scaleTweens.Add(LeanTween.scaleY(_spriteRenderer.gameObject, _initialImageScale.y, 0.3f).setEaseOutQuad().id);
        
        _scaleTweens.Add(LeanTween.scale(_orderImage, Vector3.zero, 0.3f).setEaseInOutSine().id);
        _scaleTweens.Add(LeanTween.scale(_balloon, Vector3.zero, 0.3f).setEaseInOutSine().id);
    }
}

public enum CustomerState
{
    Waiting,
    Impatient,
    Happy,
    Angry
}
