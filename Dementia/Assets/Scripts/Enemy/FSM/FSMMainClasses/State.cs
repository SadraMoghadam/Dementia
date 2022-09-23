using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/State")]
public sealed class State : BaseState
{
    public List<FSMAction> Actions = new List<FSMAction>();
    public List<Transition> Transitions = new List<Transition>(); 
    public override void Execute(BaseStateMachine machine)
    {
        foreach (var action in Actions)
        {
            action.Execute(machine);
        }

        foreach (var transition in Transitions)
        {
            transition.Execute(machine);
        }
    }
}
