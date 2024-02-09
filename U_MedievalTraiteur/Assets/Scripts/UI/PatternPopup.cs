using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class PatternPopup : MonoBehaviour
{
    public Image tileImage;
    public TMP_Text tileName;
    [SerializeField] private RecipeDisplayUI _recipeDisplayUI;

    private Queue<PatternDefinitionScriptableObject> _popUpQueue = new Queue<PatternDefinitionScriptableObject>();
    
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

    public void AddToPopUpQueue(PatternDefinitionScriptableObject patternDefinitionScriptableObject)
    {
        Debug.Log("AddToPopUpQueue");
        if (patternDefinitionScriptableObject.OutputStructure != null && TemporaryPopUpSaveVariable(patternDefinitionScriptableObject.OutputStructure) == false)
        {
            Debug.Log("add to queue");
            _popUpQueue.Enqueue(patternDefinitionScriptableObject);
            Debug.Log("queue count"+ _popUpQueue.Count);
            if (_popUpQueue.Count > 0 && transform.GetChild(0).gameObject.activeSelf == false)
            {
                ShowPopUp(_popUpQueue.Dequeue());
            }
        }
    }

    public void ShowPopUp(PatternDefinitionScriptableObject patternDefinitionScriptableObject)
    {
        _recipeDisplayUI.ShowRecipe(patternDefinitionScriptableObject);
        transform.GetChild(0).gameObject.SetActive(true);
        tileImage.sprite = patternDefinitionScriptableObject.OutputStructure.UISprite;
        tileName.text = patternDefinitionScriptableObject.OutputStructure.Name;
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
