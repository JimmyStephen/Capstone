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

        if(!firstTrigger && !secondTrigger)
        {
            firstTrigger = true;
            //become immune
            ct.isImmune = true;
            ct.effectImmune = true;
            //set max hp & current
            ct.health.ChangeMax(firstReviveHealthValue);
            //ct.health.ChangeCurrent(firstReviveHealthValue);
            ct.effects.Add(new(false, destroyAfterSeconds, 0, 7.5f, 0, 0, 1, 1, false));
            CleanseDebuff();
            Debug.Log("Max HP set to: " + firstReviveHealthValue);
        }
        else
        {
            secondTrigger = true;
            //become immune
            ct.isImmune = true;
            ct.effectImmune = true;
            //set max hp & current
            ct.health.ChangeMax(secondReviveHealthValue);
//            ct.health.ChangeCurrent(secondReviveHealthValue);
            ct.effects.Add(new(false, destroyAfterSeconds, 0, 5f, 0, 0, 1, 1, false));
            CleanseDebuff();
            Debug.Log("Max HP set to: " + secondReviveHealthValue);
        }
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

        //reset animator
        ct.animator.SetBool("Ability3", false);
    }

    public override void OnTriggerEnter(Collider other)
    {
        //no on trigger event
    }

    public bool ActivateUlt()
    {
        if (secondTrigger && firstTrigger) return false;
        return true;
    }

    private void CleanseDebuff()
    {
        List<Effect> effects = new();
        foreach (var e in ct.effects)
        {
            if (e.CheckIsDebuff())
            {
                effects.Add(e);
            }
        }
        foreach (var e in effects)
        {
            ct.effects.Remove(e);
        }
    }
}
