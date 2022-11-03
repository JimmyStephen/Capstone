using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackHighHealth : HealthStateTemplate
{
    public JackHighHealth(CharacterTemplate owner, string name, State[] childStates) : base(owner, name, childStates) { }

    public FloatRef distance;
    public FloatRef trapsOffCD;
    public FloatRef timer;

    private const float closeMaxDuration = 5;
    private const float closeDistance = 3;

    public override void OnCreate()
    {
        distance = new FloatRef();
        trapsOffCD = new FloatRef();
        timer = new FloatRef();

        //to aggressive - Both Traps off cd
        sMachine.AddTransition(sMachine.StateFromName(typeof(JackPassive).Name), new Transition(new Condition[] { new FloatCondition(trapsOffCD, Condition.Predicate.GREATER, 0) }), sMachine.StateFromName(typeof(JackAggressive).Name));
        sMachine.AddTransition(sMachine.StateFromName(typeof(JackDefensive).Name), new Transition(new Condition[] { new FloatCondition(trapsOffCD, Condition.Predicate.GREATER, 0) }), sMachine.StateFromName(typeof(JackAggressive).Name));

        //to passive - Both traps on cd
        sMachine.AddTransition(sMachine.StateFromName(typeof(JackAggressive).Name), new Transition(new Condition[] { new FloatCondition(trapsOffCD, Condition.Predicate.EQUAL, 0) }), sMachine.StateFromName(typeof(JackPassive).Name));
        sMachine.AddTransition(sMachine.StateFromName(typeof(JackDefensive).Name), new Transition(new Condition[] { new FloatCondition(trapsOffCD, Condition.Predicate.EQUAL, 0) }), sMachine.StateFromName(typeof(JackPassive).Name));

        //to defensive - close to target for to long
        sMachine.AddTransition(sMachine.StateFromName(typeof(JackAggressive).Name), new Transition(new Condition[] { new FloatCondition(timer, Condition.Predicate.GREATER, closeMaxDuration), new FloatCondition(distance, Condition.Predicate.GREATER, closeDistance) }), sMachine.StateFromName(typeof(JackDefensive).Name));
        sMachine.AddTransition(sMachine.StateFromName(typeof(JackPassive).Name), new Transition(new Condition[] { new FloatCondition(timer, Condition.Predicate.GREATER, closeMaxDuration), new FloatCondition(distance, Condition.Predicate.GREATER, closeDistance) }), sMachine.StateFromName(typeof(JackDefensive).Name));

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
        distance.value = Mathf.Abs(owner.transform.position.x - owner.opponent.transform.position.x);
        if (distance.value < closeDistance)
        {
            timer.value += Time.deltaTime;
        }
        else
        {
            timer.value = (timer.value < 0) ? 0 : timer.value - Time.deltaTime;
        }

        //ability update
        trapsOffCD.value = GetTrapsOffCD();
        //Debug.Log("Distance: " + distance.value);
        sMachine.Update();
    }

    public override bool shouldJump()
    {
        return sMachine.currentState.shouldJump();
    }
    public override float StateMovement()
    {
        return sMachine.currentState.StateMovement();
    }

    public override int UseAbility()
    {
        return sMachine.currentState.UseAbility();
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


    private int GetTrapsOffCD()
    {
        int retVal = 0;
        if (owner.currentAbilityOneCooldown <= 0) retVal++;
        if (owner.currentAbilityTwoCooldown <= 0) retVal++;
        return retVal;
    }
}
