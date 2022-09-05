using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour
{
    public bool IsLocked = false;
    public int KeyId = 0;
    [HideInInspector] public bool IsOpen;
    private Animator animator;
    private NavMeshObstacle _navMeshObstacle;
    private float _timer;
    private float _timeToGetClosed = 50f;
    private GameController _gameController;

    private void Awake()
    {
        animator = transform.parent.GetComponent<Animator>();
        _navMeshObstacle = GetComponent<NavMeshObstacle>();
    }

    private void Start()
    {
        _gameController = GameController.instance;
        List<int> keysIds = _gameController.Inventory.GetKeysIds();
        if (KeyId < keysIds.Count && IsLocked)
        {
            IsLocked = false;
        }
    }

    private void LateUpdate()
    {
        if (IsOpen)
        {
            _timer += Time.fixedDeltaTime;
            if (_timer > _timeToGetClosed)
            {
                ChangeDoorState(false);
                _timer = 0;
            }
        }

        if (IsLocked)
        {
            List<int> keysIds = _gameController.Inventory.GetKeysIds();
            if (KeyId < keysIds.Count)
            {
                IsLocked = false;
            }   
        }
    }

    public void ChangeDoorState()
    {
        try
        {
            if (IsLocked)
            {
                _gameController.QuestAndHintController.ShowHint(0);
                return;
            }
            if (!IsOpen)
            {
                animator.SetBool("Open", true);
                IsOpen = true;
                StartCoroutine(NavMeshObstacleCarving(true));
            }
            else
            {
                animator.SetBool("Open", false);
                IsOpen = false;
                StartCoroutine(NavMeshObstacleCarving(false));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Door");
        }
    }
    
    public void ChangeDoorState(bool open)
    {
        if (IsLocked)
        {
            _gameController.QuestAndHintController.ShowHint(0);
            return;
        }
        animator.SetBool("Open", open);
        IsOpen = open;
        StartCoroutine(NavMeshObstacleCarving(open));
    }

    private IEnumerator NavMeshObstacleCarving(bool carve)
    {
        yield return new WaitForSeconds(2f);
        _navMeshObstacle.carving = carve;
        StopCoroutine(NavMeshObstacleCarving(carve));
    }

}
