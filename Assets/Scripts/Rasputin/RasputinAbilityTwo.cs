using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RasputinAbilityTwo : AbilityTemplate
{
    [SerializeField] float healAmount = 20;

    public override void OnCreation()
    {
        if (audioOnCreate != null)
        {
            //play
            audioOnCreate.Play();
        }
        //Debug.Log("Starting Health: " + parent.GetComponent<CharacterTemplate>().health.GetCurrent());
        parent.GetComponent<CharacterTemplate>().effects.Add(new(false, destroyAfterSeconds, 0, (healAmount / destroyAfterSeconds), 0, 0, 0, 0, false));
    }

    public override void OnDestroy()
    {
    }

    public override void OnTriggerEnter(Collider other)
    {
        //
    }
}
