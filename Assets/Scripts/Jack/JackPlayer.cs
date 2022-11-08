using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackPlayer : JackTemplate
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

        if (CheckForStun()) {
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

    //Input System
    public void OnJump()
    {
        if(CheckForStun()) { return; }
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
