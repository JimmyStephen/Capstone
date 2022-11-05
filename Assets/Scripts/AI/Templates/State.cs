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

    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();

    public abstract float StateMovement();
    public abstract bool ShouldJump();
    public abstract bool UseBasicAbility();
    public abstract bool UseAbilityOne();
    public abstract bool UseAbilityTwo();
    public abstract bool UseAbilityThree();
    public abstract int UseAbility();
}
