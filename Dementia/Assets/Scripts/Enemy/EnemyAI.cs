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
    [SerializeField] private float runSpeed = 7;
    [SerializeField] private float viewRadius = 15;
    [SerializeField] private float viewAngle = 90;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private GameObject eyeLights;
    [SerializeField] private GameObject punchArea;
    [SerializeField] private float waitTime = 5.15f;
    private Animator _enemyAnimator;
    private List<Transform> waypoints;
    private NavMeshAgent _agent;
    private int _waypointIndex;
    private Vector3 _playerPosition;
    private Vector3 target;
    private bool choosingDestination;
    private bool _isPatrol;
    private bool _playerInViewRange;
    private bool _playerNear;
    private bool _playerCaught;
    private float _timeToRotate = 2;
    private float _waitTime;
    private bool _chaseFinished;
    private bool _startOfChase;
    private Transform player;

    private enum EnemyAnimatorParameters
    {
        Idle,
        IdleProb,
        Walk,
        Run,
        Punch,
        Turn,
        Agony
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
        _startOfChase = true;
        _agent.speed = walkSpeed;
        _waitTime = waitTime;
        eyeLights.SetActive(false);
        punchArea.SetActive(false);
        waypoints = waypointsContainer.GetComponentsInChildren<Transform>().ToList();
        waypoints.RemoveAt(0);
        choosingDestination = true;
        StartCoroutine(UpdateDestination());
    }

    private void Update()
    {
        if (GameController.instance.DamageController.isPlayerDead)
        {
            _enemyAnimator.SetBool(EnemyAnimatorParameters.Punch.ToString(), false);
            Stop();
            return;   
        }
        
        EnviromentView();
        
        if (_isPatrol)
        {
            _startOfChase = true;
            Patrol();
        }
        else
        {
            Chase();
        }
    }


    private void Patrol()
    {
        if (_chaseFinished || Vector3.Distance(transform.position, target) < _agent.stoppingDistance && !choosingDestination)
        {
            _chaseFinished = false;
            StartCoroutine(UpdateDestination());
        }   
    }

    private void Chase()
    {
        StartCoroutine(ChaseProcess());
    }
    
    private IEnumerator ChaseProcess()
    {
        if (_startOfChase)
        {
            punchArea.SetActive(true);
            eyeLights.SetActive(true);
            _enemyAnimator.SetBool(EnemyAnimatorParameters.Agony.ToString(), true);
            _agent.isStopped = true;
            yield return new WaitForSeconds(3.35f);
            _enemyAnimator.SetBool(EnemyAnimatorParameters.Agony.ToString(), false);
            _startOfChase = false;
        }
        _enemyAnimator.SetBool(EnemyAnimatorParameters.Punch.ToString(), false);
        _isPatrol = false;
        _playerNear = false;
 
        if (!_playerCaught)
        {
            Move(runSpeed);
            _agent.SetDestination(_playerPosition);
        }
        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            if (Vector3.Distance(transform.position, player.position) < _agent.stoppingDistance + 3)
            {
                punchArea.SetActive(true);
                transform.rotation = Quaternion.LookRotation(player.position - transform.position);
                _enemyAnimator.SetBool(EnemyAnimatorParameters.Punch.ToString(), true);
            }
            else if (_waitTime <= 0 && !_playerCaught && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
            {
                _isPatrol = true;
                _playerNear = false;
                _waitTime = waitTime;
                // Move(walkSpeed);
                eyeLights.SetActive(false);
            }
            else
            {
                if (Vector3.Distance(transform.position,
                        GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                {
                    Stop();
                    _chaseFinished = true;
                    punchArea.SetActive(false);
                }
                _waitTime -= Time.deltaTime;
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
        Debug.Log(target);
        Move(walkSpeed);
        _agent.SetDestination(target);
        yield return new WaitForSeconds(_timeToRotate);
        choosingDestination = false;
    }

    void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);
 
        for (int i = 0; i < playerInRange.Length; i++)
        {
            player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float dstToPlayer = Vector3.Distance(transform.position, player.position);
            if (dstToPlayer < 2)
            {
                _playerInViewRange = true;
                _isPatrol = false;
            }
            else if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
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

    private void ChooseIdleAnimation()
    {
        float rnd = Random.Range(0, 100);
        _enemyAnimator.SetBool(EnemyAnimatorParameters.Walk.ToString(), false);
        _enemyAnimator.SetBool(EnemyAnimatorParameters.Run.ToString(), false);
        _enemyAnimator.SetBool(EnemyAnimatorParameters.Agony.ToString(), false);
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
        if (_playerInViewRange || _playerNear || speed == runSpeed)
        {
            _enemyAnimator.SetBool(EnemyAnimatorParameters.Run.ToString(), true);
            _enemyAnimator.SetBool(EnemyAnimatorParameters.Walk.ToString(), false);
            _enemyAnimator.SetBool(EnemyAnimatorParameters.Idle.ToString(), false);
            _enemyAnimator.SetBool(EnemyAnimatorParameters.Agony.ToString(), false);
            
        }
        else
        {
            _enemyAnimator.SetBool(EnemyAnimatorParameters.Walk.ToString(), true);
            _enemyAnimator.SetBool(EnemyAnimatorParameters.Run.ToString(), false);
            _enemyAnimator.SetBool(EnemyAnimatorParameters.Idle.ToString(), false);
            _enemyAnimator.SetBool(EnemyAnimatorParameters.Agony.ToString(), false);
        }
    }
 
    void CaughtPlayer()
    {
        _playerCaught = true;
    }
    
}
