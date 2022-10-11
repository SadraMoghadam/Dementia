using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "FSM/Decisions/AttackCompletedDecision")]
public class AttackCompletedDecision : Decision
{
    public override bool Decide(BaseStateMachine stateMachine)
    {
        var enemyAttackSensor = stateMachine.GetComponent<EnemyAttackSensor>();
        return enemyAttackSensor.IsAttackCompleted;
    }
}
