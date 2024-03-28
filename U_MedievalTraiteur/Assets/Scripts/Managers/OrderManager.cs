using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class OrderManager : MonoBehaviour
{
    public int MaxOrders = 3;
    public float MinTimeBetweenOrders = 5f;
    public float MaxTimeBetweenOrders = 10f;
    public float TimeToCompleteOrder = 10f;
    public List<Order> Orders = new List<Order>();

    private float _newOrderCountdown = 0f;

    private int _selectedCustomerIndex = -1;
    
    public static OrderManager Instance = null;
    
    private bool _ordersEnabled = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        UpdateOrders();
    }

    public void EnableOrders(bool enabled)
    {
        _ordersEnabled = enabled;
    }
    
    public void GenerateOrder()
    {
        PatternManager.Instance.GetPossibleOrderPatterns();
        //get a random order
        Order order = new Order();
        order.OrderPattern = PatternManager.Instance.GetPossibleOrderPatterns()[Random.Range(0, PatternManager.Instance.GetPossibleOrderPatterns().Count)];
        order.OrderTime = TimeToCompleteOrder;
        
        //add order to the list
        Orders.Add(order);
        
        EventManager.Instance.AddOrderTicket(order);
    }
    
    public void UpdateOrders()
    {
        //decrease time for each order
        for (var index = Orders.Count - 1; index >= 0; index--)
        {
            Order order = Orders[index];
            order.OrderTime -= Time.deltaTime;
        }

        //remove orders that are failed
        for (var index = Orders.Count - 1; index >= 0; index--)
        {
            Order order = Orders[index];
            if (order.OrderTime <= 0)
            {
                Orders.RemoveAt(index);
                EventManager.Instance.RemoveOrderTicket(index,false);
                
                //if this is the last order and new orders are disabled
                if (Orders.Count <= 0 && _ordersEnabled == false)
                {
                    GameLoopManager.Instance.OrdersAllEnded();
                }
                
            }
        }
        
        //only go past this point if orders are enabled

        if (_ordersEnabled == false)
        {
            return;
        }
        
        //if no orders are left, generate a new order
        if (Orders.Count <= 0)
        {
            GenerateOrder();
            ResetCountdownForNewOrder();
        }
        else
        {
            _newOrderCountdown -= Time.deltaTime;
            if (_newOrderCountdown <= 0)
            {
                GenerateOrder();
                ResetCountdownForNewOrder();
            }
        }
    }

    public void SelectCustomer(int index)
    {
        _selectedCustomerIndex = index;
    }

    public void DeselectCustomer(int index)
    {
        if (_selectedCustomerIndex == index)
        {
            _selectedCustomerIndex = -1;
        }
    }
    
    public bool TryCompleteOrder(TileScriptableObject order)
    {
        if (_selectedCustomerIndex == -1)
        {
            return false;
        }
        
        if (Orders.Count <= 0)
        {
            return false;
        }
        
        Debug.Log("selected customer" + _selectedCustomerIndex );

        Order currentOrder = Orders[_selectedCustomerIndex];
        if (currentOrder.OrderPattern.OutputDish == order)
        {
            CurrencyManager.Instance.AddCurrency(order.SoulValue);
            Orders.RemoveAt(_selectedCustomerIndex);
            EventManager.Instance.RemoveOrderTicket(_selectedCustomerIndex,true);
            Debug.Log("Correct order given");
            
            if (Orders.Count <= 0 && _ordersEnabled == false)
            {
                GameLoopManager.Instance.OrdersAllEnded();
            }
            
            return true;
        }

        return false;
    }
    
    private void ResetCountdownForNewOrder()
    {
        _newOrderCountdown = Random.Range(MinTimeBetweenOrders, MaxTimeBetweenOrders);
    }

    [Serializable]
    public class Order
    {
        [FormerlySerializedAs("OrderDish")] public PatternDefinitionScriptableObject OrderPattern;
        public float OrderTime;
    }
}
