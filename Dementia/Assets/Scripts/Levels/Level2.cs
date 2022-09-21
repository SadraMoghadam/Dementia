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
        _gameController.PlayerController.SetStickyCamera(true);
        kitchenDoor.ChangeDoorState(true, false);
        _gameController.DisableAllKeys();
        _gameController.PlayerController.transform.LookAt((doctorStartPosition.position + doctorEndPosition.position) / 2);
        _gameController.PlayerController.animator.Play("FearBackwards");
        
        _gameController.EnemyStaticSystem.MoveToPosition(doctorStartPosition.position, doctorEndPosition.position, 2);
        yield return new WaitForSeconds(10f);
    }

    public void EndOfLevel()
    {
        cutsceneTriggered = false;
        _gameController.EnableAllKeys();
        _gameController.PlayerController.animator.Play("BaseState");
        _gameManager.playerPrefsManager.SaveGame(_level);
        _gameController.LevelsController.SetLevelActive(_level + 1);
    }
}