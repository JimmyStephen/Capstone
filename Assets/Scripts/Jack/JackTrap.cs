using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackTrap : MonoBehaviour
{
    [Header("Settup")]
    [SerializeField] float setupDuration = 0;
    [SerializeField] bool destroyAfterDuration = false;
    [SerializeField] float trapDuration = 0;


    [Header("If something happens")]
    [SerializeField] bool applyDot = false;
    [SerializeField] bool applySlow = false;
    [SerializeField] bool applyStun = false;

    [Header("How long the durations are")]
    [SerializeField] float dotDuration = 0;
    [SerializeField] float slowDuration = 0;
    [SerializeField] float stunDuration = 0;

    [Header("How bad the effects are")]
    [SerializeField] float dotTotalDamage = 0;
    [SerializeField] float slowPercent = 0;

    [Header("What to do when the trap triggers")]
    [SerializeField] GameObject createOnTrigger;
    [SerializeField] AudioSource audioOnTrigger;

    //who the owner of the trap is
    [HideInInspector] public GameObject owner;

    private readonly List<Effect> effectsToApply = new();

    private void Start()
    {
        //ready effects & add to effectsToApply

        //dot (if needed)
        if (applyDot)
        {
            effectsToApply.Add(new Effect(true, dotDuration, (dotTotalDamage / dotDuration), 0, 0, 0, 0, 0, false));
        }
        //slow (if needed)
        if (applySlow)
        {
            effectsToApply.Add(new Effect(true, slowDuration, 0, 0, 0, 0, 0, slowPercent, false));
        }
        //stun (if needed)
        if (applyStun)
        {
            effectsToApply.Add(new Effect(true, stunDuration, 0, 0, 0, 0, 0, 0, true));
        }

        if (destroyAfterDuration) Destroy(this.gameObject, trapDuration);
    }

    private void Update()
    {
        if (setupDuration > 0)
        {
            setupDuration -= Time.deltaTime;
//            if (setupDuration <= 0) Debug.Log("Trap <" + this.gameObject.name + "> Has been set");
        }
        else
        {
            setupDuration = 0;
        }
    }

    bool triggered = false;
    private void OnTriggerStay(Collider other)
    {
        //make sure its a valid target
        //apply the effects
        //destroy the trap
        if (triggered) return;
        if (other.gameObject == owner) return;
        if (setupDuration > 0) return;

        if (other.TryGetComponent<CharacterTemplate>(out CharacterTemplate player))
        {
            if (!player.isImmune)
            {
                if (!player.effectImmune && !player.CCImmune)
                {
                    foreach(Effect effect in effectsToApply)
                    {
                        player.effects.Add(effect);
                    }
                }
            }
            if (createOnTrigger != null) Instantiate(createOnTrigger, transform.position, transform.rotation);
            if (audioOnTrigger != null) audioOnTrigger.Play();
            triggered = true;
            Destroy(this.gameObject, 1);
        }
    }
}
