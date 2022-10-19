using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtemisDefensive : State
{
    public ArtemisDefensive(CharacterTemplate owner, string name) : base(owner, name) { }

    float jumpTimer = 5;

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
    }

    float justJumped = 0;
    public override bool shouldJump()
    {
        float distanceHeight = owner.opponent.transform.position.y - owner.transform.position.y;
        //condition to jump
        if ((distanceHeight > 3 && justJumped <= 0) || jumpTimer <= 0)
        {
            justJumped = 1;
            jumpTimer = Random.Range(3, 6);
            return true;
        }
        return false;
    }

    public override float StateMovement()
    {
        float distance = owner.transform.position.x - owner.opponent.transform.position.x;

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
        abilityOptions = shuffle(abilityOptions);

        int retVal = 4;

        for (int i = 0; i < abilityOptions.Length; i++)
        {
            switch (abilityOptions[i])
            {
                case 0:
                    if (useBasicAbility()) retVal = 0;
                    break;
                case 1:
                    if (useAbilityOne()) retVal = 1;
                    break;
                default:
                    retVal = 4;
                    break;
            }
        }
        if (retVal != 4) checkDirection();
        return retVal;
        //throw new System.NotImplementedException();
    }

    public override bool useBasicAbility()
    {
        //if enemy close enough

        //conditions to use
        return (owner.currentBasicAttackCooldown <= 0 && Mathf.Abs(owner.transform.position.x - owner.opponent.transform.position.x) < 2);
    }
    public override bool useAbilityOne()
    {
        //if not on cd
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
