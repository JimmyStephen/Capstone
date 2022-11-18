using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RasputinTemplate : CharacterTemplate
{
    public override void BasicAttack()
    {
        if (CheckForStun()) { return; }

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
        if (CheckForStun()) { return; }

        AbilityTemplate at = abilityOneProjectile.GetComponent<AbilityTemplate>();
        if (!at.CanUse(health, energy, currentAbilityOneCooldown) || animationTimer >= 0)
        {
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
        //if (CheckForStun()) { return; }

        AbilityTemplate at = abilityTwoProjectile.GetComponent<AbilityTemplate>();
        if (!at.CanUse(health, energy, currentAbilityTwoCooldown) || animationTimer >= 0)
        {
            Debug.Log("Ability Cannot Be Used");
            return;
        }
        currentAbilityTwoCooldown = at.UseAbility(health, energy);

        animator.SetTrigger("Ability2");
        animationTimer = animationTwoDuration;

        StartCoroutine(SpawnAfterDelayParent(this.gameObject, abilityTwoProjectilePosition, abilityTwoProjectile, abilityTwoDelay));
    }

    public override void AbilityThree()
    {
        animationTimer = animationThreeDuration;
        animator.SetBool("Ability3", true);
        StartCoroutine(SpawnAfterDelayParent(this.gameObject, abilityThreeProjectilePosition, abilityThreeProjectile, abilityThreeDelay));
    }

    public override void OnDeath()
    {
        //check if his passive is avalible
        if(health.GetMax() > 25)
        {
            //if it is, use it
            AbilityThree();
        }
        else
        {
            //if it isnt
            //died
            //Debug.Log("YOU DIED!!!!");
            //set the winner to your opponent
            ///
            GameManager.Instance.SetWinner(opponent.GetComponent<CharacterTemplate>());
            GameManager.Instance.EndGame();
            //destroy this object
            Destroy(gameObject);
        }
    }
}
