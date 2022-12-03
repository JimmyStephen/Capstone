using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RasputinPlayer : RasputinTemplate
{
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

    public void OnJump()
    {
        if (CheckForStun()) { return; }
        jump = true;
    }
    public void OnBasicAbility() { BasicAttack(); }
    public void OnAbilityOne() { AbilityOne(); }
    public void OnAbilityTwo() { AbilityTwo(); }
    public void OnUltimateAbility() { Debug.Log("Rasputin Ult is a Passive"); }

    public override void CharacterRequiredUpdates()
    {
        //Figure out a way so that drinking doesn't make him immortal
            //maybe checking hp before triggering effects?

        //reduce CD
        currentBasicAttackCooldown -= Time.deltaTime;
        currentAbilityOneCooldown -= Time.deltaTime;
        currentAbilityTwoCooldown -= Time.deltaTime;
        animationTimer -= Time.deltaTime;

        TriggerEffects();

        if (health.GetCurrent() <= 0)
        {
            OnDeath();
        }

        if (HealthSlider != null)
        {
            HealthSlider.size = health.GetCurrent() / health.GetMax();
        }
        if (EnergySlider != null)
        {
            EnergySlider.size = energy.GetCurrent() / energy.GetMax();
        }
    }
}
