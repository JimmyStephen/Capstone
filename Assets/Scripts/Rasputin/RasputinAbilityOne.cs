using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RasputinAbilityOne : AbilityTemplate
{
    [SerializeField] float reducedDamage = .8f;
    [SerializeField] float effectDuration = 3;

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
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == parent) return;
        if (other.CompareTag("Trap")) return;

        if (other.TryGetComponent<CharacterTemplate>(out CharacterTemplate ct))
        {
            if (!ct.effectImmune)
            {
                Effect effect = new(true, effectDuration, 0, 0, 0, 0, reducedDamage, 0, false);
                ct.effects.Add(effect);
            }
            Destroy(this.gameObject, .05f);
        }
    }
}
