using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackAggressive : State
{
    public JackAggressive(CharacterTemplate owner, string name) : base(owner, name) { }

    float timer = 0;
    float maxTime = 4;
    float minTime = 1;
    float minDistance = 2;
    bool runningAt = false;
    //
    float jumpTimer = 0;
    float minJumpTime = 3;
    float maxJumpTime = 8;

    public override void OnEnter()
    {
        //throw new System.NotImplementedException();
        timer = Random.Range(minTime, maxTime);
        runningAt = (Random.Range(0.0f, 1.0f) > .5);
        jumpTimer = Random.Range(minJumpTime, maxJumpTime);
    }

    public override void OnExit()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        //throw new System.NotImplementedException();
        timer -= Time.deltaTime;
        jumpTimer -= Time.deltaTime;
        secondTimer -= Time.deltaTime;
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

    float secondTimer = 0;
    float thirdTimer = 0;
    float maxValueThirdTimer = 2;
    public override float StateMovement()
    {
        //throw new System.NotImplementedException();
        float distance = owner.transform.position.x - owner.opponent.transform.position.x;
        
        //check distance, if to close then swap to run away
        if (distance < minDistance && secondTimer < 0)
        {
            thirdTimer += Time.deltaTime;
            if (thirdTimer > maxValueThirdTimer)
            {
                runningAt = !runningAt;
                secondTimer = 1;
                timer = Random.Range(minTime, maxTime);
            }
        }
        else
        {
            thirdTimer = (thirdTimer - Time.deltaTime < 0) ? 0 : thirdTimer -= Time.deltaTime;
        }

        //if runningAt go to the opponent
        if (runningAt)
        {
            if (timer <= 0)
            {
                runningAt = !runningAt;
                timer = Random.Range(minTime, maxTime);
            }
            return -Mathf.Sign(distance);
        }
        //else run away from the opponent
        else
        {
            if (timer <= 0)
            {
                runningAt = !runningAt;
                timer = Random.Range(minTime, maxTime) + minTime;
            }
            return Mathf.Sign(distance);
        }
    }

    float abilityTimer = 0;
    public override int UseAbility()
    {
        if (abilityTimer < 0)
        {
            abilityTimer = .5f;
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
