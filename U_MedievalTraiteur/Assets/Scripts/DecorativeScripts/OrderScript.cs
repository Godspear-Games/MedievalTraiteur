using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class OrderScript : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer = null;
    [SerializeField] private int sortingAddition = 0;

    public void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();


            _spriteRenderer.sortingOrder = -(int)(transform.position.z*100f)+sortingAddition;
#if UNITY_EDITOR
        _spriteRenderer = GetComponent<SpriteRenderer>();
#endif
    }

#if UNITY_EDITOR

    public void Update()
    {
        _spriteRenderer.sortingOrder = -(int)(transform.position.z*100f)+sortingAddition;
    }

#endif

}
