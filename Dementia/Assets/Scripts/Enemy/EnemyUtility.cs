using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public enum EnemyAnimatorParameters
{
    Idle,
    LookAround,
    Walk,
    SpeedWalk,
    AttackType1,
    AttackType2,
    Agony,
    Alert
}

public class EnemyUtility : MonoBehaviour
{
    [SerializeField] private GameObject eyeLights;
    public float viewRadius = 25;
    public float viewAngle = 150;
    public float waitTime = 25;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    private Animator EnemyAnimator;

    public static EnemyUtility Instance;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        EnemyAnimator = GetComponent<Animator>();
    }

    public void ChooseIdleAnimation()
    {
        int rnd = Random.Range(0, 100);
        if (rnd < 50)
        {
            SetAnimation(idle: true);
        }
        else
        {
            SetAnimation(lookAround: true);
        }
    }
    
    public void ChooseAttackAnimation()
    {
        int rnd = Random.Range(0, 100);
        if (rnd < 50)
        {
            SetAnimation(attackType1: true);
        }
        else
        {
            SetAnimation(attackType2: true);
        }
    }
    
    public  void SetAnimation(bool idle = false, bool lookAround = false, bool walk = false, bool speedWalk = false,
        bool attackType1 = false, bool attackType2 = false, bool agony = false, bool alert = false)
    {
        EnemyAnimator.SetBool(EnemyAnimatorParameters.Idle.ToString(), idle);
        EnemyAnimator.SetBool(EnemyAnimatorParameters.LookAround.ToString(), lookAround);
        EnemyAnimator.SetBool(EnemyAnimatorParameters.Walk.ToString(), walk);
        EnemyAnimator.SetBool(EnemyAnimatorParameters.SpeedWalk.ToString(), speedWalk);
        EnemyAnimator.SetBool(EnemyAnimatorParameters.AttackType1.ToString(), attackType1);
        EnemyAnimator.SetBool(EnemyAnimatorParameters.AttackType2.ToString(), attackType2);
        EnemyAnimator.SetBool(EnemyAnimatorParameters.Agony.ToString(), agony);
        EnemyAnimator.SetBool(EnemyAnimatorParameters.Alert.ToString(), alert);
    }

    public void SetEyeLights(bool active)
    {
        eyeLights.SetActive(active);
    }
    
}
