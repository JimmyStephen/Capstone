using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class CharacterTemplate : MonoBehaviour
{
    [HideInInspector] public bool playerOne = false;
    [Header("Animator")]
    public Animator animator;
    [Header("Movement")]
    public float speed = 1;
    public float jumpCD = 1;
    public float jumpCost = 1;
    [Header("Resources")]
    public Resource health;
    public Resource energy;
    [Header("Damage Resistance")]
    public float resistanceFlat = 0;
    [Tooltip("Enter a value from -100 (double dmg) - 100 (no dmg)"), Range(-100,100)]public float resistancePercent = 1;
    [Header("Projectiles")]
    public GameObject BasicAttackObject;
    public GameObject abilityOneProjectile;
    public GameObject abilityTwoProjectile;
    public GameObject abilityThreeProjectile;
    [Header("Transforms")]
    public GameObject BasicAttackPosition;
    public GameObject abilityOneProjectilePosition;
    public GameObject abilityTwoProjectilePosition;
    public GameObject abilityThreeProjectilePosition;
    [Header("Times")]
    public float basicAttackDuration = 0;
    public float animationOneDuration = 0;
    public float animationTwoDuration = 0;
    public float animationThreeDuration = 0;
    public float basicAttackDelay = 0;
    public float abilityOneDelay = 0;
    public float abilityTwoDelay = 0;
    public float abilityThreeDelay = 0;

    [HideInInspector] public CharacterController2D characterController;

    [HideInInspector] public List<Effect> effects = new List<Effect>();
    [HideInInspector] public float currentBasicAttackCooldown = 0;
    [HideInInspector] public float currentAbilityOneCooldown = 0;
    [HideInInspector] public float currentAbilityTwoCooldown = 0;
    [HideInInspector] public float currentAbilityThreeCooldown = 0;
    [HideInInspector] public float animationTimer = 0;

    [HideInInspector] public float currentDamageMultiplier = 1;
    [HideInInspector] public float currentSpeedMultiplier = 1;

    [HideInInspector] public bool isImmune = false;
    [HideInInspector] public bool CCImmune = false;
    [HideInInspector] public bool effectImmune = false;

    /*[HideInInspector]*/ public TMPro.TMP_Text HealthDisplay;
    /*[HideInInspector]*/ public TMPro.TMP_Text EnergyDisplay;

    //used by AI to find the opponent
    /*[HideInInspector]*/ public GameObject opponent;


    public IEnumerator SpawnAfterDelay(GameObject owner, GameObject location, GameObject spawnObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        bool right = owner.transform.rotation.y < 0;
        float angle = (right) ? 0 : 180;
        //Debug.Log("Angle: " + angle);
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);

        GameObject temp = Instantiate(spawnObject, location.transform.position, rotation);
        temp.GetComponent<AbilityTemplate>().parentTag = owner.transform.tag;
        temp.GetComponent<AbilityTemplate>().parent = owner;
    }

    public IEnumerator SpawnAfterDelayParent(GameObject owner, GameObject location, GameObject spawnObject, float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject temp = Instantiate(spawnObject, location.transform);
        temp.GetComponent<AbilityTemplate>().parentTag = owner.transform.tag;
        temp.GetComponent<AbilityTemplate>().parent = owner;
        temp.GetComponent<AbilityTemplate>().damageMultiplier = owner.GetComponent<CharacterTemplate>().currentDamageMultiplier;
    }

    public void TriggerEffects()
    {
        currentDamageMultiplier = 1;
        currentSpeedMultiplier = 1;

        List<Effect> eRemove = new List<Effect>();
        foreach(Effect e in effects)
        {
            e.updateTrigger(health, energy);
            if (e.getRemainingDuration() > 0)
            {
                currentDamageMultiplier = (currentDamageMultiplier + e.getDamageMultipler() > 0) ? currentDamageMultiplier += e.getDamageMultipler() : .01f;
                currentSpeedMultiplier = (currentSpeedMultiplier + e.getSpeedMultiplier() > 0) ? currentSpeedMultiplier += e.getSpeedMultiplier() : .01f;
            }
            else
            {
                eRemove.Add(e);
            }
        }

        foreach(Effect e in eRemove)
        {
            effects.Remove(e);
        }
    }

    public void setDisplay(TMPro.TMP_Text healthDisplay, TMPro.TMP_Text energyDisplay)
    {
        HealthDisplay = healthDisplay;
        EnergyDisplay = energyDisplay;
    }
    //Abstract

    /// <summary>
    /// What will happen when you die
    /// </summary>
    abstract public void OnDeath();

    /// <summary>
    /// What will happen when you press the button for the basic attack
    /// </summary>
    abstract public void BasicAttack();

    /// <summary>
    /// What will happen when you press the button for the primary ability
    /// </summary>
    abstract public void AbilityOne();
    /// <summary>
    /// What will happen when you press the button for your secondary ability
    /// </summary>
    abstract public void AbilityTwo();
    /// <summary>
    /// What will happen when you press the button for your ultimate ability
    /// </summary>
    abstract public void AbilityThree();

    /// <summary>
    /// Stuff that MUST happen every update
    /// </summary>
    abstract public void CharacterRequiredUpdates();
}
