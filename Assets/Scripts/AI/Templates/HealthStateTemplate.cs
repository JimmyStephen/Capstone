using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthStateTemplate : State
{
    public HealthStateTemplate(CharacterTemplate owner, string name, State[] childStates) : base(owner, name) 
    {
        sMachine = new StateMachine();
        foreach(var s in childStates)
        {
            sMachine.AddState(s);
        }
    }
    public StateMachine sMachine;

    /// <summary>
    /// What you need to do when this state is first created
    /// </summary>
    public abstract void OnCreate();
}
