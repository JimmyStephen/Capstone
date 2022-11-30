using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RasputinMediumHealth : HealthStateTemplate
{
    public RasputinMediumHealth(CharacterTemplate owner, string name, State[] childStates) : base(owner, name, childStates) { }

    readonly float closeRangeDistance = 4;
    FloatRef currentDistance;
    BoolRef abilityTwoOnCD;

    //Jump Time Stuff
    float jumpTimer = 0;
    readonly float minJumpTime = 4;
    readonly float maxJumpTime = 8;

    public override void OnCreate()
    {
        currentDistance = new FloatRef();
        abilityTwoOnCD = new BoolRef();

        //States
        //Aggressive - Close Range
        //Passive - Far Away
        //Defensive - Ability Two On CD

        //to aggressive
        sMachine.AddTransition(sMachine.StateFromName(typeof(RasputinPassive).Name),    new Transition(new Condition[] { new FloatCondition(currentDistance, Condition.Predicate.LESS_EQUAL, closeRangeDistance) }), sMachine.StateFromName(typeof(RasputinAggressive).Name));
        sMachine.AddTransition(sMachine.StateFromName(typeof(RasputinDefensive).Name),  new Transition(new Condition[] { new FloatCondition(currentDistance, Condition.Predicate.LESS_EQUAL, closeRangeDistance), new BoolCondition(abilityTwoOnCD, false) }), sMachine.StateFromName(typeof(RasputinAggressive).Name));

        //to defensive
        sMachine.AddTransition(sMachine.StateFromName(typeof(RasputinAggressive).Name), new Transition(new Condition[] { new BoolCondition(abilityTwoOnCD, true) }), sMachine.StateFromName(typeof(RasputinDefensive).Name));
        sMachine.AddTransition(sMachine.StateFromName(typeof(RasputinPassive).Name),    new Transition(new Condition[] { new BoolCondition(abilityTwoOnCD, true) }), sMachine.StateFromName(typeof(RasputinDefensive).Name));

        //to passive
        sMachine.AddTransition(sMachine.StateFromName(typeof(RasputinAggressive).Name), new Transition(new Condition[] { new FloatCondition(currentDistance, Condition.Predicate.GREATER, closeRangeDistance) }), sMachine.StateFromName(typeof(RasputinPassive).Name));
        sMachine.AddTransition(sMachine.StateFromName(typeof(RasputinDefensive).Name),  new Transition(new Condition[] { new FloatCondition(currentDistance, Condition.Predicate.GREATER, closeRangeDistance), new BoolCondition(abilityTwoOnCD, false) }), sMachine.StateFromName(typeof(RasputinPassive).Name));

        //set inital state (Passive)
        sMachine.setState(sMachine.StateFromName(typeof(RasputinPassive).Name));
    }
    public override void OnEnter()
    {
        //set variables
        jumpTimer = 0;
        abilityTwoOnCD.value = Owner.currentAbilityTwoCooldown > 0;
        currentDistance.value = Mathf.Abs(Owner.transform.position.x - Owner.opponent.transform.position.x);
    }
    public override void OnExit()
    {
        //throw new System.NotImplementedException();
    }
    public override void OnUpdate()
    {
        currentDistance.value = Mathf.Abs(Owner.transform.position.x - Owner.opponent.transform.position.x);
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
