using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject _highlight;

    private void Start()
    {
        if (_highlight)
        {
            _highlight.SetActive(false);
        }
    }

    public void MouseExitMethod()
    {
        if (_highlight)
        {
            _highlight.SetActive(false);
        }
    }
    
    public void MouseEnterMethod()
    {
        if (_highlight)
        {
            _highlight.SetActive(true);
        }
    }
}
