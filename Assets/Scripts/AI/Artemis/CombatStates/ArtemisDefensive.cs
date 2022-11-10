using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtemisDefensive : State
{
    public ArtemisDefensive(CharacterTemplate owner, string name) : base(owner, name) { }

    float attackTimer = 2;
    float jumpTimer = 3;

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
        jumpTimer -= Time.deltaTime;
        justJumped -= Time.deltaTime;
        attackTimer -= Time.deltaTime;
    }

    float justJumped = 0;
    public override bool ShouldJump()
    {
        float distanceHeight = Owner.opponent.transform.position.y - Owner.transform.position.y;
        //condition to jump
        if ((distanceHeight > 3 && justJumped <= 0) || jumpTimer <= 0)
        {
            justJumped = 1;
            jumpTimer = Random.Range(2, 4);
            return true;
        }
        return false;
    }

    public override float StateMovement()
    {
        float distance = Owner.transform.position.x - Owner.opponent.transform.position.x;

        if (Mathf.Abs(distance) > 5)
        {
            return 0;
        }

        return Mathf.Sign(distance);
    }

    public override int UseAbility()
    {
        //0 basic
        //1 basic ability
        //2 secondary ability
        //3 ult
        //4 none
        int[] abilityOptions = new int[] { 0, 1, 4 };
        abilityOptions = Shuffle(abilityOptions);

        int retVal = 4;

        for (int i = 0; i < abilityOptions.Length; i++)
        {
            switch (abilityOptions[i])
            {
                case 0:
                    if (UseBasicAbility()) retVal = 0;
                    break;
                case 1:
                    if (UseAbilityOne()) retVal = 1;
                    break;
                default:
                    retVal = 4;
                    break;
            }
        }
        if (retVal == 2 && attackTimer > 0)
        {
            retVal = 4;
            attackTimer = 1;
        }
        if (retVal != 4) CheckDirection();
        return retVal;
        //throw new System.NotImplementedException();
    }

    public override bool UseBasicAbility()
    {
        //if enemy close enough

        //conditions to use
        return (Owner.currentBasicAttackCooldown <= 0 && Mathf.Abs(Owner.transform.position.x - Owner.opponent.transform.position.x) < 2);
    }
    public override bool UseAbilityOne()
    {
        //if not on cd
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
