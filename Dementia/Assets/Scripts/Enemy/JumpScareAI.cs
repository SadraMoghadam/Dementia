using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScareAI : MonoBehaviour
{
    private GameController _gameController;
    
    private void OnEnable()
    {
        _gameController = GameController.instance;
    }

    private void Update()
    {
        _gameController.JumpScareController.LookAtPlayer();
    }
}
