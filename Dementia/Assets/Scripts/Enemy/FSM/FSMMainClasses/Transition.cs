using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "FSM/Transition")]
public class Transition : ScriptableObject
{
    public Decision Decision;
    public BaseState TrueState;
    public BaseState FalseState;
    
    public void Execute(BaseStateMachine machine)
    {
        if (Decision.Decide(machine) && !(TrueState is RemainInState))
        {
            machine.CurrentState = TrueState;
        }
        else if (!(FalseState is RemainInState))
        {
            machine.CurrentState = FalseState;
        }
    }
}
