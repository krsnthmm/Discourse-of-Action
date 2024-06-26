using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;

    [SerializeField] private float _moveSpeed;
    private Vector3 _direction;

    public void Move(float horizontalInput, float verticalInput)
    {
        Vector3 forwardDirection = Vector3.ProjectOnPlane(Camera.main.transform.up * verticalInput, Vector3.forward);
        Vector3 sideDirection = Vector3.ProjectOnPlane(Camera.main.transform.right * horizontalInput, Vector3.forward);
        _direction = (forwardDirection + sideDirection).normalized;
        _controller.Move(_moveSpeed * Time.deltaTime * _direction);
    }
}
