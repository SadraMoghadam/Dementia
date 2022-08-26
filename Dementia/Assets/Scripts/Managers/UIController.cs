using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public InventoryPanel inventoryPanel;
    public FlashlightPanel flashlightPanel;
    public StaminaBar staminaBar;
    public DiedPanel diedPanel;
    public PausePanel pausePanel;
    public Image leftMouseClickImage;
    public Sprite keyDownSprite;
    public Sprite keyUpSprite;
    public Image darkBackground;
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
    }
    
    public void HidePausePanel()
    {
        darkBackground.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(false);
        _gameController.HideCursor();
    }
    
}
