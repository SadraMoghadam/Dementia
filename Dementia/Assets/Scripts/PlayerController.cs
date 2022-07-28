using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    [SerializeField] private float animBlendSpeed = 8.9f;
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private Transform camera;
    [SerializeField] private float upperLimit = -40f;
    [SerializeField] private float bottomLimit = 70f;
    [SerializeField] private float mouseSensitivity = 10f;
    private Rigidbody _playerRigidbody;
    private InputManager _inputManager;
    private Animator _animator;
    private bool _grounded = false;
    private bool _hasAnimator;
    private int _xVelocityHash;
    private int _yVelocityHash;
    private float _xRotation;
    private const float _walkSpeed = 4f;
    private const float _runSpeed = 6f;
    private Vector2 _currentVelocity;
        
    private void Start() 
    {
        _hasAnimator = TryGetComponent<Animator>(out _animator);
        // _characterController = GetComponent<CharacterController>();
        _playerRigidbody = GetComponent<Rigidbody>();
        _inputManager = GetComponent<InputManager>();
        _xVelocityHash = Animator.StringToHash("XVelocity");
        _yVelocityHash = Animator.StringToHash("YVelocity");
    }

    private void FixedUpdate() 
    {
        Move();
    }
    private void LateUpdate()
    {
        CameraMovement();
    }
    
    private void Move()
    {
        if(!_hasAnimator) 
            return;
        float targetSpeed = _inputManager.Run ? _runSpeed : _walkSpeed;
        if(_inputManager.Move ==Vector2.zero) 
            targetSpeed = 0;
        _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, _inputManager.Move.x * targetSpeed, animBlendSpeed * Time.fixedDeltaTime);
        _currentVelocity.y = Mathf.Lerp(_currentVelocity.y, _inputManager.Move.y * targetSpeed, animBlendSpeed * Time.fixedDeltaTime);
        float step = targetSpeed * Time.deltaTime;
        var newPos = new Vector3(_currentVelocity.x, 0, _currentVelocity.y);
        transform.position += transform.TransformDirection (newPos / 40);
        _animator.SetFloat(_xVelocityHash, _currentVelocity.x);
        _animator.SetFloat(_yVelocityHash, _currentVelocity.y);
    }

    private void CameraMovement()
    {
        if(!_hasAnimator) 
            return;
        var Mouse_X = _inputManager.Look.x;
        var Mouse_Y = _inputManager.Look.y;
        camera.position = cameraRoot.position;
        _xRotation -= Mouse_Y * mouseSensitivity * Time.smoothDeltaTime;
        _xRotation = Mathf.Clamp(_xRotation, upperLimit, bottomLimit);
        camera.localRotation = Quaternion.Euler(_xRotation, 0 , 0);
        _playerRigidbody.MoveRotation(_playerRigidbody.rotation * Quaternion.Euler(0, Mouse_X * mouseSensitivity * Time.smoothDeltaTime, 0));
    }

    // private void Move()
    // {
    //     if(!_hasAnimator) 
    //         return;
    //     float targetSpeed = _inputManager.Run ? _runSpeed : _walkSpeed;
    //     if (_inputManager.Move == Vector2.zero)
    //         targetSpeed = 0.1f;
    //     _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, targetSpeed * _inputManager.Move.x, animationSpeed * Time.fixedDeltaTime);
    //     _currentVelocity.y = Mathf.Lerp(_currentVelocity.y, targetSpeed * _inputManager.Move.y, animationSpeed * Time.fixedDeltaTime);
    //
    //     var xVelocityDifference = _currentVelocity.x - _playerRigidbody.velocity.x;
    //     var zVelocityDifference = _currentVelocity.y - _playerRigidbody.velocity.z;
    //     
    //     _playerRigidbody.AddForce(transform.TransformVector(new Vector3(xVelocityDifference, 0, zVelocityDifference)), ForceMode.VelocityChange);
    //     
    //     _animator.SetFloat(_xVelocityHash, _currentVelocity.x);
    //     _animator.SetFloat(_yVelocityHash, _currentVelocity.y);
    // }
    //
    // private void CameraMovement()
    // {
    //     if(!_hasAnimator) 
    //         return;
    //     var mouseX = _inputManager.Look.x;
    //     var mouseY = _inputManager.Look.y;
    //     camera.position = cameraRoot.position;
    //     _xRotation -= mouseY * mouseSensitivity * Time.deltaTime;
    //     _xRotation = Mathf.Clamp(_xRotation, upperLimit, bottomLimit);
    //     
    //     camera.localRotation = Quaternion.Euler(_xRotation, 0, 0);
    //     transform.Rotate(Vector3.up, mouseX * mouseSensitivity * Time.deltaTime);
    // }

    
}
