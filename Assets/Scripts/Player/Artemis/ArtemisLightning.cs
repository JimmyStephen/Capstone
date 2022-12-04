using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtemisLightning : MonoBehaviour
{
    [SerializeField, Tooltip("Storm Duration")] float stormDuration = 0;
    [SerializeField, Tooltip("Damage to health")] float healthDamage = 0;
    [SerializeField, Tooltip("Damage to energy")] float energyDamage = 0;
    [SerializeField, Tooltip("How long to stun")] float stunDuration = 0;
    [HideInInspector] public GameObject owner;
    [SerializeField] AudioSource audioOnCreate;

    private void Start()
    {
        Destroy(gameObject, stormDuration);
        if(audioOnCreate != null)
        {
            audioOnCreate.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == owner) return;

        if (other.TryGetComponent<CharacterTemplate>(out CharacterTemplate player))
        {
            if (!player.isImmune)
            {
                //damage health
                float damageDealt = healthDamage -= player.resistanceFlat;
                //get the percent damage
                float tempPercent = player.GetDamagePercentReduction();
                damageDealt *= tempPercent;
                if (damageDealt < 0) damageDealt = 0;
                player.health.Damage(damageDealt);
                player.energy.Damage(energyDamage);
                if (!player.effectImmune && !player.CCImmune)
                {
                    player.effects.Add(new Effect(true, stunDuration, 0, 0, 0, 0, 1, 1, true));
                }
            }
        }
        //Destroy(this.gameObject, .25f);
    }
}
