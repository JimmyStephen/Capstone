using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : CharacterTemplate
{
    [SerializeField] TMPro.TMP_Text HealthDisplay;
    [SerializeField] TMPro.TMP_Text EnergyDisplay;

    private float currentJumpCD = 0;
    private CharacterController2D characterController;

    Vector3 direction = Vector3.zero;
    bool jump = false;

    void Start()
    {
        characterController = GetComponent<CharacterController2D>();
    }
    void Update()
    {
        currentJumpCD -= Time.deltaTime;
        currentAbilityOneCooldown -= Time.deltaTime;
        currentAbilityTwoCooldown -= Time.deltaTime;
        currentAbilityThreeCooldown -= Time.deltaTime; 
        animationTimer -= Time.deltaTime;
        
        HealthDisplay.SetText("Health: " + health.GetCurrent().ToString());
        EnergyDisplay.SetText("Energy: " + energy.GetCurrent().ToString());

        if (health.GetCurrent() <= 0)
        {
            OnDeath();
            return;
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
        Debug.Log("You Died!, now reseting health");
//        animator.SetTrigger("Dead");
        health.Heal(100);
    }

    //Abilities
    public override void BasicAttack()
    {
        Debug.Log("Use basic attack");
        animator.SetTrigger("Basic");
    }
    public override void AbilityOne()
    {
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

        //Not using delay because it is instant
        bool right = transform.localScale.x > 0;
        float angle = (right) ? 0 : 180;

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        GameObject temp = Instantiate(abilityThreeProjectile, abilityTwoProjectilePosition.transform.position, rotation);
        temp.GetComponent<AbilityTemplate>().shotFromTag = tag;
        //temp.GetComponent<AbilityThree>().setOwner(this.gameObject);

        animationTimer = animationThreeDuration;
        animator.SetTrigger("Ability3");
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
}
