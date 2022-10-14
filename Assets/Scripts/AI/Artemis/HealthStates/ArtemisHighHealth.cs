using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtemisHighHealth : HealthStateTemplate
{
    public ArtemisHighHealth(CharacterTemplate owner, string name, State[] childStates) : base(owner, name, childStates) { }

    public override void OnCreate()
    {
        throw new System.NotImplementedException();
    }

    public override void OnEnter()
    {
//        throw new System.NotImplementedException();
    }

    public override void OnExit()
    {
//        throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        //check what state to be
        //throw new System.NotImplementedException();
    }

    public override bool shouldJump()
    {
        throw new System.NotImplementedException();
    }
    public override float StateMovement()
    {
        return sMachine.currentState.StateMovement();
    }
    public override bool useBasicAbility()
    {
        return sMachine.currentState.useBasicAbility();
    }
    public override bool useAbilityOne()
    {
        return sMachine.currentState.useAbilityOne();
    }
    public override bool useAbilityTwo()
    {
        return sMachine.currentState.useAbilityTwo();
    }
    public override bool useAbilityThree()
    {
        return sMachine.currentState.useAbilityThree();
    }
}
