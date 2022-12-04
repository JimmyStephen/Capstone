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

    public override void OnEnter()
    {
        //throw new System.NotImplementedException();
        timer = Random.Range(minTime, maxTime);
        runningAt = (Random.Range(0.0f, 1.0f) > .5);
    }

    public override void OnExit()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        //throw new System.NotImplementedException();
        timer -= Time.deltaTime;
        secondTimer -= Time.deltaTime;
        abilityTimer -= Time.deltaTime;
    }

    public override bool ShouldJump()
    {
        return true;
    }

    float secondTimer = 0;
    float thirdTimer = 0;
    float maxValueThirdTimer = 2;
    public override float StateMovement()
    {
        //throw new System.NotImplementedException();
        float distance = Owner.transform.position.x - Owner.opponent.transform.position.x;
        
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
    bool nextAbilityBasic = false;
    public override int UseAbility()
    {
        if (Owner.CheckForDebuff())
        {
            if (UseAbilityThree())
            {
                return 3;
            }
            else if (Owner.CheckForStun())
            {
                return 4;
            }
        }

        if (abilityTimer > 0)
        {
            return UseBasicAbility() ? 0 : 4;
        }

        if(nextAbilityBasic)
        {
            Debug.Log("Use Basic Combo");
            nextAbilityBasic = false;
            CheckDirection();
            return UseBasicAbility() ? 0 : 4;
        }
        //0 basic
        //1 basic ability
        //2 secondary ability
        //3 ult
        //4 none
        int[] abilityOptions = new int[] { 0, 0, 1, 2, 3, 3, 3, 4 };
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
            abilityTimer = .5f;
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
        //enemy in range
        bool retVal = (Owner.currentAbilityThreeCooldown <= 0 && Mathf.Abs(Owner.transform.position.x - Owner.opponent.transform.position.x) < Owner.abilityThreeProjectile.GetComponent<JackAbilityThree>().Range);
        nextAbilityBasic = retVal;
        return retVal;
    }
}
