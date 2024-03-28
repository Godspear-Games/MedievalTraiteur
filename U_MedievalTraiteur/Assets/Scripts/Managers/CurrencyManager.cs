using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    private int _currency = 0;

    public static CurrencyManager Instance = null;

    private void Awake()
    {
        Instance = this;
    }

    public void SetupCurrency()
    {
        EventManager.Instance.QueueCurrencyUpdate(0);
        EventManager.Instance.UpdateCurrency();
    }

    public void AddCurrency(int currency)
    {
        _currency += currency;
        EventManager.Instance.QueueCurrencyUpdate(_currency);
    }
    
    public void RemoveCurrency(int currency)
    {
        _currency -= currency;
        EventManager.Instance.QueueCurrencyUpdate(_currency);
        EventManager.Instance.UpdateCurrency();
    }
    
    public int GetCurrency()
    {
        return _currency;
    }
}
