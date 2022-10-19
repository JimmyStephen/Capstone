using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtemisSecondary : AbilityTemplate
{
    public override void OnCreation()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnDestroy()
    {
        //throw new System.NotImplementedException();
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == parent) return;

        if (other.TryGetComponent<CharacterTemplate>(out CharacterTemplate ct))
        {
            //damage health
            float damageDealt = HealthDamage -= ct.resistanceFlat;
            //get the percent damage
            float tempPercent = ct.resistanceFlat != 0 ? 100 / ct.resistanceFlat : 1;
            damageDealt *= tempPercent;
            if (damageDealt < 0) damageDealt = 0;
            ct.health.Damage(damageDealt * damageMultiplier);
            ct.energy.Damage(EnergyDamage);
        }
        
        Destroy(this.gameObject, .05f);
    }
}
