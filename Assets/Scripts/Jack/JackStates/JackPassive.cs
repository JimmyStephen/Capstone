using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackPassive : State
{
    public JackPassive(CharacterTemplate owner, string name) : base(owner, name) { }

    float startDistance = 0;

    public override void OnEnter()
    {
        startDistance = Mathf.Abs(Owner.transform.position.x - Owner.opponent.transform.position.x);
    }

    public override void OnExit()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        //throw new System.NotImplementedException();

        abilityTimer -= Time.deltaTime;
    }

    public override bool ShouldJump()
    {
        return true;
    }

    public override float StateMovement()
    {
        float distance = Owner.transform.position.x - Owner.opponent.transform.position.x;
        if (Mathf.Approximately(startDistance, distance) || Mathf.Abs(distance) > 5)
        {
            return 0;
        }
        return Mathf.Sign(distance);
    }

    float abilityTimer = 0;
    public override int UseAbility()
    {
        if (Owner.CheckForDebuff())
        {
            return 3;
        }

        if (abilityTimer > 0)
        {
            return UseBasicAbility() ? 0 : 4;
        }
        //0 basic
        //1 basic ability
        //2 secondary ability
        //3 ult
        //4 none
        int[] abilityOptions = new int[] { 0, 1, 2, 4 };
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
            abilityTimer = 1.5f;
            CheckDirection();
        }
        return retVal;
    }

    public override bool UseBasicAbility()
    {
        //conditions to use
        //enemy close
        return (Owner.currentBasicAttackCooldown <= 0 && Mathf.Abs(Owner.transform.position.x - Owner.opponent.transform.position.x) < 3);
    }
    public override bool UseAbilityOne()
    {
        //conditions to use
        //off cd
        return (Owner.currentAbilityOneCooldown <= 0);
    }
    public override bool UseAbilityTwo()
    {
        //conditions to use
        //off cd
        return (Owner.currentAbilityTwoCooldown <= 0);
    }
    public override bool UseAbilityThree()
    {
        //conditions to use
        //enemy close
        return (Owner.currentAbilityThreeCooldown <= 0 && Mathf.Abs(Owner.transform.position.x - Owner.opponent.transform.position.x) < 3);
    }
}
