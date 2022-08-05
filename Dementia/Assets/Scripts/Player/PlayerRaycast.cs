using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum InteractableObjects
{
    Door,
    Battery
}

public class PlayerRaycast : MonoBehaviour
{
    public float KeyDownCooldown = 1;
    [SerializeField] private Image leftMouseClickImage;
    [SerializeField] private Sprite keyDownSprite;
    [SerializeField] private Sprite keyUpSprite;
    [SerializeField] private int rayLength = 5;
    [SerializeField] private LayerMask layerMaskInteract;
    [SerializeField] private Transform camera;
    private Door _door;
    private InputManager _inputManager;
    private float _keyDownTimer;
    

    private void Start()
    {
        _inputManager = GetComponent<InputManager>();
        _keyDownTimer = KeyDownCooldown;
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, camera.forward, out hit, rayLength, layerMaskInteract))
        {
            leftMouseClickImage.gameObject.SetActive(true);
            _keyDownTimer += Time.deltaTime;
            if (_keyDownTimer < KeyDownCooldown)
            {
                leftMouseClickImage.sprite = keyDownSprite;
                return;
            }
            leftMouseClickImage.sprite = keyUpSprite;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                _keyDownTimer = 0;
                if (hit.collider.CompareTag(InteractableObjects.Door.ToString()))
                {
                    _door = hit.collider.gameObject.GetComponent<Door>();
                    _door.ChangeDoorState();

                }
            }
        }
        else
        {
            leftMouseClickImage.gameObject.SetActive(false);
        }
    }
}
