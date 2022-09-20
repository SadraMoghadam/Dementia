using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Level1 : MonoBehaviour, ILevels
{
    [HideInInspector] private List<Door> securityGates;
    private GameController _gameController;
    private GameManager _gameManager;
    private int _level = 1;
    
    private void Start()
    {
        _gameController = GameController.instance;
        _gameManager = GameManager.instance;
        Setup();
    }

    private void Update()
    {
        Process();
    }

    public void Setup()
    {
        _gameController.QuestAndHintController.ShowQuest(_level);
    }

    public void Process()
    {
        int keysCount = _gameManager.playerPrefsManager.GetInteractableItemCount(InteractableItemType.Key);
        if (keysCount == 1)
        {
            EndOfLevel();
        }
    }

    public void EndOfLevel()
    {
        _gameManager.playerPrefsManager.SaveGame(_level);
        _gameController.LevelsController.SetLevelActive(_level + 1);
    }
}
