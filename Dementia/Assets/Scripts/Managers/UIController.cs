using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject canvas;
    public InventoryPanel inventoryPanel;
    public FlashlightPanel flashlightPanel;
    public StaminaBar staminaBar;
    public DiedPanel diedPanel;
    public PausePanel pausePanel;
    public SettingsPanel settingsPanel;
    public Image leftMouseClickImage;
    public Sprite keyDownSprite;
    public Sprite keyUpSprite;
    public Image darkBackground;
    public InspectPanel inspectPanel;
    private GameController _gameController;
    
    
    public static UIController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        darkBackground.gameObject.SetActive(false);
        diedPanel.gameObject.SetActive(false);
        _gameController = GameController.instance;
        ShowGUI();
    }

    public void ShowDiedPanel()
    {
        darkBackground.gameObject.SetActive(true);
        diedPanel.gameObject.SetActive(true);
        _gameController.ShowCursor();
    }

    public void ShowPausePanel()
    {
        darkBackground.gameObject.SetActive(true);
        pausePanel.gameObject.SetActive(true);
        _gameController.ShowCursor();
        Time.timeScale = 0;
    }
    
    public void HidePausePanel()
    {
        darkBackground.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(false);
        settingsPanel.gameObject.SetActive(false);
        _gameController.HideCursor();
        Time.timeScale = 1;
    }

    public void ShowSettingsPanel()
    {
        settingsPanel.gameObject.SetActive(true);
    }

    public void HideGUI()
    {
        canvas.SetActive(false);
    }

    public void ShowGUI()
    {
        canvas.SetActive(true);
    }
}
