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
        //0 basic
        //1 basic ability
        //2 secondary ability
        //3 ult
        //4 none
        int[] abilityOptions = new int[] { 0, 1, 2, 3, 4 };
        abilityOptions = shuffle(abilityOptions);

        int retVal = 4;

        for(int i = 0; i < abilityOptions.Length; i++)
        {
            switch(abilityOptions[i])
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
        return retVal;
    }

    //also check for cd
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
