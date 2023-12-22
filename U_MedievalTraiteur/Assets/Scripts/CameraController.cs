using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private CameraControlActions cameraActions;
    private InputAction movement;
    private Transform cameraTransform;

    //Horizontal motion
    [SerializeField]
    private float maxSpeed = 5f;
    private float speed;
    [SerializeField]
    private float acceleration = 10f;
    [SerializeField]
    private float damping = 15f;

    //Vertical motion - zooming
    [SerializeField]
    private float stepSize = 2f;
    [SerializeField]
    private float zoomDampening = 7.5f;
    [SerializeField]
    private float minHeight = 5f;
    [SerializeField]
    private float maxHeight = 50f;
    [SerializeField]
    private float zoomSpeed = 2f;

    //Rotation
    [SerializeField]
    private float maxRotationSpeed = 1f;

    //Screen edge motion
    [SerializeField]
    [Range(0f, 0.1f)]
    private float edgeTolerance = 0.05f;
    [SerializeField]
    private bool useScreenEdge = true;

    //Value set in various functions
    //used to update the position of the camera base object
    private Vector3 targetPosition;

    private float zoomHeight;

    //Used to track and maintain velocity w/o a rigidbody
    private Vector3 horizontVelocity;
    private Vector3 lastPosition;

    //tracks where the dragging action started
    Vector3 startDrag;


    private void Awake()
    {
        cameraActions = new CameraControlActions();
        cameraTransform = this.GetComponentInChildren<Camera>().transform; 
    }

    private void OnEnable()
    {
        lastPosition = this.transform.position;
        movement = cameraActions.Camera.Movement;
        cameraActions.Camera.Enable(); 
    }



}
