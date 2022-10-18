using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtemisScared : State
{
    public ArtemisScared(CharacterTemplate owner, string name) : base(owner, name) { }

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
        return false;
    }

    public override float StateMovement()
    {
        float distance = owner.transform.position.x - owner.opponent.transform.position.x;
        return Mathf.Sign(distance);
    }

    public override int UseAbility()
    {
        return useAbilityOne() ? 1 : 4;
    }

    public override bool useBasicAbility()
    {
        return (owner.currentBasicAttackCooldown <= 0 && Mathf.Abs(owner.transform.position.x - owner.opponent.transform.position.x) < 2);
    }
    public override bool useAbilityOne()
    {
        //conditions to use
        return (owner.currentAbilityOneCooldown <= 0);
    }
    public override bool useAbilityTwo()
    {
        return false;
    }
    public override bool useAbilityThree()
    {
        return false;
    }
}
