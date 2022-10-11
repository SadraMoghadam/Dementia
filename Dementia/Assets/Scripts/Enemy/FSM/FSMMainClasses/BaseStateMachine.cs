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
    [NonSerialized] public BaseState CurrentState;
    [NonSerialized] public NavMeshAgent NavMeshAgent;
    [NonSerialized] public MovingPoints MovingPoints;
    [NonSerialized] public float WaitTime = 10.15f;
    [NonSerialized] public float AttackCoolDown = 2.25f;
    [NonSerialized] public float AlertTime = 1.4f;
    [NonSerialized] public float AgonyTime = 5.84f;
    [NonSerialized] public bool isStartOfChase;
    [NonSerialized] public bool isStartOfPatrol;
    [NonSerialized] public bool isStartOfAttack;
    [NonSerialized] public bool isStartOfAgony; 
    private Dictionary<Type, Component> _cachedComponents;
    private int _updateCounter;

    private void Awake()
    {
        CurrentState = _initialState;
        _cachedComponents = new Dictionary<Type, Component>();
        _updateCounter = 0;
        NavMeshAgent = GetComponent<NavMeshAgent>();
        MovingPoints = GetComponent<MovingPoints>();
        AttackCoolDown /= 1.5f;
        AgonyTime /= 1.5f;
        isStartOfChase = true;
        isStartOfPatrol = true;
        isStartOfAttack = true;
        isStartOfAgony = true;
    }

    private void LateUpdate()
    {
        CurrentState.Execute(this);
        // _updateCounter++;
        // if (_updateCounter == 300)
        // {
        //     CurrentState.Execute(this);
        //     _updateCounter = 0;
        // }
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
        NavMeshAgent.speed = isRunning ? _runSpeed : _speed;
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
    
    public void Punch()
    {
        GameController.instance.DamageController.Damage(25);
    }
}
