using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtemisUltimate : AbilityTemplate
{

    [Header("Ultimate Inputs")]
    [SerializeField] GameObject onHitEffect;

    public override void OnCreation()
    {
        //do nothing
    }

    public override void OnDestroy()
    {
        var temp = Instantiate(onHitEffect, this.transform.position, this.transform.rotation);
        temp.GetComponent<ArtemisLightning>().owner = parent;
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == parent) return;
        //make sure the object you collide with doesnt share a parent
        if (other.gameObject.TryGetComponent<AbilityTemplate>(out AbilityTemplate at))
        {
            if (at.parent == parent) return;
        }

        if (other.tag == "Trap") return;

        if (other.TryGetComponent<CharacterTemplate>(out CharacterTemplate ct))
        {
            //damage health
            if (!ct.isImmune)
            {
                float damageDealt = HealthDamage -= ct.resistanceFlat;
                //get the percent damage
                float tempPercent = ct.resistanceFlat != 0 ? 100 / ct.resistanceFlat : 1;
                damageDealt *= tempPercent;
                if (damageDealt < 0) damageDealt = 0;
                ct.health.Damage(damageDealt * damageMultiplier);
                ct.energy.Damage(EnergyDamage);
            }
        }
        Destroy(this.gameObject, .1f);
    }
}
