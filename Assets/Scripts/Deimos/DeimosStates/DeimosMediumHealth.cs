using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeimosMediumHealth : HealthStateTemplate
{
    public DeimosMediumHealth(CharacterTemplate owner, string name, State[] childStates) : base(owner, name, childStates) { }

    //the max damage Deimos can take before being defensive
    //35 - 30 - 20
    readonly float damageThreshold = 30;
    //how long he will be defensive
    readonly float defensiveDuration = 1;
    //his health last frame (used to calculate his damage taken)
    float healthLastFrame = 0;

    //how much damage he has taken
    float currentStoredDamage = 0;
    //how much the stored damage decays per second (flat amount);
    readonly float damageDecayPerSec = .5f;

    //Jump Time Stuff
    float jumpTimer = 0;
    readonly float minJumpTime = 4;
    readonly float maxJumpTime = 8;

    BoolRef damageThresholdMet;
    FloatRef defensiveTimer;

    public override void OnCreate()
    {
        //initalize variables
        damageThresholdMet = new();
        defensiveTimer = new();

        //initalize states
        //To Aggressive
        //defensiveTimer <= 0
        sMachine.AddTransition(sMachine.StateFromName(typeof(DeimosPassive).Name), new Transition(new Condition[] { new FloatCondition(defensiveTimer, Condition.Predicate.LESS_EQUAL, 0) }), sMachine.StateFromName(typeof(DeimosAggressive).Name));
        //To passive
        //damageThreshold == true
        sMachine.AddTransition(sMachine.StateFromName(typeof(DeimosAggressive).Name), new Transition(new Condition[] { new BoolCondition(damageThresholdMet, true) }), sMachine.StateFromName(typeof(DeimosPassive).Name));

        //set inital state (Aggressive)
        sMachine.setState(sMachine.StateFromName(typeof(DeimosAggressive).Name));
    }
    public override void OnEnter()
    {
        //set variables
        jumpTimer = 0;
        defensiveTimer.value = 0;
        damageThresholdMet.value = false;
        healthLastFrame = Owner.health.GetCurrent();
    }
    public override void OnExit()
    {
        //
    }
    public override void OnUpdate()
    {
        //update timers
        if (damageThresholdMet.value)
        {
            //reduce timer
            defensiveTimer.value -= Time.deltaTime;
            //decay the stored dmg to 0
            currentStoredDamage -= damageThreshold * Time.deltaTime;
            if (defensiveTimer.value <= 0)
            {
                damageThresholdMet.value = false;
            }
        }
        else
        {
            //keep track of damage
            //get the damage taken since last frame
            float damageSinceLastFrame = healthLastFrame - Owner.health.GetCurrent();
            //update the heath last frame
            healthLastFrame = Owner.health.GetCurrent();
            //update the stored damage
            currentStoredDamage += damageSinceLastFrame;
            //remove the decay
            currentStoredDamage -= damageDecayPerSec * Time.deltaTime;

            //Check if you met the threshold
            if (currentStoredDamage >= damageThreshold)
            {
                damageThresholdMet.value = true;
                defensiveTimer.value = defensiveDuration;
            }
        }

        jumpTimer -= Time.deltaTime;
    }

    public override bool ShouldJump()
    {
        if (jumpTimer < 0)
        {
            jumpTimer = Random.Range(minJumpTime, maxJumpTime);
            return true;
        }
        return false;
    }
    public override float StateMovement()
    {
        return sMachine.currentState.StateMovement();
    }

    public override int UseAbility()
    {
        return sMachine.currentState.UseAbility();
    }
    public override bool UseBasicAbility()
    {
        return sMachine.currentState.UseBasicAbility();
    }
    public override bool UseAbilityOne()
    {
        return sMachine.currentState.UseAbilityOne();
    }
    public override bool UseAbilityTwo()
    {
        return sMachine.currentState.UseAbilityTwo();
    }
    public override bool UseAbilityThree()
    {
        return sMachine.currentState.UseAbilityThree();
    }
}
