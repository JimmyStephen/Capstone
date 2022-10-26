using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Artemis : CharacterTemplate
{
    private float currentJumpCD = 0;
    Vector3 direction = Vector3.zero;
    bool jump = false;

    void Start()
    {
        characterController = GetComponent<CharacterController2D>();
    }
    void Update()
    {
        CharacterRequiredUpdates();        

        foreach(Effect effect in effects)
        {
            if (effect.isStunned())
            {
                characterController.Move(0, false, false);
                return;
            }
        }
        
        if(animationTimer >= 0)
        {
            //in animation don't move
            characterController.Move(0, false, false);
            return;
        }

        direction.x = Input.GetAxis("Horizontal") * speed;
        characterController.Move(direction.x, false, jump);
        animator.SetFloat("Speed", Mathf.Abs(direction.x));
        jump = false;
    }

    //on death
    public override void OnDeath()
    {
//        animator.SetTrigger("Dead");
    }

    //Abilities
    public override void BasicAttack()
    {
        Debug.Log("Basic Attack Activated");

        AbilityTemplate at = BasicAttackObject.GetComponent<AbilityTemplate>();
        if (!at.canUse(health, energy, currentBasicAttackCooldown) || animationTimer >= 0)
        {
            Debug.Log("Ability Cannot Be Used");
            return;
        }
        currentBasicAttackCooldown = at.useAbility(health, energy);
        animator.SetTrigger("Basic");
        animationTimer = basicAttackDuration;
        StartCoroutine(SpawnAfterDelayParent(this.gameObject, BasicAttackPosition, BasicAttackObject, basicAttackDelay));
    }
    public override void AbilityOne()
    {
        //dodge roll
        Debug.Log("Ability 1 Activated");

        AbilityTemplate at = abilityOneProjectile.GetComponent<AbilityTemplate>();
        if(!at.canUse(health, energy, currentAbilityOneCooldown) || animationTimer >= 0){
            Debug.Log("Ability Cannot Be Used");
            return;
        }
        currentAbilityOneCooldown = at.useAbility(health, energy);

        animator.SetTrigger("Ability1");
        animationTimer = animationOneDuration;

        StartCoroutine(SpawnAfterDelay(this.gameObject, abilityOneProjectilePosition, abilityOneProjectile, abilityOneDelay));
    }
    public override void AbilityTwo()
    {
        Debug.Log("Ability 2 Activated");
        AbilityTemplate at = abilityTwoProjectile.GetComponent<AbilityTemplate>();

        if (!at.canUse(health, energy, currentAbilityTwoCooldown) || animationTimer >= 0)
        {
            Debug.Log("Ability Cannot Be Used");
            return;
        }
        currentAbilityTwoCooldown = at.useAbility(health, energy);

        animator.SetTrigger("Ability2");
        animationTimer = animationTwoDuration;
        StartCoroutine(SpawnAfterDelay(this.gameObject, abilityTwoProjectilePosition, abilityTwoProjectile, abilityTwoDelay));

    }
    public override void AbilityThree()
    {
        Debug.Log("Ultimate Ability Activated");

        AbilityTemplate at = abilityThreeProjectile.GetComponent<AbilityTemplate>();

        if (!at.canUse(health, energy, currentAbilityThreeCooldown))
        {
            Debug.Log("Not enough resources or it is on CD");
            return;
        }
        currentAbilityThreeCooldown = at.useAbility(health, energy);

        animationTimer = animationThreeDuration;
        animator.SetTrigger("Ability3");
        StartCoroutine(SpawnAfterDelay(this.gameObject, abilityThreeProjectilePosition, abilityThreeProjectile, abilityThreeDelay));
    }

    //Input System
    public void OnJump()
    {
        if (!energy.CheckEnoughResource(jumpCost))
        {
            Debug.Log("Not enough energy");
            return;
        }
        if (currentJumpCD > 0)
        {
            Debug.Log("Jump is on CD");
            return;
        }
        energy.Damage(jumpCost);
        currentJumpCD = jumpCD;
        jump = true;
    }
    public void OnBasicAbility() { BasicAttack(); }
    public void OnAbilityOne() { AbilityOne(); }
    public void OnAbilityTwo() { AbilityTwo(); }
    public void OnUltimateAbility() { AbilityThree(); }

    public override void CharacterRequiredUpdates()
    {
        //reduce CD
        currentJumpCD -= Time.deltaTime;
        currentBasicAttackCooldown -= Time.deltaTime;
        currentAbilityOneCooldown -= Time.deltaTime;
        currentAbilityTwoCooldown -= Time.deltaTime;
        currentAbilityThreeCooldown -= Time.deltaTime;
        animationTimer -= Time.deltaTime;

        TriggerEffects();

        HealthDisplay.SetText("Health: " + health.GetCurrent().ToString("F0"));
        EnergyDisplay.SetText("Energy: " + energy.GetCurrent().ToString("F0"));

        if (health.GetCurrent() <= 0)
        {
            OnDeath();
        }
    }
}
