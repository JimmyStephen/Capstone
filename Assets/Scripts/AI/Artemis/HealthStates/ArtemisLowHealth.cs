using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtemisLowHealth : HealthStateTemplate
{
    public ArtemisLowHealth(CharacterTemplate owner, string name, State[] childStates) : base(owner, name, childStates) { }

    public FloatRef distance;
    public BoolRef abilityOnCD;


    public override void OnCreate()
    {
        //create transitions
        //to agressive

        //to defensive

        //to passive

//        sMachine.AddTransition();
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
        //distance update
        distance.value = Mathf.Abs(owner.transform.position.x - owner.opponent.transform.position.x);

        //ability update
        bool basicOnCD = owner.basicAttackDuration > 0;
        bool abilityOneCD = owner.currentAbilityOneCooldown > 0;
        bool abilityTwoCD = owner.currentAbilityTwoCooldown > 0;
        bool ultCD = owner.currentAbilityThreeCooldown > 0;
        abilityOnCD.value = basicOnCD && abilityOneCD && abilityTwoCD && ultCD;

        sMachine.Update();
    }

    public override bool shouldJump()
    {
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
