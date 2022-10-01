using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3 : MonoBehaviour, ILevels
{
    private GameController _gameController;
    private GameManager _gameManager;
    private int _level = 3;
    
    private void Start()
    {
        _gameController = GameController.instance;
        _gameManager = GameManager.instance;
        Setup();
    }

    private void LateUpdate()
    {
        Process();
    }

    public void Setup()
    {
        _gameController.QuestAndHintController.ShowQuest(_level);
    }
    
    public void Process()
    {
        
    }

    public void EndOfLevel()
    {
        _gameManager.playerPrefsManager.SaveGame(_level);
        _gameController.LevelsController.SetLevelActive(_level + 1);
    }
}
