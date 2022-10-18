using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtemisLowHealth : HealthStateTemplate
{
    public ArtemisLowHealth(CharacterTemplate owner, string name, State[] childStates) : base(owner, name, childStates) { }

    public FloatRef distance;
    public BoolRef abilityOnCD;
    public FloatRef damageTaken;
    public FloatRef scaredDuration;

    private float dmgForScared;

    private float distanceForAggression = 5;
    float currentH = 0;


    public override void OnCreate()
    {
        currentH = owner.health.GetCurrent();

        distance = new FloatRef();
        abilityOnCD = new BoolRef();
        damageTaken = new FloatRef();
        scaredDuration = new FloatRef();

        //create transitions
        //agressive
        //sMachine.StateFromName(typeof(ArtemisAggressive).Name)
        //defensive
        //sMachine.StateFromName(typeof(ArtemisDefensive).Name)
        //passive
        //sMachine.StateFromName(typeof(ArtemisPassive).Name)

        //to agressive
        //if you are far from opponent w/ abilities off cd
        sMachine.AddTransition(sMachine.StateFromName(typeof(ArtemisPassive).Name), new Transition(new Condition[] { new FloatCondition(distance, Condition.Predicate.GREATER, distanceForAggression), new BoolCondition(abilityOnCD, false) }), sMachine.StateFromName(typeof(ArtemisAggressive).Name));
        sMachine.AddTransition(sMachine.StateFromName(typeof(ArtemisDefensive).Name), new Transition(new Condition[] { new FloatCondition(distance, Condition.Predicate.GREATER, distanceForAggression), new BoolCondition(abilityOnCD, false) }), sMachine.StateFromName(typeof(ArtemisAggressive).Name));

        //to defensive
        //if you are close to opponent
        sMachine.AddTransition(sMachine.StateFromName(typeof(ArtemisPassive).Name), new Transition(new Condition[] { new FloatCondition(distance, Condition.Predicate.LESS_EQUAL, distanceForAggression) }), sMachine.StateFromName(typeof(ArtemisDefensive).Name));
        sMachine.AddTransition(sMachine.StateFromName(typeof(ArtemisAggressive).Name), new Transition(new Condition[] { new FloatCondition(distance, Condition.Predicate.LESS_EQUAL, distanceForAggression) }), sMachine.StateFromName(typeof(ArtemisDefensive).Name));

        //to passive
        //if you are far but abilities on cd
        sMachine.AddTransition(sMachine.StateFromName(typeof(ArtemisAggressive).Name), new Transition(new Condition[] { new FloatCondition(distance, Condition.Predicate.GREATER, 5), new BoolCondition(abilityOnCD, true) }), sMachine.StateFromName(typeof(ArtemisPassive).Name));
        sMachine.AddTransition(sMachine.StateFromName(typeof(ArtemisDefensive).Name), new Transition(new Condition[] { new FloatCondition(distance, Condition.Predicate.GREATER, 5), new BoolCondition(abilityOnCD, true) }), sMachine.StateFromName(typeof(ArtemisPassive).Name));
        sMachine.AddTransition(sMachine.StateFromName(typeof(ArtemisScared).Name), new Transition(new Condition[] { new FloatCondition(scaredDuration, Condition.Predicate.LESS_EQUAL, 0), new BoolCondition(abilityOnCD, false) }), sMachine.StateFromName(typeof(ArtemisPassive).Name));

        //to scared
        sMachine.AddTransition(sMachine.StateFromName(typeof(ArtemisAggressive).Name), new Transition(new Condition[] { new FloatCondition(damageTaken, Condition.Predicate.GREATER, dmgForScared) }), sMachine.StateFromName(typeof(ArtemisScared).Name));
        sMachine.AddTransition(sMachine.StateFromName(typeof(ArtemisDefensive).Name), new Transition(new Condition[] { new FloatCondition(damageTaken, Condition.Predicate.GREATER, dmgForScared) }), sMachine.StateFromName(typeof(ArtemisScared).Name));
        sMachine.AddTransition(sMachine.StateFromName(typeof(ArtemisPassive).Name), new Transition(new Condition[] { new FloatCondition(damageTaken, Condition.Predicate.GREATER, dmgForScared) }), sMachine.StateFromName(typeof(ArtemisScared).Name));

        sMachine.setState(sMachine.StateFromName(typeof(ArtemisPassive).Name));
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
        damageTaken.value += currentH - owner.health.GetCurrent();
        currentH = owner.health.GetCurrent();
        damageTaken.value -= Time.deltaTime * 2;
        if (damageTaken.value < 0) damageTaken.value = 0;
        if (damageTaken.value > dmgForScared && scaredDuration.value <= 3)
        {
            scaredDuration.value = 2;
        }
        else
        {
            scaredDuration.value -= Time.deltaTime;
        }

        //distance update
        distance.value = Mathf.Abs(owner.transform.position.x - owner.opponent.transform.position.x);

        //ability update
        bool basicOnCD = owner.basicAttackDuration > 0;
        bool abilityOneCD = owner.currentAbilityOneCooldown > 0;
        bool abilityTwoCD = owner.currentAbilityTwoCooldown > 0;
        bool ultCD = owner.currentAbilityThreeCooldown > 0;
        abilityOnCD.value = basicOnCD && abilityOneCD && abilityTwoCD && ultCD;

        sMachine.Update();
        Debug.Log("Current Health State: " + name + "Current State In Health: " + sMachine.currentState.name);
        Debug.Log("Ability on CD: " + abilityOnCD.value);
    }

    public override bool shouldJump()
    {
        return sMachine.currentState.shouldJump();

        float distanceHeight = owner.opponent.transform.position.y - owner.transform.position.y;
        //condition to jump
        if(distanceHeight > 100)
        {
            return true;
        }
        return false;
    }
    public override float StateMovement()
    {
        return sMachine.currentState.StateMovement();
    }
    public override bool useBasicAbility()
    {
        throw new System.Exception("You shouldn't call this method this way");
    }
    public override bool useAbilityOne()
    {
        throw new System.Exception("You shouldn't call this method this way");
    }
    public override bool useAbilityTwo()
    {
        throw new System.Exception("You shouldn't call this method this way");
    }
    public override bool useAbilityThree()
    {
        throw new System.Exception("You shouldn't call this method this way");
    }
    public override int UseAbility()
    {
        return sMachine.currentState.UseAbility();
    }
}
