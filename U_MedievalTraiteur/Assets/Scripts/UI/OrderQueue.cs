using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderQueue : MonoBehaviour
{
    //todo rework to show available recipes this round.
    
    private List<OrderManager.Order> _orderTickets = new List<OrderManager.Order>();
    private List<OrderTicketElement> _orderTicketElements = new List<OrderTicketElement>();

    [SerializeField] private OrderTicketElement _orderPrefab = null;
    [SerializeField] private Transform _orderParent = null;
    
    private void Start()
    {
        EventManager.Instance.OnAddOrderTicket += AddOrderTicket;
        EventManager.Instance.OnRemoveOrderTicket += RemoveOrderTicket;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnAddOrderTicket -= AddOrderTicket;
        EventManager.Instance.OnRemoveOrderTicket -= RemoveOrderTicket;
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnAddOrderTicket -= AddOrderTicket;
        EventManager.Instance.OnRemoveOrderTicket -= RemoveOrderTicket;
    }

    private void AddOrderTicket(OrderManager.Order order)
    {
        OrderTicketElement orderTicket = Instantiate(_orderPrefab, _orderParent);
        orderTicket.SetOrder(order);
        _orderTickets.Add(order);
        _orderTicketElements.Add(orderTicket);
    }
    
    private void RemoveOrderTicket(int orderidtoremove, bool succes)
    {
        _orderTickets.RemoveAt(orderidtoremove);
        Destroy(_orderTicketElements[orderidtoremove].gameObject);
        _orderTicketElements.RemoveAt(orderidtoremove);
    }
}
