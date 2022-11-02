using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackAggressive : State
{
    public JackAggressive(CharacterTemplate owner, string name) : base(owner, name) { }

    public override void OnEnter()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnExit()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        //throw new System.NotImplementedException();
    }

    public override bool shouldJump()
    {
        throw new System.NotImplementedException();
    }

    public override float StateMovement()
    {
        throw new System.NotImplementedException();
    }

    public override int UseAbility()
    {
        throw new System.NotImplementedException();
    }

    public override bool useBasicAbility()
    {
        //conditions to use
        //enemy close
        return (owner.currentBasicAttackCooldown <= 0 && Mathf.Abs(owner.transform.position.x - owner.opponent.transform.position.x) < 2);
    }
    public override bool useAbilityOne()
    {
        //conditions to use
        //off cd
        return (owner.currentAbilityOneCooldown <= 0);
    }
    public override bool useAbilityTwo()
    {
        //conditions to use
        //off cd
        return (owner.currentAbilityTwoCooldown <= 0);
    }
    public override bool useAbilityThree()
    {
        //conditions to use
        //enemy close
        return (owner.currentAbilityThreeCooldown <= 0 && Mathf.Abs(owner.transform.position.x - owner.opponent.transform.position.x) < 3);
    }
}
