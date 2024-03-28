using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private List<Customer> _customerPrefabs = new List<Customer>();
    private List<Customer> _spawnedCustomers = new List<Customer>();

    [SerializeField] private List<Transform> _customerSpawnPoints = new List<Transform>();
    private List<Customer> _spawnPointOccupiers = new List<Customer>();
    [SerializeField] private Transform _exitPoint = null;
    
    // Start is called before the first frame update
    void Start()
    {
        SetupSpawnPoints();
        EventManager.Instance.OnAddOrderTicket += SpawnCustomer;
        EventManager.Instance.OnRemoveOrderTicket += RemoveCustomer;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnAddOrderTicket -= SpawnCustomer;
        EventManager.Instance.OnRemoveOrderTicket -= RemoveCustomer;
    }
    
    private void OnDestroy()
    {
        EventManager.Instance.OnAddOrderTicket -= SpawnCustomer;
        EventManager.Instance.OnRemoveOrderTicket -= RemoveCustomer;
    }
    
    private void SetupSpawnPoints()
    {
        for (int i = 0; i < _customerSpawnPoints.Count; i++)
        {
            _spawnPointOccupiers.Add(null);
        }
    }
    
    private void SpawnCustomer(OrderManager.Order order)
    {
        Customer customer = Instantiate(_customerPrefabs[UnityEngine.Random.Range(0, _customerPrefabs.Count)], transform.position, Quaternion.identity);
        _spawnedCustomers.Add(customer);
        customer.SetupCustomer(order.OrderPattern.OutputDish,_spawnedCustomers.Count-1, this);
        
        //spawn on exit point
        customer.transform.position = _exitPoint.position;
        //move to a spawn point that isnt occupied
        for (int i = 0; i < _customerSpawnPoints.Count; i++)
        {
            if (_spawnPointOccupiers[i] == null)
            {
                customer.MoveTo(_customerSpawnPoints[i].position);
                _spawnPointOccupiers[i] = customer;
                break;
            }
        }
    }

    public int GetCustomerId(Customer customer)
    {
        //get the customer id
        for (int i = 0; i < _spawnedCustomers.Count; i++)
        {
            if (_spawnedCustomers[i] == customer)
            {
                return i;
            }
        }

        return -1;
    }
    
    private void RemoveCustomer(int order, bool success)
    {
        //remove customer from spawn point occupiers
        for (int i = 0; i < _spawnPointOccupiers.Count; i++)
        {
            if (_spawnPointOccupiers[i] == _spawnedCustomers[order])
            {
                _spawnPointOccupiers[i] = null;
                break;
            }
        }

        if (success)
        {
            EventManager.Instance.DoCurrencyPopup(_spawnedCustomers[order].transform.position+Vector3.up);
        }

        //move customer to exit point and then destroy it and remove it from the list
        _spawnedCustomers[order].MoveTo(_exitPoint.position);
        _spawnedCustomers[order].DestroyCustomer();
        _spawnedCustomers.RemoveAt(order);
    }

    private void UpdateCustomerState(int order, CustomerState state)
    {
        
    }
}
