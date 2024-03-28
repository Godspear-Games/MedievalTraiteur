using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    public static GameLoopManager Instance = null;
    [SerializeField] private float _shiftTime = 60f;

    [SerializeField] private Vector2 _defaultGridSize;
    [SerializeField] private List<Vector2> _unlockedUpgradeSlots;
    
    private bool _timerEnded = false;
    private bool _ordersEnded = false;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        //load upgrades & necessary info for game

        //generate grid + upgrades + visuals + etc...
        CookingGridManager.Instance.SetupCookingGrid(_defaultGridSize, _unlockedUpgradeSlots);
        //give player ingredients
        IngredientBenchManager.Instance.SetupIngredients();
        //start generating orders
        //start timer
        TimerManager.Instance.SetShiftTime(_shiftTime);

        yield return new WaitForSecondsRealtime(1);
        
        TimerManager.Instance.StartTimer();
        CurrencyManager.Instance.SetupCurrency();
        OrderManager.Instance.EnableOrders(true);
    }

    //if field is full
    public void LoseGame()
    {
        StartCoroutine(ShowEndScreen());
    }
    
    //if timer runs out
    public void TimerEnded()
    {
        _timerEnded = true;
        OrderManager.Instance.EnableOrders(false);
        //stop generating orders
        StartCoroutine(ShowEndScreen());
        //show end screen
    }

    public void OrdersAllEnded()
    {
        _ordersEnded = true;
    }
    
    private IEnumerator ShowEndScreen()
    {
        yield return new WaitUntil(() => _timerEnded && _ordersEnded);

        EventManager.Instance.ShowEndScreen(CurrencyManager.Instance.GetCurrency());
        //show end screen
        
    }
}
