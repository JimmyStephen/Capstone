using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtemisSecondary : AbilityTemplate
{
    public override void OnCreation()
    {
        //throw new System.NotImplementedException();
        //
        if (audioOnCreate != null)
        {
            //play
            audioOnCreate.Play();
        }
    }

    public override void OnDestroy()
    {
        //throw new System.NotImplementedException();
        //
        if (audioOnDestroy != null)
        {
            //play
            audioOnDestroy.Play();
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == parent) return;
        //make sure the object you collide with doesnt share a parent
        if (other.gameObject.TryGetComponent<AbilityTemplate>(out AbilityTemplate at))
        {
            if (at.parent == parent) return;
        }
        if (other.CompareTag("Trap")) return;

        //Debug.Log(name + " collided with " + other.name + " parent is: " + parent.name);

        if (other.TryGetComponent<CharacterTemplate>(out CharacterTemplate ct))
        {
            //damage health
            float damageDealt = HealthDamage -= ct.resistanceFlat;
            //get the percent damage
            float tempPercent = ct.GetDamagePercentReduction();
            damageDealt *= tempPercent;
            if (damageDealt < 0) damageDealt = 0;
            ct.health.Damage(damageDealt * damageMultiplier);
            ct.energy.Damage(EnergyDamage);
        }
        
        Destroy(this.gameObject, .05f);
    }
}
