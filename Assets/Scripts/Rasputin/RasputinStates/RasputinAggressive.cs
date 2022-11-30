using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RasputinAggressive : State
{
    //How he acts when aggressive
    public RasputinAggressive(CharacterTemplate owner, string name) : base(owner, name) { }

    public override void OnEnter()
    {
        //throw new System.NotImplementedException();
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

        int[] abilityOptions = new int[] { 0, 0, 0, 1, 1, 2, 4 };
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
        return (Owner.currentBasicAttackCooldown <= 0 && Mathf.Abs(Owner.transform.position.x - Owner.opponent.transform.position.x) <= 3);
    }

    public override bool UseAbilityOne()
    {        
        //off cd
        //if the enemy is out of melee range
        return (Owner.currentBasicAttackCooldown <= 0 && Mathf.Abs(Owner.transform.position.x - Owner.opponent.transform.position.x) >= 3);
    }

    public override bool UseAbilityTwo()
    {
        //drink if health is below a threshold
            //when below X%
        float threshold = .50f;
        //and it is off cd
        return ((Owner.currentAbilityThreeCooldown <= 0) && ((Owner.health.GetCurrent() / Owner.health.GetMax()) < threshold));
    }

    public override bool UseAbilityThree()
    {
        throw new System.NotImplementedException("Rasputin Cannot Activate His Ult");
    }
}
