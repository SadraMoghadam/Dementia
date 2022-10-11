using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "FSM/Actions/Chase")]
public class ChaseAction : FSMAction
{
    [NonSerialized] private float timer = 0;
    [NonSerialized] private float waitTime = 0;

    public override void Execute(BaseStateMachine machine)
    {
        NavMeshAgent navMeshAgent = machine.GetComponent<NavMeshAgent>();
        MovingPoints movingPoints = machine.GetComponent<MovingPoints>();
        EnemySightSensor enemySightSensor = machine.GetComponent<EnemySightSensor>();
        var enemyAttackSensor = machine.GetComponent<EnemyAttackSensor>();
        EnemyUtility enemyUtility = EnemyUtility.Instance;
        Transform playerTransform = machine.PlayerController.transform;
        if (machine.isStartOfChase)
        {
            enemyUtility.SetEyeLights(true);
            navMeshAgent.SetDestination(playerTransform.position);
            machine.isStartOfChase = false;
            machine.isStartOfPatrol = true;
            machine.isStartOfAttack = true;
            machine.isStartOfAgony = true;
            machine.Stop();
            enemyUtility.SetAnimation(alert: true);
        }
        timer += Time.deltaTime;
        if (timer > machine.AlertTime)
        {
            if (movingPoints.HasReached(navMeshAgent, playerTransform))
            {
                enemyAttackSensor.StartAttack = true;
            }
            else
            {
                if (!enemySightSensor.Escaped(playerTransform, machine.transform, enemyUtility.viewRadius))
                {
                    waitTime = machine.WaitTime;
                    machine.Move(true);
                    navMeshAgent.SetDestination(playerTransform.position);   
                }
                else
                {
                    waitTime -= Time.deltaTime;
                    machine.Stop();
                }

                if (waitTime <= .1f)
                {
                    enemySightSensor.ChangeEscapedState(true);
                    machine.isStartOfChase = true;
                }
            }
        }
    }
}
