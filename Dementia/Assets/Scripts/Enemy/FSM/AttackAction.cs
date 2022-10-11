using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "FSM/Actions/Attack")]
public class AttackAction : FSMAction
{
    [NonSerialized] private float timer = 0;
    
    public override void Execute(BaseStateMachine machine)
    {
        var enemyAttackSensor = machine.GetComponent<EnemyAttackSensor>();
        EnemyUtility enemyUtility = EnemyUtility.Instance;
        
        if (machine.isStartOfAttack)
        {
            timer = machine.AttackCoolDown + machine.AgonyTime;
            enemyUtility.ChooseAttackAnimation();
            // machine.isStartOfAttack = false;
            enemyAttackSensor.IsAttackCompleted = false;
            machine.isStartOfPatrol = true;
            machine.isStartOfChase = true;
            machine.isStartOfAttack = false;
        }

        if (timer <= machine.AgonyTime && machine.isStartOfAgony)
        {
            enemyUtility.SetAnimation(agony: true);
            machine.isStartOfAgony = false;
        }

        if (timer <= 0)
        {
            // machine.isStartOfAttack = true;
            // machine.isStartOfAgony = true;
            enemyAttackSensor.IsAttackCompleted = true;
            enemyAttackSensor.StartAttack = false;
        }
        timer -= Time.deltaTime;
    }
}
