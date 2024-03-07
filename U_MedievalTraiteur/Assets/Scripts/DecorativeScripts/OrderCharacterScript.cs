using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class OrderCharacterScript : MonoBehaviour
{
    private MeshRenderer _meshrenderer = null;
    [SerializeField] private int sortingAddition = 0;
    float CameraZ = 0;

    public void Start()
    {
        _meshrenderer = GetComponent<MeshRenderer>();
        
        _meshrenderer.sortingOrder = -(int)(transform.position.z*100f);
    }

    public void Update()
    {
        _meshrenderer.sortingOrder = -(int)(transform.position.z*100f);
    }

}
