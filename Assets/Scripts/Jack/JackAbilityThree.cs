using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackAbilityThree : AbilityTemplate
{
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

    public override void OnCreation()
    {
        //throw new System.NotImplementedException();
        //cleanse all debuffs
        List<Effect> effects = new();
        foreach(var e in parent.GetComponent<CharacterTemplate>().effects)
        {
            if (e.CheckIsDebuff())
            {
                effects.Add(e);
            }
        }

        foreach(var e in effects)
        {
            parent.GetComponent<CharacterTemplate>().effects.Remove(e);
        }
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
}
