using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackAbilityThree : AbilityTemplate
{
    public override void OnCreation()
    {
        //throw new System.NotImplementedException();
        //cleanse all debuffs
        List<Effect> effects = new List<Effect>();
        foreach(var e in parent.GetComponent<CharacterTemplate>().effects)
        {
            if (e.checkIsDebuff())
            {
                effects.Add(e);
            }
        }

        foreach(var e in effects)
        {
            parent.GetComponent<CharacterTemplate>().effects.Remove(e);
        }


    }

    public override void OnDestroy()
    {
//        throw new System.NotImplementedException();
    }

    public override void OnTriggerEnter(Collider other)
    {
        //throw new System.NotImplementedException();
    }
}
