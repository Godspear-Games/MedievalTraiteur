using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using TMPro;
using UnityEngine;

public class AmountPopUp : MonoBehaviour
{
    [SerializeField] private TypewriterByCharacter _typewriter;

    public void SetAmount(int amount)
    {
        string amountString = amount.ToString();

        if (amount > 0)
        {
            amountString = "+" + amountString;
        }

        _typewriter.ShowText(amountString);
        DestroyPopUp();
    }
    
    public void DestroyPopUp()
    {
        TMP_Text text = GetComponentInChildren<TMP_Text>();
        
        LeanTween.moveY(gameObject, transform.position.y + 1, 1f).setEaseOutCubic();
        LeanTween.value(gameObject, 1, 0, 1f).setOnUpdate((float value) =>
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, value);
        });
        Destroy(gameObject, 2f);
    }
}
