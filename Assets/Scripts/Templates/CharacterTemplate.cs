using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterTemplate : MonoBehaviour
{
    [Header("Animator")]
    public  Animator animator;
    [Header("Movement")]
    public float speed = 1;
    public float jumpCD = 1;
    public float jumpCost = 1;
    [Header("Resources")]
    public Resource health;
    public Resource energy;
    [Header("Damage Resistance")]
    public float resistanceFlat;
    public float resistancePercent;
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
    public float animationOneDuration = 0;
    public float animationTwoDuration = 0;
    public float animationThreeDuration = 0;
    public float abilityOneDelay = 0;
    public float abilityTwoDelay = 0;
    public float abilityThreeDelay = 0;

    [HideInInspector] public float currentBasicAttackCooldown = 0;
    [HideInInspector] public float currentAbilityOneCooldown = 0;
    [HideInInspector] public float currentAbilityTwoCooldown = 0;
    [HideInInspector] public float currentAbilityThreeCooldown = 0;
    [HideInInspector] public float animationTimer = 0;

    public IEnumerator SpawnAfterDelay(GameObject owner, GameObject location, GameObject spawnObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        bool right = owner.transform.localScale.x > 0;
        float angle = (right) ? 0 : 180;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        GameObject temp = Instantiate(spawnObject, location.transform.position, rotation);
        temp.GetComponent<AbilityTemplate>().shotFromTag = owner.transform.tag;
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
}
