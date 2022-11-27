using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RasputinHighHealth : HealthStateTemplate
{
    public RasputinHighHealth(CharacterTemplate owner, string name, State[] childStates) : base(owner, name, childStates) { }


    public override void OnCreate()
    {
        throw new System.NotImplementedException();
    }
    public override void OnEnter()
    {
        throw new System.NotImplementedException();
    }
    public override void OnExit()
    {
        throw new System.NotImplementedException();
    }
    public override void OnUpdate()
    {
        throw new System.NotImplementedException();
    }

    public override bool ShouldJump()
    {
        throw new System.NotImplementedException();
    }
    public override float StateMovement()
    {
        return sMachine.currentState.StateMovement();
    }

    public override int UseAbility()
    {
        return sMachine.currentState.UseAbility();
    }
    public override bool UseBasicAbility()
    {
        return sMachine.currentState.UseBasicAbility();
    }
    public override bool UseAbilityOne()
    {
        return sMachine.currentState.UseAbilityOne();
    }
    public override bool UseAbilityTwo()
    {
        return sMachine.currentState.UseAbilityTwo();
    }
    public override bool UseAbilityThree()
    {
        return sMachine.currentState.UseAbilityThree();
    }
}
