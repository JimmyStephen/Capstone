using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtemisLowHealth : HealthStateTemplate
{
    public ArtemisLowHealth(CharacterTemplate owner, string name, State[] childStates) : base(owner, name, childStates) { }

    public override void OnCreate()
    {
        //create transitions
        //to agressive

        //to defensive

        //to passive

/*        sMachine.AddTransition();
        sMachine.AddTransition();
        sMachine.AddTransition();
        sMachine.AddTransition();
        sMachine.AddTransition();*/
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
        float distanceAcross = Mathf.Abs(owner.transform.position.x - owner.opponent.transform.position.x);
        bool basicOnCD = owner.basicAttackDuration > 0;
        bool abilityOneCD = owner.currentAbilityOneCooldown > 0;
        bool abilityTwoCD = owner.currentAbilityTwoCooldown > 0;
        bool ultCD = owner.currentAbilityThreeCooldown > 0;

        bool abilityOnCD = basicOnCD && abilityOneCD && abilityTwoCD && ultCD;
        bool damageAbilityOnCD = basicOnCD && abilityTwoCD && ultCD;

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
