using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum InteractableObjects
{
    Door,
    LeftDoor,
    RightDoor,
    Battery
}

public class PlayerRaycast : MonoBehaviour
{
    public float KeyDownCooldown = 1;
    [SerializeField] private Image leftMouseClickImage;
    [SerializeField] private Sprite keyDownSprite;
    [SerializeField] private Sprite keyUpSprite;
    [SerializeField] private int rayLength = 3;
    [SerializeField] private LayerMask layerMaskInteract;
    [SerializeField] private Transform camera;
    private Door _door;
    private InputManager _inputManager;
    private float _keyDownTimer;
    private GameController _gameController;
    private GameManager _gameManager;
    

    private void Start()
    {
        _inputManager = GetComponent<InputManager>();
        _gameController = GameController.instance;
        _gameManager = GameManager.instance;
        _keyDownTimer = KeyDownCooldown;
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, rayLength, layerMaskInteract))
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
                    try
                    {
                        _door = hit.collider.gameObject.GetComponent<Door>();
                        _door.ChangeDoorState();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("PlayerRaycast");
                    }
                }
                else if (hit.collider.CompareTag(InteractableObjects.LeftDoor.ToString()))
                {
                    Transform leftDoor = hit.collider.GetComponent<Transform>();
                    Vector3 temp = leftDoor.localEulerAngles;
                    switch (temp.y)
                    {
                        case 0:
                            temp.y = 280;
                            break;
                        case 280:
                            temp.y = 0;
                            break;
                        case 180:
                            temp.y = 260;
                            break;
                        case 260:
                            temp.y = 180;
                            break;
                    }
                    leftDoor.localEulerAngles = temp;
                }
                else if (hit.collider.CompareTag(InteractableObjects.RightDoor.ToString()))
                {
                    Transform rightDoor = hit.collider.GetComponent<Transform>();
                    Vector3 temp = rightDoor.localEulerAngles;
                    switch (temp.y)
                    {
                        case 0:
                            temp.y = 280;
                            break;
                        case 280:
                            temp.y = 0;
                            break;
                        case 180:
                            temp.y = 260;
                            break;
                        case 260:
                            temp.y = 180;
                            break;
                    }
                    rightDoor.localEulerAngles = temp;
                }
                InteractableItemsProcess(hit);
            }
        }
        else
        {
            leftMouseClickImage.gameObject.SetActive(false);
        }
    }


    private void InteractableItemsProcess(RaycastHit hit)
    {
        int currentInventoryItemsCount = _gameManager.playerPrefsManager.GetInt(PlayerPrefsKeys.InventoryInteractableItemsCount, 0);
        if(currentInventoryItemsCount >= _gameController.Inventory.inventorySpace)
            return;
        if (hit.collider.CompareTag(InteractableItemType.MedKit.ToString()))
        {
            InteractableItemOnClick(hit, InteractableItemType.MedKit);
        }
        else if (hit.collider.CompareTag(InteractableItemType.Battery.ToString()))
        {
            InteractableItemOnClick(hit, InteractableItemType.Battery);
        }
        else if (hit.collider.CompareTag(InteractableItemType.Flashlight.ToString()))
        {
            InteractableItemOnClick(hit, InteractableItemType.Flashlight);
        }
        else if (hit.collider.CompareTag(InteractableItemType.Key.ToString()))
        {
            InteractableItemOnClick(hit, InteractableItemType.Key);
        }
    }

    private void InteractableItemOnClick(RaycastHit hit, InteractableItemType type)
    {
        Destroy(hit.collider.gameObject, .1f);
        _gameController.Inventory.AddItem(type);
        InteractableItemInfo itemInfo = hit.collider.gameObject.GetComponent<InteractableItemInfo>();
        _gameManager.playerPrefsManager.AddDestroyedInteractableItem(itemInfo);
    }
    
}
