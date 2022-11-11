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
        //
        if(audioOnDestroy != null)
        {
            //play
            audioOnDestroy.Play();
        }
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
            float tempPercent = ct.resistanceFlat != 0 ? 100 / ct.resistanceFlat : 1;
            damageDealt *= tempPercent;
            if (damageDealt < 0) damageDealt = 0;
            ct.health.Damage(damageDealt * damageMultiplier);

            //knockback
            ct.characterController.ForcedMove(ct.characterController.GetDirection() ? -knockbackForce : knockbackForce);
            Destroy(this.gameObject, .05f);
        }
    }
}
