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
        justJumped -= Time.deltaTime;
    }

    float justJumped = 0;
    public override bool ShouldJump()
    {
        if (justJumped >= 0) return false;
        if(Random.Range(0, 1.0f) > .5f)
        {
            justJumped = 3;
            return true;
        }
        return false;
    }

    public override float StateMovement()
    {
        float distance = Owner.transform.position.x - Owner.opponent.transform.position.x;
        return Mathf.Sign(distance);
    }

    public override int UseAbility()
    {
        int retVal = UseAbilityOne() ? 1 : 4;
        if (retVal != 4) CheckDirection();
        return retVal;
    }

    public override bool UseBasicAbility()
    {
        return (Owner.currentBasicAttackCooldown <= 0 && Mathf.Abs(Owner.transform.position.x - Owner.opponent.transform.position.x) < 2);
    }
    public override bool UseAbilityOne()
    {
        //conditions to use
        return (Owner.currentAbilityOneCooldown <= 0);
    }
    public override bool UseAbilityTwo()
    {
        return false;
    }
    public override bool UseAbilityThree()
    {
        return false;
    }
}
