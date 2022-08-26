using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button checkpointButton;
    [SerializeField] private Button quitButton;

    private GameManager _gameManager;
    private UIController _uiController;
    private void OnEnable()
    {
        _gameManager = GameManager.instance;
        _uiController = UIController.instance;
        resumeButton.onClick.AddListener(OnResumeClick);
        settingsButton.onClick.AddListener(OnSettingsClicked);
        checkpointButton.onClick.AddListener(OnCheckpointClick);
        quitButton.onClick.AddListener(OnQuitClick);
    }


    private void OnResumeClick()
    {
        _uiController.HidePausePanel();
    }
    
    private void OnSettingsClicked()
    {
        _uiController.HidePausePanel();
    }
    
    private void OnCheckpointClick()
    {
        _gameManager.LoadScene("Game");
    }
    
    private void OnQuitClick()
    {
        _gameManager.LoadScene("MainMenu");
    }
}
