using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RasputinAI : RasputinTemplate
{
    //This is ignored for life one
    [SerializeField] float highToMediumPercent;
    [SerializeField] float mediumToLowPercent;

    private HealthStateTemplate currentState;
    private RasputinHighHealth HighHealth;
    private RasputinMediumHealth MedHealth;
    private RasputinLowHealth LowHealth;

    private float healthPercent;
    bool attack = true;
    float attackTimer = 0;

    void Start()
    {
        characterController = GetComponent<CharacterController2D>();

        //make combat states
        RasputinAggressive Aggressive = new(this,   typeof(RasputinAggressive).Name);
        RasputinDefensive Defensive = new(this,     typeof(RasputinDefensive).Name);
        RasputinPassive Passive = new(this,         typeof(RasputinPassive).Name);
        //make health states
        HighHealth = new RasputinHighHealth(this,   typeof(RasputinHighHealth).Name, new State[] { Aggressive, Defensive, Passive });
        MedHealth = new  RasputinMediumHealth(this, typeof(RasputinMediumHealth).Name, new State[] { Aggressive, Defensive, Passive });
        LowHealth = new  RasputinLowHealth(this,    typeof(RasputinLowHealth).Name, new State[] { Aggressive, Defensive, Passive });
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
                //AbilityThree();
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
        
        if (health.GetCurrent() <= 0)
        {
            OnDeath();
        }
        else
        {
            TriggerEffects();
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

    /// <summary>
    /// Moves the AI as needed
    /// </summary>
    private void AIMovement()
    {
        if (CheckForStun()) return;
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
        if (health.GetMax() == 75) return;

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
                Debug.Log("Somehow called ult, Doesnt exist");
//                AbilityThree();
                break;
            default:
                //                Debug.Log("No Ability Used");
                break;
        }
    }
}
