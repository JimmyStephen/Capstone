using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtemisAI : CharacterTemplate
{
    [SerializeField] float highToMediumPercent;
    [SerializeField] float mediumToLowPercent;

    private HealthStateTemplate currentState;

    private ArtemisHighHealth aHighHealth;
    private ArtemisMediumHealth aMedHealth;
    private ArtemisLowHealth aLowHealth;

//    private FloatRef healthPercent;
    private float healthPercent;

    void Start()
    {
        characterController = GetComponent<CharacterController2D>();

        //make combat states
        ArtemisAggressive aAggressive   = new ArtemisAggressive(this, typeof(ArtemisAggressive).Name);
        ArtemisDefensive aDefensive     = new ArtemisDefensive(this, typeof(ArtemisDefensive).Name);
        ArtemisPassive aPassive         = new ArtemisPassive(this, typeof(ArtemisPassive).Name);
        ArtemisScared aScared           = new ArtemisScared(this, typeof(ArtemisScared).Name);
        //make health states
        aHighHealth = new ArtemisHighHealth(this, typeof(ArtemisHighHealth).Name, new State[] {aAggressive, aDefensive, aPassive, aScared});
        aMedHealth = new ArtemisMediumHealth(this, typeof(ArtemisMediumHealth).Name, new State[] { aAggressive, aDefensive, aPassive, aScared });
        aLowHealth = new ArtemisLowHealth(this, typeof(ArtemisLowHealth).Name, new State[] { aAggressive, aDefensive, aPassive, aScared });
        //run the create methods
        aHighHealth.OnCreate();
        aMedHealth.OnCreate();
        aLowHealth.OnCreate();

        //set default state
        currentState = aHighHealth;
    }

    bool attack = true;
    // Update is called once per frame
    void Update()
    {
        Debug.Log("Current State: " + currentState.name + " Inner State: " + currentState.sMachine.currentState.name);  
        stateUpdates();
        CharacterRequiredUpdates();

        //check for stun
        foreach (Effect effect in effects)
        {
            if (effect.isStunned())
            {
                characterController.Move(0, false, false);
                return;
            }
        }

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
            useAbility();
        }
        else
        {
            //check for movement
            AIMovement();
        }

        attack = !attack;
    }

    /// <summary>
    /// Moves the AI as needed
    /// </summary>
    private void AIMovement()
    {
        float movement = currentState.StateMovement();
        bool shouldJump = currentState.shouldJump();
        animator.SetFloat("Speed", Mathf.Abs(movement));
        characterController.Move(movement, false, shouldJump);
    }

    public override void BasicAttack()
    {
        AbilityTemplate at = BasicAttackObject.GetComponent<AbilityTemplate>();
        if (!at.canUse(health, energy, currentBasicAttackCooldown) || animationTimer >= 0)
        {
            //Debug.Log("Ability Cannot Be Used");
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
        //Debug.Log("Ability 1 Activated");

        AbilityTemplate at = abilityOneProjectile.GetComponent<AbilityTemplate>();
        if (!at.canUse(health, energy, currentAbilityOneCooldown) || animationTimer >= 0)
        {
            //Debug.Log("Ability Cannot Be Used");
            return;
        }
        currentAbilityOneCooldown = at.useAbility(health, energy);

        animator.SetTrigger("Ability1");
        animationTimer = animationOneDuration;
        StartCoroutine(SpawnAfterDelay(this.gameObject, abilityOneProjectilePosition, abilityOneProjectile, abilityOneDelay));
    }
    public override void AbilityTwo()
    {
        //Debug.Log("Ability 2 Activated");
        AbilityTemplate at = abilityTwoProjectile.GetComponent<AbilityTemplate>();

        if (!at.canUse(health, energy, currentAbilityTwoCooldown) || animationTimer >= 0)
        {
            //Debug.Log("Ability Cannot Be Used");
            return;
        }
        currentAbilityTwoCooldown = at.useAbility(health, energy);

        animator.SetTrigger("Ability2");
        animationTimer = animationTwoDuration;
        StartCoroutine(SpawnAfterDelay(this.gameObject, abilityTwoProjectilePosition, abilityTwoProjectile, abilityTwoDelay));

    }
    public override void AbilityThree()
    {
        //Debug.Log("Ultimate Ability Activated");

        AbilityTemplate at = abilityThreeProjectile.GetComponent<AbilityTemplate>();

        if (!at.canUse(health, energy, currentAbilityThreeCooldown))
        {
            //Debug.Log("Not enough resources or it is on CD");
            return;
        }
        currentAbilityThreeCooldown = at.useAbility(health, energy);

        animationTimer = animationThreeDuration;
        animator.SetTrigger("Ability3");
        StartCoroutine(SpawnAfterDelay(this.gameObject, abilityThreeProjectilePosition, abilityThreeProjectile, abilityThreeDelay));
    }

    public override void CharacterRequiredUpdates()
    {
        //reduce all timers
        //currentJumpCD -= Time.deltaTime;
        currentBasicAttackCooldown -= Time.deltaTime;
        currentAbilityOneCooldown -= Time.deltaTime;
        currentAbilityTwoCooldown -= Time.deltaTime;
        currentAbilityThreeCooldown -= Time.deltaTime;
        animationTimer -= Time.deltaTime;

        //check health states

        //apply buffs/debuffs
        TriggerEffects();

        //update GUI
        if (HealthDisplay != null)
        {
            HealthDisplay.SetText("Health: " + health.GetCurrent().ToString("F0"));
        }
        else
        {
            Debug.Log("No Health Display");
        }
        if (EnergyDisplay != null)
        {
            EnergyDisplay.SetText("Energy: " + energy.GetCurrent().ToString("F0"));
        }
        else
        {
            Debug.Log("No Energy Display");
        }

        //check dead
        if (health.GetCurrent() <= 0)
        {
            OnDeath();
        }
    }

    /// <summary>
    /// Updates the state and checks if it needs to change
    /// </summary>
    private void stateUpdates()
    {
        healthPercent = (health.GetCurrent() / health.GetMax()) * 100;
        //to high health
        if (healthPercent > highToMediumPercent && currentState != aHighHealth) {
            currentState.OnExit();
            currentState = aHighHealth; 
            currentState.OnEnter(); 
        }

        //to medium health
        if (healthPercent < highToMediumPercent && healthPercent > mediumToLowPercent && currentState != aMedHealth) {
            currentState.OnExit();
            currentState = aMedHealth; 
            currentState.OnEnter();
        }

        //to low health
        if (healthPercent < mediumToLowPercent && currentState != aLowHealth) {
            currentState.OnExit();
            currentState = aLowHealth;
            currentState.OnEnter();
        }

        currentState.OnUpdate();
    }
    
    /// <summary>
    /// Checks if you need to use an ability, then calls the respective functions
    /// </summary>
    private void useAbility()
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

    public override void OnDeath()
    {
        //died
        Debug.Log("YOU DIED!!!!");
        //set the winner to your opponent
        ///
        //destroy this object
        Destroy(gameObject);
    }
}
