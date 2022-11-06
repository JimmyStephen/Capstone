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

        //set descriptions
        BasicAbilityDesc = "Kick forward dealing damage and knocking the opponent back a short distance";
        AbilityOneDesc = "Roll away from the direction your facing becoming immune to damage for the duration of the roll";
        AbilityTwoDesc = "Shoot an arrow that deals a small amount of damage";
        UltimateAbilityDesc = "Shoot an arrow that does a medium amount of damage that turns into a tornado that stuns and does more damage when it collides with anything";

        //make combat states
        ArtemisAggressive aAggressive   = new (this, typeof(ArtemisAggressive).Name);
        ArtemisDefensive aDefensive     = new (this, typeof(ArtemisDefensive).Name);
        ArtemisPassive aPassive         = new (this, typeof(ArtemisPassive).Name);
        ArtemisScared aScared           = new (this, typeof(ArtemisScared).Name);
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
        //Debug.Log("Current State: " + currentState.name + " Inner State: " + currentState.sMachine.currentState.name);  
        StateUpdates();
        CharacterRequiredUpdates();

        //check for stun
        if (CheckForStun())
        {
            characterController.Move(0, false, false);
            animator.SetFloat("Speed", 0);
            return;
        }

        //check for animation
        if (animationTimer >= 0)
        {
            characterController.Move(0, false, false);
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
        float movement = currentState.StateMovement() * speed * currentSpeedMultiplier;
        bool shouldJump = false;
        if (characterController.m_Grounded)
        {
            shouldJump = currentState.ShouldJump();
        }
        animator.SetFloat("Speed", Mathf.Abs(movement));
        characterController.Move(movement, false, shouldJump);
    }

    public override void BasicAttack()
    {
        if (CheckForStun())
        {
            return;
        }
        AbilityTemplate at = BasicAttackObject.GetComponent<AbilityTemplate>();
        if (!at.CanUse(health, energy, currentBasicAttackCooldown) || animationTimer >= 0)
        {
            //Debug.Log("Ability Cannot Be Used");
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
        //Debug.Log("Ability 1 Activated");

        AbilityTemplate at = abilityOneProjectile.GetComponent<AbilityTemplate>();
        if (!at.CanUse(health, energy, currentAbilityOneCooldown) || animationTimer >= 0)
        {
            //Debug.Log("Ability Cannot Be Used");
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
        //Debug.Log("Ability 2 Activated");
        AbilityTemplate at = abilityTwoProjectile.GetComponent<AbilityTemplate>();

        if (!at.CanUse(health, energy, currentAbilityTwoCooldown) || animationTimer >= 0)
        {
            //Debug.Log("Ability Cannot Be Used");
            return;
        }
        currentAbilityTwoCooldown = at.UseAbility(health, energy);

        animator.SetTrigger("Ability2");
        animationTimer = animationTwoDuration;
        StartCoroutine(SpawnAfterDelay(this.gameObject, abilityTwoProjectilePosition, abilityTwoProjectile, abilityTwoDelay));

    }
    public override void AbilityThree()
    {
        //Debug.Log("Ultimate Ability Activated");
        if (CheckForStun())
        {
            return;
        }
        AbilityTemplate at = abilityThreeProjectile.GetComponent<AbilityTemplate>();

        if (!at.CanUse(health, energy, currentAbilityThreeCooldown))
        {
            //Debug.Log("Not enough resources or it is on CD");
            return;
        }
        currentAbilityThreeCooldown = at.UseAbility(health, energy);

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
        if (EnergyDisplay != null)
        {
            EnergyDisplay.SetText("Energy: " + energy.GetCurrent().ToString("F0"));
        }
        if(HealthSlider != null)
        {
            HealthSlider.size = health.GetCurrent() / health.GetMax();
        }
        if(EnergySlider != null)
        {
            EnergySlider.size = energy.GetCurrent() / energy.GetMax();
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
    private void StateUpdates()
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
