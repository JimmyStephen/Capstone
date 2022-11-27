using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public CharacterTemplate Owner { get; private set; }
    public string Name { get; private set; }

    public State(CharacterTemplate owner, string name)
    {
        this.Owner = owner;
        this.Name = name;
    }

    public int[] Shuffle(int[] toShuffle)
    {
        for (int i = 0; i < toShuffle.Length; i++)
        {
            int rnd = Random.Range(0, toShuffle.Length);
            (toShuffle[i], toShuffle[rnd]) = (toShuffle[rnd], toShuffle[i]);
        }
        return toShuffle;
    }

    /// <summary>
    /// Makes sure your facing the right direction
    /// </summary>
    public void CheckDirection()
    {
        //get direction
        float direction = Mathf.Sign(Owner.transform.position.x - Owner.opponent.transform.position.x);
        if((direction > 0 && Owner.characterController.GetDirection()) || (direction < 0 && !Owner.characterController.GetDirection()))
        {
            Owner.characterController.Flip();
        }
    }

    /// <summary>
    /// What happens when you enter this state
    /// </summary>
    public abstract void OnEnter();
    /// <summary>
    /// What happens every update
    /// </summary>
    public abstract void OnUpdate();
    /// <summary>
    /// What happens when the state exits
    /// </summary>
    public abstract void OnExit();

    /// <summary>
    /// How the object should move
    /// </summary>
    /// <returns>The direction of movement</returns>
    public abstract float StateMovement();
    /// <summary>
    /// If the object should jump
    /// </summary>
    /// <returns>If the object should jump</returns>
    public abstract bool ShouldJump();
    /// <summary>
    /// When the character should use their basic ability
    /// </summary>
    /// <returns>If the ability should be used</returns>
    public abstract bool UseBasicAbility();
    /// <summary>
    /// When the character should use their primary ability
    /// </summary>
    /// <returns>If the ability should be used</returns>
    public abstract bool UseAbilityOne();
    /// <summary>
    /// When the character should use their secondary ability
    /// </summary>
    /// <returns>If the ability should be used</returns>
    public abstract bool UseAbilityTwo();
    /// <summary>
    /// When the character should use their ultimate ability
    /// </summary>
    /// <returns>If the ability should be used</returns>
    public abstract bool UseAbilityThree();
    /// <summary>
    /// What ability (if any) should be used
    /// </summary>
    /// <returns>What ability should be used. 0 for basic -> 4 nothing</returns>
    public abstract int UseAbility();
}
