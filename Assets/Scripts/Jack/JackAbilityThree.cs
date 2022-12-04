using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackAbilityThree : AbilityTemplate
{
    public float Range = 5;
    [SerializeField] float teleportDistance = 1;
    //before use***
        //make sure the enemy is within a certain range (4 units?)
            //if you arent within the range, the ability is canceled since they are out of range
            //if they are within range, cast the ability and continue to here

    //Get location of opponent
        //Get direction it is facing
            //check behind it to see if there is a wall
                //if no wall, teleport there
                //if wall, teleport in front of them
    //after teleport immediatly make an attack dealing a large amount of damage
    CharacterTemplate ct;

    public override void OnCreation()
    {
        ct = parent.GetComponent<CharacterTemplate>();
        //throw new System.NotImplementedException();
        ct.CleanseDebuffs();

        //Get location of opponent
        Vector3 enemyLocation = ct.opponent.transform.position;
        //Get direction it is facing
        bool opponentFacingRight = (ct.opponent.transform.rotation.y > 0);
        
        //check behind it to see if there is a wall
        //set the teleport location
        Vector3 teleportLocation;
        if(opponentFacingRight)
        {
            float wallLocation = ct.leftWall.transform.position.x;
            Vector3 estimateTeleportLocation = new(enemyLocation.x - teleportDistance, enemyLocation.y, enemyLocation.z);
            //if there is no wall, set the location behind the target
            //if there is a wall set the location infront of the target
            if(wallLocation < estimateTeleportLocation.x)
            {
                teleportLocation = estimateTeleportLocation;
            }
            else
            {
                estimateTeleportLocation.x += (2 * teleportDistance);
                teleportLocation = estimateTeleportLocation;
            }
        }
        else
        {
            float wallLocation = ct.rightWall.transform.position.x;
            Vector3 estimateTeleportLocation = new(enemyLocation.x + teleportDistance, enemyLocation.y, enemyLocation.z);
            //if there is no wall, set the location behind the target
            //if there is a wall set the location infront of the target
            if (wallLocation > estimateTeleportLocation.x)
            {
                teleportLocation = estimateTeleportLocation;
            }
            else
            { 
                estimateTeleportLocation.x -= (2 * teleportDistance);
                teleportLocation = estimateTeleportLocation;
            }
        }
        //teleport to the chosenlocation
        parent.transform.position = teleportLocation;

        //after teleport fix direction to be facing opponent
        CheckDirection();
        //give a large damage buff for a short duration
        //give a minor speed buff
        ct.effects.Add(new(false, 1, 0, 0, 0, 0, 3, 1, false));
        ct.effects.Add(new(false, 4, 0, 0, 0, 0, 1, 1.5f, false));


        //
        if (audioOnCreate != null)
        {
            //play
            audioOnCreate.Play();
        }

    }

    public override void OnDestroy()
    {
    }

    public override void OnTriggerEnter(Collider other)
    {
        //throw new System.NotImplementedException();
    }

    /// <summary>
    /// Makes sure your facing the right direction
    /// </summary>
    public void CheckDirection()
    {
        //get direction
        float direction = Mathf.Sign(parent.transform.position.x - ct.opponent.transform.position.x);
        if ((direction > 0 && ct.characterController.GetDirection()) || (direction < 0 && !ct.characterController.GetDirection()))
        {
            ct.characterController.Flip();
        }
    }
}
