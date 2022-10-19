using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtemisPassive : State
{
    public ArtemisPassive(CharacterTemplate owner, string name) : base(owner, name) { }

    float attackTimer = 2;
    float startingDistance = 0;
    float jumpTimer = 5;

    public override void OnEnter()
    {
        //throw new System.NotImplementedException();
        startingDistance = Mathf.Abs(owner.transform.position.x - owner.opponent.transform.position.x);
    }

    public override void OnExit()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        //throw new System.NotImplementedException();
        attackTimer -= Time.deltaTime;
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
            jumpTimer = Random.Range(1, 3);
            return true;
        }
        return false;
    }

    public override float StateMovement()
    {
        float distance = owner.transform.position.x - owner.opponent.transform.position.x;
        if (Mathf.Approximately(startingDistance, distance) || Mathf.Abs(distance) > 5)
        {
            return 0;
        }

        return Mathf.Sign(distance);
    }

    public override int UseAbility()
    {
        //dont use abilities to often while in passive state
        if (attackTimer > 0) return 4;
        //0 basic
        //1 basic ability
        //2 secondary ability
        //3 ult
        //4 none
        int[] abilityOptions = new int[] { 0, 1, 2, 3, 4 };
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
                case 2:
                    if (useAbilityTwo()) retVal = 2;
                    break;
                case 3:
                    if (useAbilityThree()) retVal = 3;
                    break;
                default:
                    retVal = 4;
                    break;
            }
        }
        if(retVal != 4) checkDirection();
        attackTimer = 2;
        return retVal;
        //throw new System.NotImplementedException();
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
        //enemy very close
        return (owner.currentAbilityOneCooldown <= 0 && Mathf.Abs(owner.transform.position.x - owner.opponent.transform.position.x) < 1);
    }
    public override bool useAbilityTwo()
    {
        //conditions to use
        //enemy far
        return (owner.currentAbilityTwoCooldown <= 0 && Mathf.Abs(owner.transform.position.x - owner.opponent.transform.position.x) > 3);
    }
    public override bool useAbilityThree()
    {
        //conditions to use
        //enemy far
        return (owner.currentAbilityThreeCooldown <= 0 && Mathf.Abs(owner.transform.position.x - owner.opponent.transform.position.x) > 3);
    }
}
