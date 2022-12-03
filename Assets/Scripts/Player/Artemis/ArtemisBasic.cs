using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtemisBasic : AbilityTemplate
{
    [SerializeField] float knockbackForce = 0;
    public override void OnCreation()
    {
        //
        if(audioOnCreate != null)
        {
            //play
            audioOnCreate.Play();
        }
    }

    public override void OnDestroy()
    {
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == parent) return;
        //knockback the target
        if(other.TryGetComponent<CharacterTemplate>(out CharacterTemplate ct))
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

            //knockback
            ct.effects.Add(new(false, 1, 0, 0, 0, 0, 0, 0, true));
            ct.characterController.ForcedMove(ct.characterController.GetDirection() ? -knockbackForce : knockbackForce, 0);
            Destroy(this.gameObject, .05f);
        }
    }
}
