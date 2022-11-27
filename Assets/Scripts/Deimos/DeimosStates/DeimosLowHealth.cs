using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeimosLowHealth : HealthStateTemplate
{
    public DeimosLowHealth(CharacterTemplate owner, string name, State[] childStates) : base(owner, name, childStates) { }

    //Jump Time Stuff
    float jumpTimer = 0;
    readonly float minJumpTime = 4;
    readonly float maxJumpTime = 8;

    public override void OnCreate()
    {
        //initalize variables

        //initalize states

        //set inital state (Passive)

        throw new System.NotImplementedException();
    }
    public override void OnEnter()
    {
        //set timers
        throw new System.NotImplementedException();
    }
    public override void OnExit()
    {
        //
    }
    public override void OnUpdate()
    {
        //update timers

        //update variables
        throw new System.NotImplementedException();
        jumpTimer -= Time.deltaTime;
    }

    public override bool ShouldJump()
    {
        if (jumpTimer < 0)
        {
            jumpTimer = Random.Range(minJumpTime, maxJumpTime);
            return true;
        }
        return false;
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
