using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CurrencySpawnerUI : MonoBehaviour
{
    [SerializeField] private GameObject _currencyPrefab = null;
    [SerializeField] private Transform _currencyTarget = null;

    private void Start()
    {
        EventManager.Instance.OnDoCurrencyPopup += SpawnCurrency;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnDoCurrencyPopup -= SpawnCurrency;
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnDoCurrencyPopup -= SpawnCurrency;
    }

    private void SpawnCurrency(Vector3 realworldposition)
    {
        //translate realworldposition to screenposition
        Vector3 screenposition = Camera.main.WorldToScreenPoint(realworldposition+Vector3.up*2f);
        //spawn currency at screenposition
        GameObject currency = Instantiate(_currencyPrefab, screenposition, Quaternion.identity,transform);
        MoveCurrency(currency);
    }
    
    private void MoveCurrency(GameObject currency)
    {
        LeanTween.scale(currency, Vector3.one * 1.5f, 0.5f).setEaseOutBack();
        LeanTween.delayedCall(0.3f, () =>
        {
            LeanTween.moveX(currency, _currencyTarget.position.x, 0.5f).setEaseOutQuad();
            LeanTween.moveY(currency, _currencyTarget.position.y, 0.5f).setEaseInOutQuad();
            LeanTween.scale(currency, Vector3.zero,0.7f).setEaseOutExpo().setDelay(0.5f);
            LeanTween.delayedCall(0.7f, () =>
            {
                LeanTween.cancel(currency);
                EventManager.Instance.UpdateCurrency();
                Destroy(currency);
            });
        });

    }
}
