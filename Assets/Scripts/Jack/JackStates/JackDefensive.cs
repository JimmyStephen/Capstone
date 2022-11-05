using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackDefensive : State
{
    public JackDefensive(CharacterTemplate owner, string name) : base(owner, name) { }

    float jumpTimer = 0;
    float minJumpTime = 3;
    float maxJumpTime = 8;

    public override void OnEnter()
    {
        //throw new System.NotImplementedException();
        jumpTimer = Random.Range(minJumpTime, maxJumpTime);
    }

    public override void OnExit()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        //0 basic
        //1 basic ability
        //2 secondary ability
        //3 ult
        //4 none
        jumpTimer -= Time.deltaTime;
        abilityTimer -= Time.deltaTime;
    }

    public override bool ShouldJump()
    {
        if (jumpTimer < 0)
        {
            jumpTimer = Random.Range(minJumpTime, maxJumpTime);
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

    float abilityTimer = 0;
    public override int UseAbility()
    {
        int[] abilityOptions = new int[] { 1, 2, 3, 4 };
        abilityOptions = Shuffle(abilityOptions);

        int retVal = 4;

        for (int i = 0; i < abilityOptions.Length; i++)
        {
            switch (abilityOptions[i])
            {
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
        
        if (abilityTimer < 0 && retVal != 3)
        {
            abilityTimer = 1.5f;
            return 4;
        }

        if (retVal != 4) CheckDirection();
        return retVal;
    }

    public override bool UseBasicAbility()
    {
        //conditions to use
        //enemy close
        return (Owner.currentBasicAttackCooldown <= 0 && Mathf.Abs(Owner.transform.position.x - Owner.opponent.transform.position.x) < 2);
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
