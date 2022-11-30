using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeimosAI : DeimosTemplate
{
    [SerializeField] float highToMediumPercent;
    [SerializeField] float mediumToLowPercent;

    private HealthStateTemplate currentState;
    private DeimosHighHealth HighHealth;
    private DeimosMediumHealth MedHealth;
    private DeimosLowHealth LowHealth;

    private float healthPercent;
    bool attack = true;
    float attackTimer = 0;

    void Start()
    {        
        characterController = GetComponent<CharacterController2D>();

        //make combat states
        DeimosAggressive Aggressive = new(this, typeof(DeimosAggressive).Name);
        //DeimosDefensive Defensive = new(this,   typeof(DeimosDefensive).Name);
        DeimosPassive Passive = new(this,       typeof(DeimosPassive).Name);
        //make health states
        HighHealth = new DeimosHighHealth(this,     typeof(DeimosHighHealth).Name, new State[] { Aggressive, Passive });
        MedHealth  = new DeimosMediumHealth(this,   typeof(DeimosMediumHealth).Name, new State[] { Aggressive, Passive });
        LowHealth  = new DeimosLowHealth(this,      typeof(DeimosLowHealth).Name, new State[] { Aggressive, Passive });
        //run the create methods
        HighHealth.OnCreate();
        MedHealth.OnCreate();
        LowHealth.OnCreate();

        //set default state
        currentState = HighHealth;
    }

    void Update()
    {
        StateUpdates();
        CharacterRequiredUpdates();

        //check for animation
        if (animationTimer >= 0)
        {
            //Debug.Log("Timer Active");
            animator.SetFloat("Speed", 0);
            return;
        }

        if (attack)
        {
            //check for ability
            UseAbility();
        }
        else
        {
            //check for stun
            if (CheckForStun())
            {
                AbilityThree();
                characterController.Move(0, false, false);
                animator.SetFloat("Speed", 0);
                return;
            }
            //check for movement
            AIMovement();
        }

        if (attackTimer < 0)
        {
            if (attack)
            {
                attackTimer = 2;
            }
            else
            {
                attackTimer = .5f;
            }
            attack = !attack;
        }
    }

    public override void CharacterRequiredUpdates()
    {
        //reduce CD
        //currentJumpCD -= Time.deltaTime;
        currentBasicAttackCooldown -= Time.deltaTime;
        currentAbilityOneCooldown -= Time.deltaTime;
        currentAbilityTwoCooldown -= Time.deltaTime;
        currentAbilityThreeCooldown -= Time.deltaTime;
        animationTimer -= Time.deltaTime;
        attackTimer -= Time.deltaTime;

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

    /// <summary>
    /// Moves the AI as needed
    /// </summary>
    private void AIMovement()
    {
        if (CheckForStun()) return;
        if (animationTimer > 0) return;

        float movement = currentState.StateMovement() * speed * currentSpeedMultiplier;
        bool shouldJump = false;
        if (characterController.m_Grounded)
        {
            shouldJump = currentState.ShouldJump();
        }

        animator.SetFloat("Speed", Mathf.Abs(movement));
        characterController.Move(movement, false, shouldJump);
    }

    /// <summary>
    /// Updates the state and checks if it needs to change
    /// </summary>
    private void StateUpdates()
    {
        //Debug.Log("Current State: " + currentState.Name);
        healthPercent = (health.GetCurrent() / health.GetMax()) * 100;
        //to high health
        if (healthPercent > highToMediumPercent && currentState != HighHealth)
        {
            currentState.OnExit();
            currentState = HighHealth;
            currentState.OnEnter();
        }

        //to medium health
        if (healthPercent < highToMediumPercent && healthPercent > mediumToLowPercent && currentState != MedHealth)
        {
            currentState.OnExit();
            currentState = MedHealth;
            currentState.OnEnter();
        }

        //to low health
        if (healthPercent < mediumToLowPercent && currentState != LowHealth)
        {
            currentState.OnExit();
            currentState = LowHealth;
            currentState.OnEnter();
        }

        currentState.OnUpdate();
    }

    /// <summary>
    /// Checks if you need to use an ability, then calls the respective functions
    /// </summary>
    private void UseAbility()
    {
        int ability = currentState.UseAbility();
        switch (ability)
        {
            case 0:
                //        Debug.Log("Use basic ability");
                //animationTimer = .5f;
                BasicAttack();
                break;
            case 1:
                //          Debug.Log("Use ability 1");
                //animationTimer = .5f;
                AbilityOne();
                break;
            case 2:
                //            Debug.Log("Use ability 2");
                //animationTimer = .5f;
                AbilityTwo();
                break;
            case 3:
                //              Debug.Log("Use ability three");
                //animationTimer = .5f;
                AbilityThree();
                break;
            default:
                //                Debug.Log("No Ability Used");
                break;
        }
    }
}
