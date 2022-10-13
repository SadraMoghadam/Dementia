using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiedPanel : MonoBehaviour
{
    [SerializeField] private Button checkpointButton;
    [SerializeField] private Button quitButton;

    private GameManager _gameManager;
    private void OnEnable()
    {
        _gameManager = GameManager.instance;
        checkpointButton.onClick.AddListener(OnCheckpointClick);
        quitButton.onClick.AddListener(OnQuitClick);
        _gameManager.playerPrefsManager.SetFloat(PlayerPrefsKeys.Health, GameController.instance.DamageController.maxHealth);
    }

    private void OnCheckpointClick()
    {
        _gameManager.LoadScene("Game");
    }
    
    private void OnQuitClick()
    {
        _gameManager.LoadScene("Game");
    }
}
