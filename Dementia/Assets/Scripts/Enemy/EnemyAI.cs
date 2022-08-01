using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private GameObject waypointsContainer;
    [SerializeField] private float walkSpeed = 4;
    [SerializeField] private float runSpeed = 6;
    [SerializeField] private float viewRadius = 15;
    [SerializeField] private float viewAngle = 90;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask obstacleMask;
    private Animator _enemyAnimator;
    private List<Transform> waypoints;
    private NavMeshAgent _agent;
    private int _waypointIndex;
    private Vector3 _playerLastPosition = Vector3.zero;
    private Vector3 _playerPosition;
    private Vector3 target;
    private bool choosingDestination;
    private bool _isPatrol;
    private bool _playerInViewRange;
    private bool _playerNear;
    private bool _playerCaught;
    private float _timeToRotate = 2;
    private float _waitTime = 5.15f;
    private bool _chaseFinished;

    private enum EnemyAnimatorParameters
    {
        Idle,
        IdleProb,
        Walk,
        Run,
        Punch,
        Turn
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _enemyAnimator = GetComponent<Animator>();
        _isPatrol = true;
        _playerInViewRange = false;
        _playerNear = false;
        _playerCaught = false;
        _chaseFinished = false;
        _agent.speed = walkSpeed;
        waypoints = waypointsContainer.GetComponentsInChildren<Transform>().ToList();
        waypoints.RemoveAt(0);
        choosingDestination = true;
        StartCoroutine(UpdateDestination());
    }

    private void Update()
    {
        EnviromentView();
        
        if (!_isPatrol)
        {
            Chase();
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if (_chaseFinished || Vector3.Distance(transform.position, target) < 2.5f && !choosingDestination)
        {
            _chaseFinished = false;
            StartCoroutine(UpdateDestination());
        }   
    }

    private void Chase()
    {
        _isPatrol = false;
        _playerNear = false;
        _playerLastPosition = Vector3.zero;
 
        if (!_playerCaught)
        {
            Move(runSpeed);
            _agent.SetDestination(_playerPosition);
        }
        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            if (_waitTime <= 0 && !_playerCaught && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
            {
                _isPatrol = true;
                _playerNear = false;
                Move(walkSpeed);
            }
            else
            {
                if (Vector3.Distance(transform.position,
                        GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                {
                    Stop();
                    _chaseFinished = true;
                }
                _waitTime -= Time.deltaTime;
            }
        }
    }
    
    void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);
 
        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    _playerInViewRange = true;
                    _isPatrol = false;
                }
                else
                {
                    _playerInViewRange = false;
                }
            }
            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {
                _playerInViewRange = false;
            }
            if (_playerInViewRange)
            {
                _playerPosition = player.transform.position;
            }
        }
    }
    
    private IEnumerator UpdateDestination()
    {
        ChooseIdleAnimation();
        choosingDestination = true;
        int newIndex = Random.Range(0, waypoints.Count);
        if (_waypointIndex == newIndex)
        {
            if (newIndex == waypoints.Count - 1)
                _waypointIndex--;
            else
                _waypointIndex++;
        }
        else
        {
            _waypointIndex = newIndex;
        }
        yield return new WaitForSeconds(_waitTime);
        _enemyAnimator.SetBool(EnemyAnimatorParameters.Walk.ToString(), true);
        Debug.Log(_waypointIndex);
        target = waypoints[_waypointIndex].position;
        // Vector3 direction = target - transform.position;
        // transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), .1f);
        Debug.Log(target);
        _agent.SetDestination(target);
        yield return new WaitForSeconds(_timeToRotate);
        choosingDestination = false;
    }

    private void ChooseIdleAnimation()
    {
        float rnd = Random.Range(0, 100);
        _enemyAnimator.SetBool(EnemyAnimatorParameters.Walk.ToString(), false);
        _enemyAnimator.SetBool(EnemyAnimatorParameters.Idle.ToString(), true);
        _enemyAnimator.SetFloat(EnemyAnimatorParameters.IdleProb.ToString(), rnd);
    }
    
    void Stop()
    {
        _agent.isStopped = true;
        ChooseIdleAnimation();
        _agent.speed = 0;
    }
 
    void Move(float speed)
    {
        _agent.isStopped = false;
        _agent.speed = speed;
        if (_playerInViewRange || _playerNear)
        {
            _enemyAnimator.SetBool(EnemyAnimatorParameters.Run.ToString(), true);
            _enemyAnimator.SetBool(EnemyAnimatorParameters.Walk.ToString(), false);
            _enemyAnimator.SetBool(EnemyAnimatorParameters.Idle.ToString(), false);
        }
        else
        {
            _enemyAnimator.SetBool(EnemyAnimatorParameters.Walk.ToString(), true);
            _enemyAnimator.SetBool(EnemyAnimatorParameters.Run.ToString(), false);
            _enemyAnimator.SetBool(EnemyAnimatorParameters.Idle.ToString(), false);
        }
    }
 
    void CaughtPlayer()
    {
        _playerCaught = true;
    }
    
}
