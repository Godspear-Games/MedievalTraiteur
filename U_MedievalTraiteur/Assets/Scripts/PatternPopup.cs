using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class PatternPopup : MonoBehaviour
{
    public Image tileImage;
    public TMP_Text tileName;

    private void OnEnable()
    {
        EventManager.Instance.OnShowPopup += ShowPopup;
    }
    
    private void OnDestroy()
    {
        EventManager.Instance.OnShowPopup -= ShowPopup;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnShowPopup -= ShowPopup;
    }

    public void ShowPopup(TileScriptableObject tile)
    {
        if (tile != null)
        {
            tileImage.sprite = tile.UISprite;
            tileName.text = tile.Name; 
            gameObject.SetActive(true);
        }
    }

    public void HidePopup()
    {
        gameObject.SetActive(false);
    }
}
