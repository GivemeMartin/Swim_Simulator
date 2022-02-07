using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase
{
    protected StateMachineBase stateMachine;

    protected StateBase(StateMachineBase sm)
    {
        stateMachine = sm;
    }

    public virtual void OnUpdate() {}

    public abstract void EnterState();
    public abstract void ExitState();
    
}
