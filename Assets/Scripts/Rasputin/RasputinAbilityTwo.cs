using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RasputinAbilityTwo : AbilityTemplate
{
    [SerializeField] float healAmount = 20;
    [SerializeField] float healDuration = 1;


    public override void OnCreation()
    {
        if (audioOnCreate != null)
        {
            //play
            audioOnCreate.Play();
        }
        parent.GetComponent<CharacterTemplate>().effects.Add(new(false, healDuration, 0, (healAmount / healDuration), 0, 0, 0, 0, false));
    }

    public override void OnDestroy()
    {
        if (audioOnDestroy != null)
        {
            //play
            audioOnDestroy.Play();
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        //
    }
}
