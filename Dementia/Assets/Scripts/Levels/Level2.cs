using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Level2 : MonoBehaviour, ILevels
{
    [HideInInspector] public bool cutsceneTriggered;
    private GameController _gameController;
    private GameManager _gameManager;
    private int _level = 2;
    private float cutsceneTime = 3.41f;
    
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

    private void OnCutsceneStart(PlayableDirector obj)
    {
    }
    
    private void OnCutsceneEnd(PlayableDirector obj)
    {
    }
    
    public void Process()
    {
        if (cutsceneTriggered)
        {
            _gameController.DisableAllKeys();
            _gameController.PlayerController.animator.Play("FearBackwards");
        }
    }

    public void EndOfLevel()
    {
        _gameController.EnableAllKeys();
        _gameController.PlayerController.animator.Play("BaseState");
        _gameManager.playerPrefsManager.SaveGame(_level);
        _gameController.LevelsController.SetLevelActive(_level + 1);
    }
}