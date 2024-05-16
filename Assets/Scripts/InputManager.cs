using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] public float acceleration;

    public Action<Vector3> OnGridClicked;
    
    private float _currentSpeed = 0f;
    private Camera _camera;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        ProcessCameraMovement();
        ProcessGridClick();
    }

    private void ProcessGridClick()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                OnGridClicked?.Invoke(hit.point);
            }
        }
    }

    private void ProcessCameraMovement()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");
        
        var movement = new Vector3(horizontalInput, 0, verticalInput).normalized;
        if (movement.magnitude > 0)
        {
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, maxSpeed, acceleration * Time.deltaTime);
            transform.position += movement * _currentSpeed * Time.deltaTime;
        }
        else
        {
            _currentSpeed = 0;
        }
    }
}
