using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera _mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = _mainCamera.transform.position;
        targetPosition = transform.position - targetPosition;
        transform.LookAt(targetPosition);
        
        transform.rotation = Quaternion.Euler(Mathf.Clamp(transform.rotation.eulerAngles.x,20,70), 0, 0);
        
    }
}
