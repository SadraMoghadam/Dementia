using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStateMachine : MonoBehaviour
{
    [SerializeField] private BaseState _initialState;
    [HideInInspector] public BaseState CurrentState;
    [HideInInspector] public float WaitTime = 10.15f;
    [HideInInspector] public float AttackCoolDown = 5.08f;
    private Dictionary<Type, Component> _cachedComponents;

    private void Awake()
    {
        CurrentState = _initialState;
        _cachedComponents = new Dictionary<Type, Component>();
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
    
}
