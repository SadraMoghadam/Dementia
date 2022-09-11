using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private Button controlsButton;
    [SerializeField] private Button audioButton;
    [SerializeField] private Button videoButton;
    [SerializeField] private Button gameplayButton;
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject audioPanel;
    [SerializeField] private GameObject videoPanel;
    [SerializeField] private GameObject gameplayPanel;
    
    private GameManager _gameManager;
    private UIController _uiController;
    
    
    private void OnEnable()
    {
        _gameManager = GameManager.instance;
        _uiController = UIController.instance;
        controlsButton.onClick.AddListener(OnControlsClick);
        audioButton.onClick.AddListener(OnAudioClicked);
        videoButton.onClick.AddListener(OnVideoClick);
        gameplayButton.onClick.AddListener(OnGameplayClick);
        backButton.onClick.AddListener(OnBackClick);
    }

    private void OnControlsClick()
    {
        controlsPanel.SetActive(true);
    }
    
    private void OnAudioClicked()
    {
        audioPanel.SetActive(true);
    }
    
    private void OnVideoClick()
    {
        videoPanel.SetActive(true);
    }
    
    private void OnGameplayClick()
    {
        gameplayPanel.SetActive(true);
    }
    
    private void OnBackClick()
    {
        gameObject.SetActive(false);
    }
}
