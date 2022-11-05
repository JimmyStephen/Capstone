using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class JackTemplate : CharacterTemplate
{
    //maybe??
    //[SerializeField] Resource traps;
//    [SerializeField, Tooltip("The Main Body")] GameObject owner;

    public override void BasicAttack()
    {
        if (CheckForStun()) { return; }
        Debug.Log("Stabby stab");
        AbilityTemplate at = BasicAttackObject.GetComponent<AbilityTemplate>();
        if (!at.CanUse(health, energy, currentBasicAttackCooldown) || animationTimer >= 0)
        {
            Debug.Log("Ability Cannot Be Used");
            return;
        }
        currentBasicAttackCooldown = at.UseAbility(health, energy);
        animator.SetTrigger("Stab");
        animationTimer = basicAttackDuration;
        StartCoroutine(SpawnAfterDelayParent(this.gameObject, BasicAttackPosition, BasicAttackObject, basicAttackDelay));
    }
    public override void AbilityOne()
    {
        if (CheckForStun()) { return; }
        Debug.Log("Place Trap");

        AbilityTemplate at = abilityOneProjectile.GetComponent<AbilityTemplate>();
        if (!at.CanUse(health, energy, currentAbilityOneCooldown) || animationTimer >= 0)
        {
            Debug.Log("Ability Cannot Be Used");
            return;
        }
        currentAbilityOneCooldown = at.UseAbility(health, energy);

        animator.SetTrigger("PlaceTrap");
        animationTimer = animationOneDuration;

        StartCoroutine(SpawnAfterDelay(this.gameObject, abilityOneProjectilePosition, abilityOneProjectile, abilityOneDelay));
    }
    public override void AbilityTwo()
    {
        if (CheckForStun()) { return; }
        Debug.Log("Place Trap");

        AbilityTemplate at = abilityTwoProjectile.GetComponent<AbilityTemplate>();
        if (!at.CanUse(health, energy, currentAbilityTwoCooldown) || animationTimer >= 0)
        {
            Debug.Log("Ability Cannot Be Used");
            return;
        }
        currentAbilityTwoCooldown = at.UseAbility(health, energy);

        animator.SetTrigger("PlaceTrap");
        animationTimer = animationTwoDuration;

        StartCoroutine(SpawnAfterDelay(this.gameObject, abilityTwoProjectilePosition, abilityTwoProjectile, abilityTwoDelay));
    }
    public override void AbilityThree()
    {
        AbilityTemplate at = abilityThreeProjectile.GetComponent<AbilityTemplate>();

        if (!at.CanUse(health, energy, currentAbilityThreeCooldown))
        {
            Debug.Log("Not enough resources or it is on CD");
            return;
        }
        currentAbilityThreeCooldown = at.UseAbility(health, energy);

        animationTimer = animationThreeDuration;
        animator.SetTrigger("Ult");
        StartCoroutine(SpawnAfterDelay(this.gameObject, abilityThreeProjectilePosition, abilityThreeProjectile, abilityThreeDelay));
    }

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
}
