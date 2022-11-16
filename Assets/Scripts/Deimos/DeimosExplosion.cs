using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeimosExplosion : MonoBehaviour
{
    [SerializeField] float explosionDuration = 0;
    [SerializeField] float knockupForce = 0;
    [SerializeField] float stunDuration = 0;
    [SerializeField] float damage = 10;
    [SerializeField] AudioSource audioOnCreate;

    [HideInInspector] public GameObject owner;

    Effect stunEffect = null;

    void Start()
    {
        Destroy(gameObject, explosionDuration);

        if(stunDuration > 0 && knockupForce <= 0)
        {
            stunEffect = new(true, stunDuration, 0, 0, 0, 0, 0, 0, true);
        }else if(knockupForce > 0)
        {
            stunEffect = new(false, stunDuration, 0, 0, 0, 0, 0, 0, true);
        }

        if (audioOnCreate != null)
        {
            audioOnCreate.Play();
        }
    }

    bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == owner) return;
        if (triggered) return;

        if (other.TryGetComponent<CharacterTemplate>(out CharacterTemplate player))
        {
            if (!player.isImmune)
            {
                float damageDealt = damage -= player.resistanceFlat;
                float tempPercent = player.resistanceFlat != 0 ? 100 / player.resistanceFlat : 1;
                damageDealt *= tempPercent;
                if (damageDealt < 0) damageDealt = 0;
                player.health.Damage(damageDealt);

                if (!player.effectImmune && !player.CCImmune)
                {
                    player.effects.Add(stunEffect);
                }
            }
            triggered = true;
        }
    }
}
