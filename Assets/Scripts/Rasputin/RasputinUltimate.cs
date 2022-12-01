using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RasputinUltimate : AbilityTemplate
{
    [SerializeField] float firstReviveHealthValue = 50;
    [SerializeField] float secondReviveHealthValue = 25;

    [SerializeField] AudioSource firstReviveAudio;

    private CharacterTemplate ct;

    public override void OnCreation()
    {
        ct = parent.GetComponent<CharacterTemplate>();

        if(ct.health.GetMax() == 75)
        {
            //become immune
            ct.isImmune = true;
            ct.effectImmune = true;
            //set max hp & current
            ct.health.ChangeMax(firstReviveHealthValue);
            //ct.health.ChangeCurrent(firstReviveHealthValue);
            ct.effects.Add(new(false, destroyAfterSeconds, 0, (firstReviveHealthValue / destroyAfterSeconds), 0, 0, 1, 1, false));
            CleanseDebuff();
            if (firstReviveAudio != null)
            {
                //play
                firstReviveAudio.Play();
            }
        }
        else if(ct.health.GetMax() == 50)
        {
            //become immune
            ct.isImmune = true;
            ct.effectImmune = true;
            //set max hp & current
            ct.health.ChangeMax(secondReviveHealthValue);
//            ct.health.ChangeCurrent(secondReviveHealthValue);
            ct.effects.Add(new(false, destroyAfterSeconds, 0, (secondReviveHealthValue / destroyAfterSeconds), 0, 0, 1, 1, false));
            ct.effects.Add(new(false, 1000, 0, 0, 0, 0, 2, 1, false));
            CleanseDebuff();
            //play audio
            AudioManager.Instance.ChangeBackgroundAudio(3);
        }
        else
        {
            Debug.Log("How???");
        }
    }

    public override void OnDestroy()
    {
        //remove immunity
        ct.isImmune = false;
        ct.effectImmune = false;

        ct.health.ChangeCurrent(100);

        //reset animator
        ct.animator.SetBool("Ability3", false);
    }

    public override void OnTriggerEnter(Collider other)
    {
        //no on trigger event
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
