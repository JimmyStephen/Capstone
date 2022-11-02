using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackBasic : AbilityTemplate
{
    //deals slight damage and applies a poison

    public override void OnCreation()
    {
  //      throw new System.NotImplementedException();
    }

    public override void OnDestroy()
    {
//        throw new System.NotImplementedException();
    }

    public override void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided With: " + other.gameObject.name + "Owner: " + parent.name);
        if (other.gameObject == parent) return;
        //knockback the target
        if (other.TryGetComponent<CharacterTemplate>(out CharacterTemplate ct))
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

            //weak poison (5dmg)
            ct.effects.Add(new Effect(5, 1, 0, 0, 0, 0, 0, false));
            Destroy(this.gameObject, .05f);
        }
    }
}
