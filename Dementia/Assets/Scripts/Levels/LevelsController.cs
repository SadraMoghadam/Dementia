using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsController : MonoBehaviour
{
    [HideInInspector] public Level0 level0;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.instance;
        level0 = GetComponentInChildren<Level0>();
        if (_gameManager.playerPrefsManager.GetInt(PlayerPrefsKeys.Level, -1) == 0)
        {
            level0.gameObject.SetActive(false);
        }
    }
    
}
