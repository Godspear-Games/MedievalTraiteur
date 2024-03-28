using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    
    public static EventManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public event Action<int> OnGameOver;
    public void GameOver(int totalscore)
    {
        OnGameOver?.Invoke(totalscore);
    }

    public event Action OnGameEnd;
    public void GameEnd()
    {
        OnGameEnd?.Invoke();
    }
    
    public event Action<int> OnShowEndScreen;
    public void ShowEndScreen(int income)
    {
        OnShowEndScreen?.Invoke(income);
    }

    public event Action<float,float> OnUpdateTimer;
    
    public void UpdateTimer(float time, float maxtime)
    {
        OnUpdateTimer?.Invoke(time, maxtime);
    }
    
    public event Action<OrderManager.Order> OnAddOrderTicket;
    
    public void AddOrderTicket(OrderManager.Order order)
    {
        OnAddOrderTicket?.Invoke(order);
    }
    
    public event Action<int,bool> OnRemoveOrderTicket;
    
    public void RemoveOrderTicket(int orderid,bool succes)
    {
        OnRemoveOrderTicket?.Invoke(orderid,succes);
    }
    
    public event Action<int, CustomerState> OnUpdateCustomerState;
    
    public void UpdateCustomerState(int orderid, CustomerState state)
    {
        OnUpdateCustomerState?.Invoke(orderid,state);
    }

    public event Action<Vector3> OnDoCurrencyPopup;
    public void DoCurrencyPopup(Vector3 position)
    {
        OnDoCurrencyPopup?.Invoke(position);
    }
    
    public event Action<int> OnQueueCurrencyUpdate;
    public void QueueCurrencyUpdate(int currency)
    {
        OnQueueCurrencyUpdate?.Invoke(currency);
    }

    public event Action OnUpdateCurrency;
    public void UpdateCurrency()
    {
        OnUpdateCurrency?.Invoke();
    }

}
