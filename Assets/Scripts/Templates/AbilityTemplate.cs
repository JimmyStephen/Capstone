using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
abstract public class AbilityTemplate : MonoBehaviour
{
    [Header("Requirements To Use This Ability")]
    [SerializeField] float HealthCost = 1;
    [SerializeField] float EnergyCost = 1;
    [SerializeField] float Cooldown = 1;

    [Header("Damage")]
    public float HealthDamage = 1;
    public float EnergyDamage = 1;

    [Header("Movement")]
    [SerializeField] float Speed = 1;

    [Header("Destruction")]
    public bool destroyOnHit = false;
    [SerializeField] bool destroyAfterDuration = false;
    [SerializeField] float destroyAfterSeconds = 1;

    [HideInInspector] public string shotFromTag;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 forceSpeed = Vector3.right * Speed;
        if (transform.rotation.z == 1)
        {
            forceSpeed *= -1;
        }
        rb.AddForce(forceSpeed, ForceMode.Force);
        if (destroyAfterDuration) Destroy(this.gameObject, destroyAfterSeconds);
        OnCreation();
    }

    /// <summary>
    /// Checks to make sure you have enough resources to use the ability and that it isnt on cooldown
    /// </summary>
    /// <param name="health">Your health component</param>
    /// <param name="energy">You energy component</param>
    /// <returns>If you can use the ability</returns>
    public bool canUse(Resource health, Resource energy, float currentCooldown)
    {
        return(health.GetCurrent() >= HealthCost && energy.GetCurrent() >= EnergyCost && currentCooldown <= 0);
    }
    /// <summary>
    /// subtracts the needed resources and then returns the cooldown of the ability
    /// </summary>
    /// <param name="health">Your health component</param>
    /// <param name="energy">You energy component</param>
    /// <returns>The cooldown of the ability</returns>
    public float useAbility(Resource health, Resource energy)
    {
        health.Damage(HealthCost);
        energy.Damage(EnergyCost);
        return Cooldown;
    }

    //What to do on collision
    abstract public void OnTriggerEnter(Collider other);
    //on creation
    abstract public void OnCreation();
    //on destruction
    abstract public void OnDestroy();
}
