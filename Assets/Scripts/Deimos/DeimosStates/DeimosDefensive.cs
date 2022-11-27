using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeimosDefensive : State
{
    //--
    //DELETE (NOT GOING TO USE)
    //--

    //on start roar
    //if enemy close stomp/punch
    //if enemy far jump
    //if enemy midrange roar
    public DeimosDefensive(CharacterTemplate owner, string name) : base(owner, name) { }

    public override void OnEnter()
    {
        //Ult
        if (UseAbilityThree()) { Owner.AbilityThree(); }
    }

    public override void OnUpdate()
    {
        throw new System.NotImplementedException();
    }

    public override void OnExit()
    {
        throw new System.NotImplementedException();
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
        //throw new System.NotImplementedException();
        //run at the opponent
        float distance = Owner.transform.position.x - Owner.opponent.transform.position.x;
        return -Mathf.Sign(distance);
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
        //off cd
        //if the enemy is close
        return (Owner.currentBasicAttackCooldown <= 0 && Mathf.Abs(Owner.transform.position.x - Owner.opponent.transform.position.x) < 3);
    }
    public override bool UseAbilityTwo()
    {
        //off cd
        //if the enemy is far away
        return (Owner.currentBasicAttackCooldown <= 0 && Mathf.Abs(Owner.transform.position.x - Owner.opponent.transform.position.x) > 3);
    }
    public override bool UseAbilityThree()
    {
        return (Owner.currentAbilityThreeCooldown <= 0);
    }
}