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

        if (CheckForStun())
        {
            characterController.Move(0, false, false);
            animator.SetFloat("Speed", 0);
            return;
        }

        if (animationTimer >= 0)
        {
            //in animation don't move
            characterController.Move(0, false, false);
            animator.SetFloat("Speed", 0);
            return;
        }

        direction.x = Input.GetAxis("Horizontal") * speed * currentSpeedMultiplier;
        characterController.Move(direction.x, false, jump);
        animator.SetFloat("Speed", Mathf.Abs(direction.x));
        jump = false;
    }

    //on death
    public override void OnDeath()
    {
        //died
        Debug.Log("YOU DIED!!!!");
        //set the winner to your opponent
        ///
        GameManager.Instance.SetWinner(opponent.GetComponent<CharacterTemplate>());
        GameManager.Instance.EndGame();
        //destroy this object
        Destroy(gameObject);
    }

    //Abilities
    public override void BasicAttack()
    {
        //Debug.Log("Basic Attack Activated");
        if (CheckForStun())
        {
            return;
        }

        AbilityTemplate at = BasicAttackObject.GetComponent<AbilityTemplate>();
        if (!at.CanUse(health, energy, currentBasicAttackCooldown) || animationTimer >= 0)
        {
            Debug.Log("Ability Cannot Be Used");
            return;
        }
        currentBasicAttackCooldown = at.UseAbility(health, energy);
        animator.SetTrigger("Basic");
        animationTimer = basicAttackDuration;
        StartCoroutine(SpawnAfterDelayParent(this.gameObject, BasicAttackPosition, BasicAttackObject, basicAttackDelay));
    }
    public override void AbilityOne()
    {
        if (CheckForStun())
        {
            return;
        }
        //dodge roll
        Debug.Log("Ability 1 Activated");

        AbilityTemplate at = abilityOneProjectile.GetComponent<AbilityTemplate>();
        if(!at.CanUse(health, energy, currentAbilityOneCooldown) || animationTimer >= 0){
            Debug.Log("Ability Cannot Be Used");
            return;
        }
        currentAbilityOneCooldown = at.UseAbility(health, energy);

        animator.SetTrigger("Ability1");
        animationTimer = animationOneDuration;

        StartCoroutine(SpawnAfterDelay(this.gameObject, abilityOneProjectilePosition, abilityOneProjectile, abilityOneDelay));
    }
    public override void AbilityTwo()
    {
        if (CheckForStun())
        {
            return;
        }
        Debug.Log("Ability 2 Activated");
        AbilityTemplate at = abilityTwoProjectile.GetComponent<AbilityTemplate>();

        if (!at.CanUse(health, energy, currentAbilityTwoCooldown) || animationTimer >= 0)
        {
            Debug.Log("Ability Cannot Be Used");
            return;
        }
        currentAbilityTwoCooldown = at.UseAbility(health, energy);

        animator.SetTrigger("Ability2");
        animationTimer = animationTwoDuration;
        StartCoroutine(SpawnAfterDelay(this.gameObject, abilityTwoProjectilePosition, abilityTwoProjectile, abilityTwoDelay));

    }
    public override void AbilityThree()
    {
        if (CheckForStun())
        {
            return;
        }

        AbilityTemplate at = abilityThreeProjectile.GetComponent<AbilityTemplate>();

        if (!at.CanUse(health, energy, currentAbilityThreeCooldown))
        {
            Debug.Log("Not enough resources or it is on CD");
            return;
        }
        currentAbilityThreeCooldown = at.UseAbility(health, energy);

        animationTimer = animationThreeDuration;
        animator.SetTrigger("Ability3");
        StartCoroutine(SpawnAfterDelay(this.gameObject, abilityThreeProjectilePosition, abilityThreeProjectile, abilityThreeDelay));
    }

    //Input System
    public void OnJump()
    {
        if (CheckForStun())
        {
            return;
        }
        if (currentJumpCD > 0)
        {
            Debug.Log("Jump is on CD");
            return;
        }
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

        if (HealthDisplay != null)
        {
            HealthDisplay.SetText("Health: " + health.GetCurrent().ToString("F0"));
        }
        if (EnergyDisplay != null)
        {
            EnergyDisplay.SetText("Energy: " + energy.GetCurrent().ToString("F0"));
        }
        if (HealthSlider != null)
        {
            HealthSlider.size = health.GetCurrent() / health.GetMax();
        }
        if (EnergySlider != null)
        {
            EnergySlider.size = energy.GetCurrent() / energy.GetMax();
        }

        if (health.GetCurrent() <= 0)
        {
            OnDeath();
        }
    }
}
