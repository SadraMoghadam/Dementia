using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


public class BaseStateMachine : MonoBehaviour
{
    public PlayerController PlayerController;
    [SerializeField] private BaseState _initialState;
    [SerializeField] private float _speed;
    [SerializeField] private float _runSpeed;
    [HideInInspector] public BaseState CurrentState;
    [HideInInspector] public NavMeshAgent NavMeshAgent;
    [HideInInspector] public MovingPoints MovingPoints;
    [HideInInspector] public float WaitTime = 10.15f;
    [HideInInspector] public float AttackCoolDown = 5.08f;
    private Dictionary<Type, Component> _cachedComponents;

    private void Awake()
    {
        CurrentState = _initialState;
        _cachedComponents = new Dictionary<Type, Component>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
        MovingPoints = GetComponent<MovingPoints>();
    }

    private void Update()
    {
        CurrentState.Execute(this);
    }
    
    public new T GetComponent<T>() where T : Component
    {
        if(_cachedComponents.ContainsKey(typeof(T)))
            return _cachedComponents[typeof(T)] as T;

        var component = base.GetComponent<T>();
        if(component != null)
        {
            _cachedComponents.Add(typeof(T), component);
        }
        return component;
    }
    
    public void Stop(bool chooseIdleAnimation = true)
    {
        NavMeshAgent.isStopped = true;
        NavMeshAgent.speed = 0;
        if(chooseIdleAnimation)
            EnemyUtility.Instance.ChooseIdleAnimation();
        else
        {
            EnemyUtility.Instance.SetAnimation(lookAround: true);
        }
    }
    
    public void Move(bool isRunning = false)
    {
        NavMeshAgent.isStopped = false;
        NavMeshAgent.speed = _speed;
        if (isRunning)
        {
            EnemyUtility.Instance.SetAnimation(speedWalk:true);
            
        }
        else
        {
            EnemyUtility.Instance.SetAnimation(walk:true);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MutantInteractableArea"))
        {
            other.transform.parent.GetComponent<Door>().ChangeDoorState(true, true);
        }
    }
    
}
