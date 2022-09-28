using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Level0 : MonoBehaviour, ILevels
{
    // public delegate void OnMovementChanged(float direction);
    // public event OnMovementChanged OnMoveMentChanged;
    [SerializeField] private GameObject collider;
    [SerializeField] private Transform spawnTransform;
    [SerializeField] private float cutsceneTime = 16.25f;
    private GameController _gameController;
    private GameManager _gameManager;
    private int _level = 0;
    private float _timer;

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
        collider.SetActive(true);
        _gameController.DisableAllKeys();
        _timer = 0;
        _gameController.PlayerController.SetStickyCamera(true);
        // StartCoroutine(_gameController.PlayerController.CutSceneColliderActivation(true));
        _gameController.SetPlayerTransform(spawnTransform);
        _gameController.PlayerController.animator.Play("StandUp");
        StartCoroutine(_gameController.PlayerController.BlurOutOverTime(40, 0, 15));
    }

    public void Process()
    {
        _timer += Time.deltaTime;
        if (_timer >= cutsceneTime && _gameController.KeysDisabled)
        {
            _gameController.EnableAllKeys();
            _gameController.PlayerController.animator.Play("BaseState");
            _gameController.PlayerController.SetStickyCamera(false);
            // StartCoroutine(_gameController.PlayerController.CutSceneColliderActivation(false));
            _gameController.QuestAndHintController.ShowQuest(0);
        }
        bool hasFlashlight = _gameManager.playerPrefsManager.GetBool(PlayerPrefsKeys.HasFlashlight, false);
        bool lightsEnabled = _gameManager.playerPrefsManager.GetBool(PlayerPrefsKeys.LightsEnabled, true);
        if (hasFlashlight && lightsEnabled)
        {
            _gameController.LightsController.TurnAllLightsOnOrOff(false);
            collider.SetActive(false);
            _gameManager.playerPrefsManager.SetBool(PlayerPrefsKeys.LightsEnabled, false);
            EndOfLevel();
        }
    }

    public void EndOfLevel()
    {
        _gameController.EnableAllKeys();
        _gameManager.playerPrefsManager.SaveGame(_level);
        _gameController.LevelsController.SetLevelActive(_level + 1);
    }
}
