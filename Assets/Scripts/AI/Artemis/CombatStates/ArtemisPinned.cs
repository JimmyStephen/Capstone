using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtemisPinned : State
{
    //get the location of the wall
    //run away from the wall
    //use abilities that would help get away
        //Roll
        //Ult
    public ArtemisPinned(CharacterTemplate owner, string name) : base(owner, name) { }

    float jumpTimer = 3;
    float attackTimer = 2;
    private float closeWallPosition;

    public override void OnEnter()
    {
        closeWallPosition = GetCloserWall();
        //throw new System.NotImplementedException();
    }
    public override void OnExit()
    {
        //throw new System.NotImplementedException();
    }
    public override void OnUpdate()
    {
        //throw new System.NotImplementedException();
        jumpTimer -= Time.deltaTime;
        attackTimer -= Time.deltaTime;
    }

    //Movement
    public override bool ShouldJump()
    {
        //condition to jump
        if (jumpTimer <= 0)
        {
            jumpTimer = Random.Range(2, 3);
            return true;
        }
        return false;
    }
    public override float StateMovement()
    {
        float distance = Owner.transform.position.x - closeWallPosition;
        return Mathf.Sign(distance);
    }

    //Abilities
    public override int UseAbility()
    {
        //0 basic
        //1 basic ability
        //2 secondary ability
        //3 ult
        //4 none
        if (attackTimer > 0) return 4;

        int[] abilityOptions = new int[] { 0, 0, 1, 1, 1, 3, 4 };
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
                case 3:
                    if(UseAbilityThree()) retVal = 3;
                    break;
                default:
                    retVal = 4;
                    break;
            }
        }

        if (retVal != 4)
        {
            CheckDirection();
            if (retVal == 1) CheckWallDirection();
            attackTimer = 2;
        }
        return retVal;
    }
    public override bool UseBasicAbility()
    {
        //if enemy close enough
        //conditions to use
        return (Owner.currentBasicAttackCooldown <= 0 && Mathf.Abs(Owner.transform.position.x - Owner.opponent.transform.position.x) < 2);
    }
    public override bool UseAbilityOne()
    {
        //if not on cd
        //conditions to use
        return (Owner.currentAbilityOneCooldown <= 0);
    }
    public override bool UseAbilityTwo()
    {
        //throw new System.NotImplementedException();
        //Doesn't help escape from the wall
        return false;
    }
    public override bool UseAbilityThree()
    {
        return (Owner.currentAbilityThreeCooldown <= 0);
    }

    ///
    private float GetCloserWall()
    {
        float distanceLeft = Mathf.Abs(Owner.transform.position.x - Owner.leftWall.transform.position.x);
        float distanceRight = Mathf.Abs(Owner.transform.position.x - Owner.rightWall.transform.position.x);
        float retVal = (distanceLeft > distanceRight) ? Owner.rightWall.transform.position.x : Owner.leftWall.transform.position.x;
        return retVal;
    }
    public void CheckWallDirection()
    {
        //get direction
        float direction = Mathf.Sign(Owner.transform.position.x - closeWallPosition);
        if ((direction > 0 && Owner.characterController.GetDirection()) || (direction < 0 && !Owner.characterController.GetDirection()))
        {
            Owner.characterController.Flip();
        }
    }
}
