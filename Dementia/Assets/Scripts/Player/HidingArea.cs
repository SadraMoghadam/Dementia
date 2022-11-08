using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingArea : MonoBehaviour
{
    private PlayerController _playerController;

    private void Start()
    {
        _playerController = GameController.instance.PlayerController;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerController.isHiding = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerController.isHiding = false;
        }
    }
}
