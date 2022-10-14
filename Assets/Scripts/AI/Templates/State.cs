using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public CharacterTemplate owner { get; private set; }
    public string name { get; private set; }

    public State(CharacterTemplate owner, string name)
    {
        this.owner = owner;
        this.name = name;
    }

    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();

    public abstract float StateMovement();
    public abstract bool shouldJump();
    public abstract bool useBasicAbility();
    public abstract bool useAbilityOne();
    public abstract bool useAbilityTwo();
    public abstract bool useAbilityThree();
}
