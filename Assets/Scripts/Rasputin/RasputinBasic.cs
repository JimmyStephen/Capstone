using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RasputinBasic : AbilityTemplate
{
    public override void OnCreation()
    {
        if (audioOnCreate != null)
        {
            //play
            audioOnCreate.Play();
        }
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
        if (other.gameObject == parent) return;

        if (other.TryGetComponent<CharacterTemplate>(out CharacterTemplate ct))
        {
            if (ct.isImmune)
            {
                return;
            }
            //damage health
            float damageDealt = HealthDamage -= ct.resistanceFlat;
            //get the percent damage
            float tempPercent = ct.GetDamagePercentReduction();
            damageDealt *= tempPercent;
            if (damageDealt < 0) damageDealt = 0;
            ct.health.Damage(damageDealt * damageMultiplier);

            Destroy(this.gameObject, .05f);
        }
    }
}
