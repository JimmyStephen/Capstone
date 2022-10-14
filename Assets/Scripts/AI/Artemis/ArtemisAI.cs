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

    // Update is called once per frame
    void Update()
    {
        stateUpdates();
        CharacterRequiredUpdates();
    }


    public void AIMovement()
    {
        float movement = currentState.StateMovement();
        bool shouldJump = currentState.shouldJump();
        characterController.Move(movement, false, shouldJump);
    }

    public override void BasicAttack()
    {
        throw new System.NotImplementedException();
    }
    public override void AbilityOne()
    {
        throw new System.NotImplementedException();
    }
    public override void AbilityTwo()
    {
        throw new System.NotImplementedException();
    }
    public override void AbilityThree()
    {
        throw new System.NotImplementedException();
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
        //HealthDisplay.SetText("Health: " + health.GetCurrent().ToString("F0"));
        //EnergyDisplay.SetText("Energy: " + energy.GetCurrent().ToString("F0"));

        //check dead
        if (health.GetCurrent() <= 0)
        {
            OnDeath();
            return;
        }
    }
    private void stateUpdates()
    {
        healthPercent = (health.GetCurrent() / health.GetMax()) * 100;
        //to high health
        if (healthPercent > highToMediumPercent && currentState != aHighHealth) { 
            currentState = aHighHealth; 
            currentState.OnEnter(); 
        }

        //to medium health
        if (healthPercent < highToMediumPercent && healthPercent > mediumToLowPercent && currentState != aMedHealth) { 
            currentState = aMedHealth; 
            currentState.OnEnter();
        }

        //to low health
        if (healthPercent < mediumToLowPercent && currentState != aLowHealth) { 
            currentState = aLowHealth;
            currentState.OnEnter();
        }

        currentState.OnUpdate();
        //Debug.Log("Current Health Precent: " + healthPercent);
        Debug.Log("Current Health State: " + currentState.name);
    }

    public override void OnDeath()
    {
        throw new System.NotImplementedException();
    }
}
