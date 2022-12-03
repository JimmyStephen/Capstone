using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtemisMediumHealth : HealthStateTemplate
{
    public ArtemisMediumHealth(CharacterTemplate owner, string name, State[] childStates) : base(owner, name, childStates) { }

    private float currentPinnedDuration;
    readonly private float pinnedDuration = 2;
    readonly private float pinnedStartDuration = 2;
    readonly private float pinnedDistance = 2;

    public FloatRef distance;
    public BoolRef abilityOnCD;
    public BoolRef pinned;
    private const float distanceForAggression = 5;


    public override void OnCreate()
    {
        distance = new FloatRef();
        abilityOnCD = new BoolRef();
        pinned = new BoolRef();

        //to agressive
        //if you are far from opponent w/ abilities off cd
        sMachine.AddTransition(sMachine.StateFromName(typeof(ArtemisPassive).Name), new Transition(new Condition[] { new FloatCondition(distance, Condition.Predicate.GREATER, distanceForAggression), new BoolCondition(abilityOnCD, false) }), sMachine.StateFromName(typeof(ArtemisAggressive).Name));
        sMachine.AddTransition(sMachine.StateFromName(typeof(ArtemisDefensive).Name), new Transition(new Condition[] { new FloatCondition(distance, Condition.Predicate.GREATER, distanceForAggression), new BoolCondition(abilityOnCD, false) }), sMachine.StateFromName(typeof(ArtemisAggressive).Name));

        //to defensive
        //if you are close to opponent
        sMachine.AddTransition(sMachine.StateFromName(typeof(ArtemisPassive).Name), new Transition(new Condition[] { new FloatCondition(distance, Condition.Predicate.LESS_EQUAL, distanceForAggression) }), sMachine.StateFromName(typeof(ArtemisDefensive).Name));
        sMachine.AddTransition(sMachine.StateFromName(typeof(ArtemisAggressive).Name), new Transition(new Condition[] { new FloatCondition(distance, Condition.Predicate.LESS_EQUAL, distanceForAggression) }), sMachine.StateFromName(typeof(ArtemisDefensive).Name));

        //to passive
        //if you are far but abilities on cd
        sMachine.AddTransition(sMachine.StateFromName(typeof(ArtemisAggressive).Name), new Transition(new Condition[] { new FloatCondition(distance, Condition.Predicate.GREATER, 5), new BoolCondition(abilityOnCD, true) }), sMachine.StateFromName(typeof(ArtemisPassive).Name));
        sMachine.AddTransition(sMachine.StateFromName(typeof(ArtemisDefensive).Name), new Transition(new Condition[] { new FloatCondition(distance, Condition.Predicate.GREATER, 5), new BoolCondition(abilityOnCD, true) }), sMachine.StateFromName(typeof(ArtemisPassive).Name));
        sMachine.AddTransition(sMachine.StateFromName(typeof(ArtemisPinned).Name), new Transition(new Condition[] { new BoolCondition(pinned, false) }), sMachine.StateFromName(typeof(ArtemisPassive).Name));

        //to pinned
        //if you are stuck between the wall and opponent
        sMachine.AddTransition(sMachine.StateFromName(typeof(ArtemisAggressive).Name), new Transition(new Condition[] { new BoolCondition(pinned, true) }), sMachine.StateFromName(typeof(ArtemisPinned).Name));
        sMachine.AddTransition(sMachine.StateFromName(typeof(ArtemisDefensive).Name), new Transition(new Condition[] { new BoolCondition(pinned, true) }), sMachine.StateFromName(typeof(ArtemisPinned).Name));
        sMachine.AddTransition(sMachine.StateFromName(typeof(ArtemisPassive).Name), new Transition(new Condition[] { new BoolCondition(pinned, true) }), sMachine.StateFromName(typeof(ArtemisPinned).Name));

        sMachine.setState(sMachine.StateFromName(typeof(ArtemisPassive).Name));
    }

    public override void OnEnter()
    {
        //        throw new System.NotImplementedException();
    }

    public override void OnExit()
    {
        //        throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {

        //distance update
        distance.value = Mathf.Abs(Owner.transform.position.x - Owner.opponent.transform.position.x);

        //ability update
        bool basicOnCD = Owner.basicAttackDuration > 0;
        bool abilityOneCD = Owner.currentAbilityOneCooldown > 0;
        bool abilityTwoCD = Owner.currentAbilityTwoCooldown > 0;
        bool ultCD = Owner.currentAbilityThreeCooldown > 0;
        abilityOnCD.value = basicOnCD && abilityOneCD && abilityTwoCD && ultCD;

        if (pinned.value)
        {
            //reduce timer
            currentPinnedDuration -= Time.deltaTime;
            if (currentPinnedDuration <= 0)
            {
                pinned.value = false;
            }
        }
        else
        {
            //check the distance
            float wallDistance = GetCloserWallDistance();
            //if you are to close to either wall increase timer
            if (wallDistance <= pinnedDistance)
            {
                //if you are pinned increase the timer
                currentPinnedDuration += Time.deltaTime;
                //if you are pinned for long enough set the trigger
                if (currentPinnedDuration >= pinnedStartDuration)
                {
                    //set the pinned duration and the trigger
                    currentPinnedDuration = pinnedDuration;
                    pinned.value = true;
                }
            }
            else
            {
                //if you arent pinned decrement the timer down to 0
                currentPinnedDuration -= Time.deltaTime;
                //if the timer is below 0 set it to 0
                currentPinnedDuration = (currentPinnedDuration <= 0) ? 0 : currentPinnedDuration;
            }
        }

        sMachine.Update();
    }

    public override bool ShouldJump()
    {
        return sMachine.currentState.ShouldJump();
    }
    public override float StateMovement()
    {
        return sMachine.currentState.StateMovement();
    }
    public override bool UseBasicAbility()
    {
        throw new System.Exception("You shouldn't call this method this way");
    }
    public override bool UseAbilityOne()
    {
        throw new System.Exception("You shouldn't call this method this way");
    }
    public override bool UseAbilityTwo()
    {
        throw new System.Exception("You shouldn't call this method this way");
    }
    public override bool UseAbilityThree()
    {
        throw new System.Exception("You shouldn't call this method this way");
    }
    public override int UseAbility()
    {
        return sMachine.currentState.UseAbility();
    }



    private float GetCloserWallDistance()
    {
        float retVal;
        float distanceToLeftWall = Mathf.Abs(Owner.transform.position.x - Owner.leftWall.transform.position.x);
        float distanceToRightWall = Mathf.Abs(Owner.transform.position.x - Owner.rightWall.transform.position.x);
        retVal = (distanceToLeftWall > distanceToRightWall) ? distanceToRightWall : distanceToLeftWall;
//        Debug.Log("Distance to closer wall: " + retVal + " | Left: " + distanceToLeftWall + " Right: " + distanceToRightWall);
        return retVal;
    }
}
