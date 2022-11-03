using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackPassive : State
{
    public JackPassive(CharacterTemplate owner, string name) : base(owner, name) { }

    float startDistance = 0;
    float jumpTimer = 0;
    float minJumpTime = 3;
    float maxJumpTime = 8;

    public override void OnEnter()
    {
        startDistance = Mathf.Abs(owner.transform.position.x - owner.opponent.transform.position.x);
        jumpTimer = Random.Range(minJumpTime, maxJumpTime);
    }

    public override void OnExit()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        //throw new System.NotImplementedException();
        jumpTimer -= Time.deltaTime;
        abilityTimer -= Time.deltaTime;
    }

    public override bool shouldJump()
    {
        //        throw new System.NotImplementedException();
        if(jumpTimer < 0)
        {
            jumpTimer = Random.Range(minJumpTime, maxJumpTime);
            return true;
        }
        return false;
    }

    public override float StateMovement()
    {
        float distance = owner.transform.position.x - owner.opponent.transform.position.x;
        if (Mathf.Approximately(startDistance, distance) || Mathf.Abs(distance) > 5)
        {
            return 0;
        }
        return Mathf.Sign(distance);
    }

    float abilityTimer = 0;
    public override int UseAbility()
    {
        if (abilityTimer < 0)
        {
            abilityTimer = 1.5f;
            return 4;
        }
        //0 basic
        //1 basic ability
        //2 secondary ability
        //3 ult
        //4 none
        int[] abilityOptions = new int[] { 0, 1, 2, 4 };
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
                default:
                    retVal = 4;
                    break;
            }
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
