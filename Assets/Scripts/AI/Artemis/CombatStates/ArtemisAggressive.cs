using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtemisAggressive : State
{
    public ArtemisAggressive(CharacterTemplate owner, string name) : base(owner, name) { }

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
        justJumped -= Time.deltaTime;
        //throw new System.NotImplementedException();
    }

    float justJumped = 0;
    public override bool ShouldJump()
    {
        float distanceHeight = Owner.opponent.transform.position.y - Owner.transform.position.y;
        //condition to jump
        if ((distanceHeight > 3 && justJumped <= 0))
        {
            justJumped = 1;
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
        int[] abilityOptions = new int[] { 0, 1, 2, 3, 4 };
        abilityOptions = Shuffle(abilityOptions);

        int retVal = 4;

        for(int i = 0; i < abilityOptions.Length; i++)
        {
            switch(abilityOptions[i])
            {
                case 0:
                    if (UseBasicAbility()) retVal = 0;
                    break;
                case 1:
                    if (UseAbilityOne()) retVal = 1;
                    break;
                case 2:
                    if (UseAbilityTwo()) retVal = 2;
                    break;
                case 3:
                    if (UseAbilityThree()) retVal = 3;
                    break;
                default:
                    retVal = 4;
                    break;
            }
        }
        if (retVal != 4) CheckDirection();
        return retVal;
    }

    //also check for cd
    public override bool UseBasicAbility()
    {
        //conditions to use
        //enemy close
        return (Owner.currentBasicAttackCooldown <= 0 && Mathf.Abs(Owner.transform.position.x - Owner.opponent.transform.position.x) < 2);
    }
    public override bool UseAbilityOne()
    {
        //conditions to use
        //enemy very close
        return (Owner.currentAbilityOneCooldown <= 0 && Mathf.Abs(Owner.transform.position.x - Owner.opponent.transform.position.x) < 1);
    }
    public override bool UseAbilityTwo()
    {
        //conditions to use
        //enemy far
        return (Owner.currentAbilityTwoCooldown <= 0 && Mathf.Abs(Owner.transform.position.x - Owner.opponent.transform.position.x) > 3);
    }
    public override bool UseAbilityThree()
    {
        //conditions to use
        //enemy far
        return (Owner.currentAbilityThreeCooldown <= 0 && Mathf.Abs(Owner.transform.position.x - Owner.opponent.transform.position.x) > 3);
    }
}
