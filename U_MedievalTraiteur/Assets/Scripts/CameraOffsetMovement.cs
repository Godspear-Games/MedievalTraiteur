using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOffsetMovement : MonoBehaviour
{
    [SerializeField] private float _modifier = 1.0f;
    private Vector3 _initialPosition;

    private void Start()
    {
        _initialPosition = transform.position;
    }
    
    private void Update()
    {
        UpdateCameraBasedOnMousePosition();
    }

    private void UpdateCameraBasedOnMousePosition()
    {
        Vector2 mouseOffsetFromCenter = new Vector2(Mouse.current.position.ReadValue().x - Screen.width / 2, Mouse.current.position.ReadValue().y - Screen.height / 2);
        mouseOffsetFromCenter = new Vector2(mouseOffsetFromCenter.x / Screen.width, mouseOffsetFromCenter.y / Screen.height);
        
        Vector3 targetPosition = _initialPosition + new Vector3(mouseOffsetFromCenter.x * _modifier, mouseOffsetFromCenter.y * _modifier, 0);
        
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5);
    }
}
