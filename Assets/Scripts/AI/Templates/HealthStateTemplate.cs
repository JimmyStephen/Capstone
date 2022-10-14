using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthStateTemplate : State
{
    public HealthStateTemplate(CharacterTemplate owner, string name, State[] childStates) : base(owner, name) 
    {
        foreach(var s in childStates)
        {
            sMachine.AddState(s);
        }
    }
    public StateMachine sMachine = new StateMachine();

    public abstract void OnCreate();
}
