using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolAction : FSMAction
{
    private float timer = 0;
    public override void Execute(BaseStateMachine machine)
    {
        NavMeshAgent navMeshAgent = machine.GetComponent<NavMeshAgent>();
        PatrolPoints patrolPoints = machine.GetComponent<PatrolPoints>();
        Animator animator = machine.GetComponent<Animator>();

        if (patrolPoints.HasReached(navMeshAgent))
        {
            navMeshAgent.isStopped = true;
            timer += Time.deltaTime;
            if (timer > machine.WaitTime)
            {
                navMeshAgent.SetDestination(patrolPoints.GetNext().position);
                timer = 0;
            }
        }
    }
}
