using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyCountListenerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _currencyText = null;
    private Queue<int> _currencyUpdates = new Queue<int>();
    
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.OnQueueCurrencyUpdate += ReceiveCurrencyUpdate;
        EventManager.Instance.OnUpdateCurrency += ShowCurrencyUpdate;
    }
    
    private void OnDisable()
    {
        EventManager.Instance.OnQueueCurrencyUpdate -= ReceiveCurrencyUpdate;
        EventManager.Instance.OnUpdateCurrency -= ShowCurrencyUpdate;
    }
    
    private void OnDestroy()
    {
        EventManager.Instance.OnQueueCurrencyUpdate -= ReceiveCurrencyUpdate;
        EventManager.Instance.OnUpdateCurrency -= ShowCurrencyUpdate;
    }

    private void ReceiveCurrencyUpdate(int currency)
    {
        _currencyUpdates.Enqueue(currency);
    }
    
    private void ShowCurrencyUpdate()
    {
        LeanTween.scale(gameObject, Vector3.one * 0.9f, 0.6f).setEasePunch();
        _currencyText.text = "<sprite=0> " + _currencyUpdates.Dequeue().ToString();
    }
}
