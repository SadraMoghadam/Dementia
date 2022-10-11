using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Decisions/Attack")]
public class AttackDecision : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        var enemyAttackSensor = stateMachine.GetComponent<EnemyAttackSensor>();
        return enemyAttackSensor.StartAttack;
    }
}
