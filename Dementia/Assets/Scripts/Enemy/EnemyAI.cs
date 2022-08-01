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
    private Animator _enemyAnimator;
    private List<Transform> waypoints;
    private NavMeshAgent _agent;
    private int _waypointIndex;
    private Vector3 target;
    private bool choosingDestination;

    private enum EnemyAnimatorParameters
    {
        Idle,
        Walk,
        Run,
        Punch,
        Turn
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _enemyAnimator = GetComponent<Animator>();
        waypoints = waypointsContainer.GetComponentsInChildren<Transform>().ToList();
        waypoints.RemoveAt(0);
        choosingDestination = true;
        StartCoroutine(UpdateDestination());
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, target) < 2.5f && !choosingDestination)
        {
            StartCoroutine(UpdateDestination());
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
        yield return new WaitForSeconds(5.5f);
        _enemyAnimator.SetBool(EnemyAnimatorParameters.Walk.ToString(), true);
        Debug.Log(_waypointIndex);
        target = waypoints[_waypointIndex].position;
        // Vector3 direction = target - transform.position;
        // transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), .1f);
        Debug.Log(target);
        _agent.SetDestination(target);
        yield return new WaitForSeconds(2);
        choosingDestination = false;
        // yield return null;
    }

    private void ChooseIdleAnimation()
    {
        float rnd = Random.Range(0, 100);
        _enemyAnimator.SetBool(EnemyAnimatorParameters.Walk.ToString(), false);
        _enemyAnimator.SetFloat(EnemyAnimatorParameters.Idle.ToString(), rnd);
    }
    
}
