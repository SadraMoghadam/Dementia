using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackSensor : MonoBehaviour
{
    private bool _isAttackCompleted;
    private bool _startAttack;

    public void Awake()
    {
        _isAttackCompleted = false;
        _startAttack = false;
    }

    public bool IsAttackCompleted
    {
        get => _isAttackCompleted;
        set => _isAttackCompleted = value;
    }

    public bool StartAttack
    {
        get => _startAttack;
        set => _startAttack = value;
    }
}
