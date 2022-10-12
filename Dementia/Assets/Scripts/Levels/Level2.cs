using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Level2 : MonoBehaviour, ILevels
{
    public GameObject cutsceneTrigger;
    [SerializeField] private Transform doctorStartPosition;
    [SerializeField] private Transform doctorEndPosition;
    [SerializeField] private Door kitchenDoor;
    [SerializeField] private float doctorSpeed = 2;
    [HideInInspector] public bool cutsceneTriggered;
    private GameController _gameController;
    private GameManager _gameManager;
    private int _level = 2;
    private float cutsceneTime = 3.41f * 1.2f;
    
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
        cutsceneTrigger.SetActive(true);
    }
    
    public void Process()
    {
        if (cutsceneTriggered)
        {
            cutsceneTriggered = false;
            StartCoroutine(Cutscene());
        }
    }

    private IEnumerator Cutscene()
    {
        cutsceneTrigger.SetActive(false);
        cutsceneTriggered = false;
        PlayerController playerController = _gameController.PlayerController;
        playerController.SetStickyCamera(true);
        kitchenDoor.ChangeDoorState(true, false);
        _gameController.DisableAllKeys();
        StartCoroutine(playerController.CameraBlur(true));
        playerController.transform.LookAt(doctorEndPosition);
        playerController.animator.Play("FearBackwards");
        StartCoroutine(playerController.StepBack());
        
        _gameController.EnemyStaticSystem.MoveToPosition(doctorStartPosition.position, doctorEndPosition.position, doctorSpeed);
        yield return new WaitForSeconds(cutsceneTime);
        EndOfLevel();
    }

    public void EndOfLevel()
    {
        cutsceneTriggered = false;
        _gameController.PlayerController.SetStickyCamera(false);
        _gameController.EnableAllKeys();
        _gameController.PlayerController.animator.SetTrigger("DefaultState");
        _gameManager.playerPrefsManager.SaveGame(_level);
        _gameController.LevelsController.SetLevelActive(_level + 1);
    }
}