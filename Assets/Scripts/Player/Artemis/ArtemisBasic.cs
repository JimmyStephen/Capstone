using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtemisBasic : AbilityTemplate
{
    [SerializeField] float knockbackForce = 0;
    public override void OnCreation()
    {
        //
    }

    public override void OnDestroy()
    {
        //
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == parent) return;
        //knockback the target
        if(other.TryGetComponent<CharacterTemplate>(out CharacterTemplate ct))
        {
            //damage health
            float damageDealt = HealthDamage -= ct.resistanceFlat;
            damageDealt -= damageDealt * ct.resistancePercent;
            if (damageDealt < 0) damageDealt = 0;
            ct.health.Damage(damageDealt * damageMultiplier);
            //knockback
            Debug.Log("knockback here... (help)");
            //ct.characterController.Dash((ct.characterController.GetDirection() ? knockbackForce : -knockbackForce));
        }
    }
}
