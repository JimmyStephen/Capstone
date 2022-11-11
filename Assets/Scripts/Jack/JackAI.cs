using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackAI : JackTemplate
{
    [SerializeField] float highToMediumPercent;
    [SerializeField] float mediumToLowPercent;

    private HealthStateTemplate currentState;

    private JackHighHealth HighHealth;
    private JackMediumHealth MedHealth;
    private JackLowHealth LowHealth;

    private float healthPercent;
    bool attack = true;
    float attackTimer = 0;

    private void Start()
    {
        characterController = GetComponent<CharacterController2D>();

        //make combat states
        JackAggressive Aggressive =    new (this, typeof(JackAggressive).Name);
        JackDefensive Defensive =      new (this, typeof(JackDefensive).Name);
        JackPassive Passive =          new (this, typeof(JackPassive).Name);
        //make health states
        HighHealth = new JackHighHealth(this, typeof(JackHighHealth).Name, new State[] { Aggressive, Defensive, Passive });
        MedHealth = new  JackMediumHealth(this, typeof(JackMediumHealth).Name, new State[] { Aggressive, Defensive, Passive });
        LowHealth = new JackLowHealth(this, typeof(JackLowHealth).Name, new State[] { Aggressive, Defensive, Passive });
        //run the create methods
        HighHealth.OnCreate();
        MedHealth.OnCreate();
        LowHealth.OnCreate();

        //set default state
        currentState = HighHealth;
    }

    private void Update()
    {
        //Debug.Log("Current State: " + currentState.Name + " Inner State: " + currentState.sMachine.currentState.Name);
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
                BasicAttack();
                break;
            case 1:
                //          Debug.Log("Use ability 1");
                AbilityOne();
                break;
            case 2:
                //            Debug.Log("Use ability 2");
                AbilityTwo();
                break;
            case 3:
                //              Debug.Log("Use ability three");
                AbilityThree();
                break;
            default:
                //                Debug.Log("No Ability Used");
                break;
        }
    }
}
