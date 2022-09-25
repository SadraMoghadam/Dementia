using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "FSM/Actions/Patrol")]
public class PatrolAction : FSMAction
{
    [NonSerialized] private float timer = 0;
    [NonSerialized] private bool isStartOfPatrol = true;
    [NonSerialized] private bool stopAnimationChoose = false;


    public override void Execute(BaseStateMachine machine)
    {
        NavMeshAgent navMeshAgent = machine.GetComponent<NavMeshAgent>();
        MovingPoints movingPoints = machine.GetComponent<MovingPoints>();

        if (isStartOfPatrol)
        {
            EnemyUtility.Instance.SetEyeLights(false);
            navMeshAgent.SetDestination(movingPoints.GetNext(navMeshAgent).position);
            isStartOfPatrol = false;
        }

        timer += Time.deltaTime;
        if (movingPoints.HasReached(navMeshAgent) && timer < machine.WaitTime)
        {
            if(!stopAnimationChoose)
            {
                machine.Stop();
                stopAnimationChoose = true;
            }
        }
        else
        {
            navMeshAgent.SetDestination(movingPoints.GetNext(navMeshAgent).position);
            machine.Move();
            timer = 0;
            stopAnimationChoose = false;
        }
    }
    
}
