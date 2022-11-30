using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RasputinLowHealth : HealthStateTemplate
{
    public RasputinLowHealth(CharacterTemplate owner, string name, State[] childStates) : base(owner, name, childStates) { }

    BoolRef abilityTwoOnCD;

    //Jump Time Stuff
    float jumpTimer = 0;
    readonly float minJumpTime = 4;
    readonly float maxJumpTime = 8;

    public override void OnCreate()
    {
        abilityTwoOnCD = new BoolRef();

        //States
        //Aggressive - Ability Two Off CD
        //Passive - Nope
        //Defensive - Ability Two On CD

        //to aggressive
        sMachine.AddTransition(sMachine.StateFromName(typeof(RasputinDefensive).Name), new Transition(new Condition[] { new BoolCondition(abilityTwoOnCD, false) }), sMachine.StateFromName(typeof(RasputinAggressive).Name));

        //to defensive
        sMachine.AddTransition(sMachine.StateFromName(typeof(RasputinAggressive).Name), new Transition(new Condition[] { new BoolCondition(abilityTwoOnCD, true) }), sMachine.StateFromName(typeof(RasputinDefensive).Name));
        
        //to passive

        //set inital state (Defensive)
        sMachine.setState(sMachine.StateFromName(typeof(RasputinDefensive).Name));
    }
    public override void OnEnter()
    {
        //set variables
        jumpTimer = 0;
        abilityTwoOnCD.value = Owner.currentAbilityTwoCooldown > 0;
    }
    public override void OnExit()
    {
        //throw new System.NotImplementedException();
    }
    public override void OnUpdate()
    {
        jumpTimer -= Time.deltaTime;
        //throw new System.NotImplementedException();
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
