using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackAbilityThree : AbilityTemplate
{
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
        //        throw new System.NotImplementedException();
        if (audioOnDestroy != null)
        {
            //play
            audioOnDestroy.Play();
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        //throw new System.NotImplementedException();
    }
}
