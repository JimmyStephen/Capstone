using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect
{
    //How long it lasts
    private float duration = 1;
    //How much to damage/heal per second (0 for none)
    private readonly float healthDamagePerSec = 0;
    private readonly float healthHealingPerSec = 0;
    private readonly float energyDamagePerSec = 0;
    private readonly float energyHealingPerSec = 0;
    //How much to multiply stats by 1 is default
    private readonly float damageMultiplier = 1;
    private readonly float speedMultiplier = 1;
    //bonus effects
    private readonly bool stun = false;
    private readonly bool IsDebuff = false;

    /// <summary>
    /// What this effect does
    /// </summary>
    /// <param name="duration">How long this lasts</param>
    /// <param name="healthDamagePerSec">The healing per second</param>
    /// <param name="healthHealingPerSec">The damage per second</param>
    /// <param name="energyDamagePerSec">Energy gain per second</param>
    /// <param name="energyHealingPerSec">Energy damage per second</param>
    /// <param name="damageMultiplier">damage multiplier</param>
    /// <param name="speedMultiplier">speed multiplier</param>
    /// <param name="stun">if this effect stuns</param>
    public Effect(bool IsDebuff, float duration, float healthDamagePerSec, float healthHealingPerSec, float energyDamagePerSec, float energyHealingPerSec, float damageMultiplier, float speedMultiplier, bool stun)
    {
        this.IsDebuff = IsDebuff;
        this.duration = duration;
        this.healthDamagePerSec = healthDamagePerSec;
        this.healthHealingPerSec = healthHealingPerSec;
        this.energyDamagePerSec = energyDamagePerSec;
        this.energyHealingPerSec = energyHealingPerSec;
        this.damageMultiplier = damageMultiplier;
        this.speedMultiplier = speedMultiplier;
        this.stun = stun;
    }


    /// <summary>
    /// Call during every update, will effect health/energy as needed
    /// </summary>
    /// <param name="health">The health to effect</param>
    /// <param name="energy">Energy to effect</param>
    public void UpdateTrigger(Resource health, Resource energy)
    {
        //reduce the duration
        duration -= Time.deltaTime;
        health.Heal(healthHealingPerSec * Time.deltaTime);
        health.Damage(healthDamagePerSec * Time.deltaTime);
        energy.Heal(energyHealingPerSec * Time.deltaTime);
        energy.Damage(energyDamagePerSec * Time.deltaTime);
        Debug.Log("Remaining Duration: " + duration);
    }


    public float GetRemainingDuration()
    {
        return duration;
    }
    public float GetDamageMultipler()
    {
        return damageMultiplier;
    }
    public float GetSpeedMultiplier()
    {
        return speedMultiplier;
    }
    
    
    public bool IsStunned()
    {
        return stun;
    }
    public bool CheckIsDebuff()
    {
        return IsDebuff;
    }
}
