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

    public override bool shouldJump()
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
        float distance = owner.transform.position.x - owner.opponent.transform.position.x;
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
        abilityOptions = shuffle(abilityOptions);

        int retVal = 4;

        for (int i = 0; i < abilityOptions.Length; i++)
        {
            switch (abilityOptions[i])
            {
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
        
        if (abilityTimer < 0 && retVal != 3)
        {
            abilityTimer = 1.5f;
            return 4;
        }

        if (retVal != 4) checkDirection();
        return retVal;
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
        //off cd
        return (owner.currentAbilityOneCooldown <= 0);
    }
    public override bool useAbilityTwo()
    {
        //conditions to use
        //off cd
        return (owner.currentAbilityTwoCooldown <= 0);
    }
    public override bool useAbilityThree()
    {
        //conditions to use
        //enemy close
        return (owner.currentAbilityThreeCooldown <= 0 && Mathf.Abs(owner.transform.position.x - owner.opponent.transform.position.x) < 3);
    }
}
