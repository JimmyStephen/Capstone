using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeimosPassive : State
{
    //on start roar
    //if enemy close stomp/punch
    //if enemy far jump
    //if enemy midrange roar
    public DeimosPassive(CharacterTemplate owner, string name) : base(owner, name) { }

    readonly float maxDistance = 5;
    readonly float minDistance = 1;

    public override void OnEnter()
    {
        //Ult
        if (UseAbilityThree()) { Owner.AbilityThree(); }
    }

    public override void OnUpdate()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnExit()
    {
        //throw new System.NotImplementedException();
    }

    //------------
    //Movement

    public override bool ShouldJump()
    {
        //throw new System.NotImplementedException();
        return true;
    }

    public override float StateMovement()
    {
        //get distance
        float distance = Owner.transform.position.x - Owner.opponent.transform.position.x;
        if(distance > maxDistance)
        {
            return -Mathf.Sign(distance);
        }else if(distance < minDistance)
        {
            return Mathf.Sign(distance);
        }
        else
        {
            return 0;
        }
    }

    //----------------
    //Abilities

    public override int UseAbility()
    {
        int retVal = 4;

        int[] abilityOptions = new int[] { 0, 1, 2, 3, 4 };
        abilityOptions = Shuffle(abilityOptions);
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

        if (retVal != 4)
        {
            CheckDirection();
        }

        return retVal;
    }

    public override bool UseBasicAbility()
    {
        //off cd
        //if the enemy is close
        return (Owner.currentBasicAttackCooldown <= 0 && Mathf.Abs(Owner.transform.position.x - Owner.opponent.transform.position.x) < 3);
    }
    public override bool UseAbilityOne()
    {
        //make sure the enemy isnt above you & that your on the ground
        if(Mathf.Abs(Owner.transform.position.y - Owner.opponent.transform.position.y) > 3 || (!Owner.characterController.m_Grounded))
        {
            return false;
        }
        //make sure the enemy is close & the ability is off cd
        return (Owner.currentBasicAttackCooldown <= 0 && Mathf.Abs(Owner.transform.position.x - Owner.opponent.transform.position.x) < 3);
    }
    public override bool UseAbilityTwo()
    {
        //off cd
        //if the enemy is far away
        //on the ground
        if (!Owner.characterController.m_Grounded) return false;
        return (Owner.currentBasicAttackCooldown <= 0 && Mathf.Abs(Owner.transform.position.x - Owner.opponent.transform.position.x) > 3);
    }
    public override bool UseAbilityThree()
    {
        return (Owner.currentAbilityThreeCooldown <= 0);
    }
}
