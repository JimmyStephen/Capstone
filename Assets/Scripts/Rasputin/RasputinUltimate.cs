using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RasputinUltimate : AbilityTemplate
{
    [SerializeField] float firstReviveHealthValue = 50;
    [SerializeField] float secondReviveHealthValue = 25;

    private bool firstTrigger = false;
    private bool secondTrigger = false;
    private CharacterTemplate ct;

    public override void OnCreation()
    {
        if (audioOnCreate != null)
        {
            //play
            audioOnCreate.Play();
        }

        ct = parent.GetComponent<CharacterTemplate>();

        if(firstTrigger && !secondTrigger)
        {
            secondTrigger = true;
            //become immune
            ct.isImmune = true;
            ct.effectImmune = true;
            //set max hp & current
            ct.health.ChangeMax(firstReviveHealthValue);
            ct.health.ChangeCurrent(firstReviveHealthValue);
        }
        else
        {
            firstTrigger = true;
            //become immune
            ct.isImmune = true;
            ct.effectImmune = true;
            //set max hp & current
            ct.health.ChangeMax(secondReviveHealthValue);
            ct.health.ChangeCurrent(secondReviveHealthValue);
        }
        Quaternion rotation = parent.transform.rotation;
        rotation.x = -90;
        parent.transform.rotation = rotation;
    }

    public override void OnDestroy()
    {
        if (audioOnDestroy != null)
        {
            //play
            audioOnDestroy.Play();
        }

        //remove immunity
        ct.isImmune = false;
        ct.effectImmune = false;

        //reset rotation
        Quaternion rotation = parent.transform.rotation;
        rotation.x = 0;
        parent.transform.rotation = rotation;
    }

    public override void OnTriggerEnter(Collider other)
    {
        //no on trigger event
    }

    private void Update()
    {
        //rotate
        Quaternion rotation = parent.transform.rotation;
        rotation.x += (90 / destroyAfterSeconds) * Time.deltaTime;
        parent.transform.rotation = rotation;
    }

    public bool ActivateUlt()
    {
        if (secondTrigger && firstTrigger) return false;
        return true;
    }
}
