using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageController : MonoBehaviour
{
    [HideInInspector] public bool isPlayerDead;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private Image damageBackground;
    [SerializeField] private float regenerationDelay = 5;
    private Animator _animator;
    private float _health;
    private float _healingTimeOut = 6;
    private float _damageStartTime;
    private int _counter = 0;
    private CapsuleCollider _collider;
    private UIController _uiController;
    

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<CapsuleCollider>();
        _uiController = UIController.instance;
        _health = maxHealth;
        isPlayerDead = false;
    }


    // private void Update()
    // {
    //     if (_health >= maxHealth)
    //     {
    //         return;
    //     }
    //     _damageStartTime += Time.deltaTime;
    //     _counter++;
    //     if (_damageStartTime >= _healingTimeOut && _counter >= regenerationDelay)
    //     {
    //         _counter = 0;
    //         _health = _health < maxHealth ? _health + 5 : maxHealth;
    //         Color tempColor = damageBackground.color;
    //         tempColor.a = 1 - (float)_health / maxHealth;
    //         damageBackground.color = tempColor;
    //         Debug.Log(_health);
    //     }
    // }

    public void Damage(float damageAmount)
    {
        _damageStartTime = 0;
        _health -= damageAmount;
        Color tempColor = damageBackground.color;
        tempColor.a = 1 - (float)_health / maxHealth;
        damageBackground.color = tempColor;
        if (_health <= 0)
        {
            _health = 0;
            isPlayerDead = true;
            _animator.SetBool("Dead", true);
            _collider.radius = .1f;
            _collider.height = .1f;
            Debug.Log("You Lost");
            _uiController.ShowDiedPanel();
        }
        Debug.Log(_health);
    }

    public void Heal(float amount)
    {
        _health += amount;
        if (_health >= maxHealth)
        {
            _health = maxHealth;
        }
        Color tempColor = damageBackground.color;
        tempColor.a = 1 - (float)_health / maxHealth;
        damageBackground.color = tempColor;
    }

    public float GetHealth()
    {
        return _health;
    }
    
}
