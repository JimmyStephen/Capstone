using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtemisHighHealth : HealthStateTemplate
{
    public ArtemisHighHealth(CharacterTemplate owner, string name, State[] childStates) : base(owner, name, childStates) { }

    public FloatRef distance;
    public BoolRef abilityOnCD;
    private const float distanceForAggression = 5;

    public override void OnCreate()
    {
        distance = new FloatRef();
        abilityOnCD = new BoolRef();

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
        sMachine.AddTransition(sMachine.StateFromName(typeof(ArtemisAggressive).Name), new Transition(new Condition[] { new FloatCondition(distance, Condition.Predicate.GREATER, distanceForAggression), new BoolCondition(abilityOnCD, true) }), sMachine.StateFromName(typeof(ArtemisPassive).Name));
        sMachine.AddTransition(sMachine.StateFromName(typeof(ArtemisDefensive).Name), new Transition(new Condition[] { new FloatCondition(distance, Condition.Predicate.GREATER, distanceForAggression), new BoolCondition(abilityOnCD, true) }), sMachine.StateFromName(typeof(ArtemisPassive).Name));

        //set default state
        sMachine.setState(sMachine.StateFromName(typeof(ArtemisPassive).Name));
    }

    public override void OnEnter()
    {
//        throw new System.NotImplementedException();
    }

    public override void OnExit()
    {
        //        throw new System.NotImplementedException();
//        Debug.Log("Exit");
    }

    public override void OnUpdate()
    {
        //distance update
        distance.value = Mathf.Abs(Owner.transform.position.x - Owner.opponent.transform.position.x);
        //ability update
        bool basicOnCD = Owner.basicAttackDuration > 0;
        bool abilityOneCD = Owner.currentAbilityOneCooldown > 0;
        bool abilityTwoCD = Owner.currentAbilityTwoCooldown > 0;
        bool ultCD = Owner.currentAbilityThreeCooldown > 0;
        abilityOnCD.value = basicOnCD && abilityOneCD && abilityTwoCD && ultCD;

        sMachine.Update();
    }

    public override bool ShouldJump()
    {
        return sMachine.currentState.ShouldJump();
    }
    public override float StateMovement()
    {
        return sMachine.currentState.StateMovement();
    }
    public override bool UseBasicAbility()
    {
        throw new System.Exception("You shouldn't call this method this way");
    }
    public override bool UseAbilityOne()
    {
        throw new System.Exception("You shouldn't call this method this way");
    }
    public override bool UseAbilityTwo()
    {
        throw new System.Exception("You shouldn't call this method this way");
    }
    public override bool UseAbilityThree()
    {
        throw new System.Exception("You shouldn't call this method this way");
    }
    public override int UseAbility()
    {
        return sMachine.currentState.UseAbility();
    }
}
