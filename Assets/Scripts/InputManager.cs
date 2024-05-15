using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] public float acceleration;
    
    private float _currentSpeed = 0f;

    private void Update()
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
