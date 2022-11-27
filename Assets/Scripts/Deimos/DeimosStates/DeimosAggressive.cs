using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeimosAggressive : State
{
    //start by using a jump at the opponent
    //if enemy close, stomp or punch
    //if enemy far jump
    //if enemy mid range roar
    public DeimosAggressive(CharacterTemplate owner, string name) : base(owner, name) { }

    float maxTimer = 2;
    float timer = 0;

    public override void OnEnter()
    {
        //Charge
        if (UseAbilityTwo()) { Owner.AbilityTwo(); }
    }

    public override void OnUpdate()
    {
        //if you get stunned or are unable to use an ability for some reason, reset the combo
        if (setNext != -1)
        {
            timer += Time.deltaTime;
            if (timer >= maxTimer)
            {
                timer = 0;
                setNext = -1;
            }
        }
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
        //throw new System.NotImplementedException();
        //run at the opponent
        float distance = Owner.transform.position.x - Owner.opponent.transform.position.x;
        return -Mathf.Sign(distance);
    }

    //----------------
    //Abilities
    int setNext = -1;
    public override int UseAbility()
    {
        if(setNext != -1)
        {
            switch (setNext)
            {
                case 0:
                    setNext = -1;
                    timer = 0;
                    if (UseBasicAbility())
                    {
                        return 0;
                    }
                    break;
                case 1:
                    setNext = -1;
                    timer = 0;
                    if (UseAbilityOne())
                    {
                        return 1;
                    }
                    break;
                case 2:
                    setNext = -1;
                    timer = 0;
                    if (UseAbilityTwo())
                    {
                        return 1;
                    }
                    break;
            }
        }
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
        if (Mathf.Abs(Owner.transform.position.y - Owner.opponent.transform.position.y) > 3 || (!Owner.characterController.m_Grounded))
        {
            return false;
        }
        //make sure the enemy is close & the ability is off cd
        bool ret = (Owner.currentBasicAttackCooldown <= 0 && Mathf.Abs(Owner.transform.position.x - Owner.opponent.transform.position.x) < 3);
        //if you stomp, set the next ability to be punch
        if (ret) setNext = 1;
        return ret;
    }
    public override bool UseAbilityTwo()
    {
        //on the ground
        if (!Owner.characterController.m_Grounded) return false;
        //off cd
        //if the enemy is far away
        bool ret = (Owner.currentBasicAttackCooldown <= 0 && Mathf.Abs(Owner.transform.position.x - Owner.opponent.transform.position.x) > 3);
        //if you jump, set the next ability to be stomp
        if(ret) setNext = 1;
        return ret;
    }
    public override bool UseAbilityThree()
    {
        bool ret = (Owner.currentAbilityThreeCooldown <= 0);
        //if you yell jump
        if (ret) setNext = 2;
        return ret;
    }
}
