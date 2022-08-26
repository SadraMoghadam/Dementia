using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button quitButton;

    private GameManager _gameManager;
    private void OnEnable()
    {
        _gameManager = GameManager.instance;
        resumeButton.onClick.AddListener(OnResumeClick);
        newGameButton.onClick.AddListener(OnNewGameClick);
        quitButton.onClick.AddListener(OnQuitClick);
    }


    private void OnResumeClick()
    {
        _gameManager.LoadScene("Game");
    }
    
    private void OnNewGameClick()
    {
        _gameManager.playerPrefsManager.DeletePlayerPrefs();
        _gameManager.LoadScene("Game");
    }
    
    private void OnQuitClick()
    {
        Application.Quit();
    }
}
