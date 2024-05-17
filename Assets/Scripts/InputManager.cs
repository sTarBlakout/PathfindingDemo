using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class InputManager : MonoBehaviour
{
    [SerializeField] private float speed;

    public Action<Vector3> OnGridClicked;
    
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
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
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
        if (movement.magnitude > 0) transform.position += movement * speed * Time.deltaTime;
    }
}
