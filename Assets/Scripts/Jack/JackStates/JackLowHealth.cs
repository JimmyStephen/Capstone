using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackLowHealth : HealthStateTemplate
{
    public JackLowHealth(CharacterTemplate owner, string name, State[] childStates) : base(owner, name, childStates) { }

    public FloatRef distance;
    public FloatRef trapsOffCD;
    public FloatRef timer;

    private const float closeMaxDuration = 2;
    private const float closeDistance = 3;
    float jumpTimer = 0;
    float minJumpTime = 3;
    float maxJumpTime = 8;

    public override void OnCreate()
    {
        distance = new FloatRef();
        trapsOffCD = new FloatRef();
        timer = new FloatRef();
        jumpTimer = Random.Range(minJumpTime, maxJumpTime);

        //to aggressive - Both Traps off cd
        sMachine.AddTransition(sMachine.StateFromName(typeof(JackPassive).Name), new Transition(new Condition[] { new FloatCondition(trapsOffCD, Condition.Predicate.GREATER, 0) }), sMachine.StateFromName(typeof(JackAggressive).Name));
        sMachine.AddTransition(sMachine.StateFromName(typeof(JackDefensive).Name), new Transition(new Condition[] { new FloatCondition(trapsOffCD, Condition.Predicate.GREATER, 0) }), sMachine.StateFromName(typeof(JackAggressive).Name));

        //to passive - Both traps on cd
        sMachine.AddTransition(sMachine.StateFromName(typeof(JackAggressive).Name), new Transition(new Condition[] { new FloatCondition(trapsOffCD, Condition.Predicate.EQUAL, 0) }), sMachine.StateFromName(typeof(JackPassive).Name));
        sMachine.AddTransition(sMachine.StateFromName(typeof(JackDefensive).Name),  new Transition(new Condition[] { new FloatCondition(trapsOffCD, Condition.Predicate.EQUAL, 0) }), sMachine.StateFromName(typeof(JackPassive).Name));

        //to defensive - close to target for to long
        sMachine.AddTransition(sMachine.StateFromName(typeof(JackAggressive).Name), new Transition(new Condition[] { new FloatCondition(timer, Condition.Predicate.GREATER, closeMaxDuration), new FloatCondition(distance, Condition.Predicate.GREATER, closeDistance) }), sMachine.StateFromName(typeof(JackDefensive).Name));
        sMachine.AddTransition(sMachine.StateFromName(typeof(JackPassive).Name),    new Transition(new Condition[] { new FloatCondition(timer, Condition.Predicate.GREATER, closeMaxDuration), new FloatCondition(distance, Condition.Predicate.GREATER, closeDistance) }), sMachine.StateFromName(typeof(JackDefensive).Name));

        //set default state
        sMachine.setState(sMachine.StateFromName(typeof(JackPassive).Name));
    }
    public override void OnEnter()
    {
        timer.value = 0;
        trapsOffCD.value = GetTrapsOffCD();
    }
    public override void OnExit()
    {
        //
    }
    public override void OnUpdate()
    {
        //distance update
        distance.value = Mathf.Abs(Owner.transform.position.x - Owner.opponent.transform.position.x);
        if(distance.value < closeDistance)
        {
            timer.value += Time.deltaTime;
        }
        else
        {
            timer.value = (timer.value < 0) ? 0 : timer.value - Time.deltaTime;
        }

        //ability update
        trapsOffCD.value = GetTrapsOffCD();
        jumpTimer -= Time.deltaTime;
        sMachine.Update();
    }

    public override bool ShouldJump()
    {
        //        throw new System.NotImplementedException();
        if (jumpTimer < 0)
        {
            jumpTimer = Random.Range(minJumpTime, maxJumpTime);
            //Debug.Log("Jump Time! Is Grounded: " + Owner.characterController.m_Grounded);
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

    private int GetTrapsOffCD()
    {
        int retVal = 0;
        if (Owner.currentAbilityOneCooldown <= 0) retVal++;
        if (Owner.currentAbilityTwoCooldown <= 0) retVal++;
        return retVal;
    }
}
