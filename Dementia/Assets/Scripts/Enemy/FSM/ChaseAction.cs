using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "FSM/Actions/Chase")]
public class ChaseAction : FSMAction
{
    [NonSerialized] private bool isStartOfChase = true;

    public override void Execute(BaseStateMachine machine)
    {
        NavMeshAgent navMeshAgent = machine.GetComponent<NavMeshAgent>();
        MovingPoints movingPoints = machine.GetComponent<MovingPoints>();
        Transform playerTransform = machine.PlayerController.transform;
        if (isStartOfChase)
        {
            EnemyUtility.Instance.SetEyeLights(true);
            navMeshAgent.SetDestination(playerTransform.position);
            isStartOfChase = false;
        }
        if (movingPoints.HasReached(navMeshAgent))
        {
            machine.Stop();
        }
        else
        {
            navMeshAgent.SetDestination(playerTransform.position);
            machine.Move(true);
        }
    }
}
