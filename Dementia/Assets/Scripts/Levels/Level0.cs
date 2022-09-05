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
    private Animator _playerAnimator;

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
        _gameController.SetPlayerTransform(spawnTransform);
        _playerAnimator = _gameController.PlayerController.transform.GetComponent<Animator>();
        _playerAnimator.Play("StandUp");
    }

    public void Process()
    {
        _timer += Time.deltaTime;
        if (_timer >= cutsceneTime && _gameController.KeysDisabled)
        {
            _gameController.EnableAllKeys();
            _playerAnimator.Play("BaseState");
            _gameController.PlayerController.SetStickyCamera(false);
            _gameController.QuestAndHintController.ShowQuest(0);
        }
        bool hasFlashlight = _gameManager.playerPrefsManager.GetBool(PlayerPrefsKeys.HasFlashlight, false);
        bool lightsEnabled = _gameManager.playerPrefsManager.GetBool(PlayerPrefsKeys.LightsEnabled, true);
        if (hasFlashlight && lightsEnabled)
        {
            _gameController.LightsController.TurnAllLightsOnOrOff(false);
            collider.SetActive(false);
            _gameManager.playerPrefsManager.SetBool(PlayerPrefsKeys.LightsEnabled, false);
            _gameController.QuestAndHintController.ShowHint(0);
            EndOfLevel();
        }
    }

    public void EndOfLevel()
    {
        _gameController.EnableAllKeys();
        _gameManager.playerPrefsManager.SetInt(PlayerPrefsKeys.Level, _level);
        _gameManager.playerPrefsManager.SaveGame();
        _gameController.LevelsController.SetLevelActive(_level + 1);
    }
}