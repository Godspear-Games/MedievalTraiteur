using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class PatternPopup : MonoBehaviour
{
    public Image tileImage;
    public TMP_Text tileName;

    private Queue<TileScriptableObject> _popUpQueue = new Queue<TileScriptableObject>();
    
    private void Start()
    {
        EventManager.Instance.OnShowPopup += AddToPopUpQueue;
        HidePopUp();
    }
    
    private void OnDestroy()
    {
        EventManager.Instance.OnShowPopup -= AddToPopUpQueue;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnShowPopup -= AddToPopUpQueue;
    }

    public void AddToPopUpQueue(TileScriptableObject tile)
    {
        Debug.Log("AddToPopUpQueue");
        if (tile != null && TemporaryPopUpSaveVariable(tile) == false)
        {
            Debug.Log("add to queue");
            _popUpQueue.Enqueue(tile);
            Debug.Log("queue count"+ _popUpQueue.Count);
            if (_popUpQueue.Count > 0)
            {
                ShowPopUp(_popUpQueue.Dequeue());
            }
        }
    }

    public void ShowPopUp(TileScriptableObject tile)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        tileImage.sprite = tile.UISprite;
        tileName.text = tile.Name;
    }

    public void ContinuePopUpQueue()
    {
        if (_popUpQueue.Count > 0)
        {
            ShowPopUp(_popUpQueue.Dequeue());
        }
        else
        {
            HidePopUp();
        }
    }

    public void HidePopUp()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private bool TemporaryPopUpSaveVariable(TileScriptableObject tile)
    {
        if (PlayerPrefs.HasKey(tile.Name))
        {
            return true;
        }
        else
        {
            //save now
            PlayerPrefs.SetInt(tile.Name, 1);
            return false;
        }
    }
}
