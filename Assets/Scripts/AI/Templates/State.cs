using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public CharacterTemplate owner { get; private set; }
    public string name { get; private set; }

    public State(CharacterTemplate owner, string name)
    {
        this.owner = owner;
        this.name = name;
    }

    public int[] shuffle(int[] toShuffle)
    {
        int temp = 0;
        for(int i = 0; i < toShuffle.Length; i++)
        {
            int rnd = Random.Range(0, toShuffle.Length);
            temp = toShuffle[rnd];
            toShuffle[rnd] = toShuffle[i];
            toShuffle[i] = temp;
        }
        return toShuffle;
    }

    /// <summary>
    /// Makes sure your facing the right direction
    /// </summary>
    public void checkDirection()
    {
        //get direction
        float direction = Mathf.Sign(owner.transform.position.x - owner.opponent.transform.position.x);
        if((direction > 0 && owner.characterController.GetDirection()) || (direction < 0 && !owner.characterController.GetDirection()))
        {
            owner.characterController.Flip();
        }
    }

    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();

    public abstract float StateMovement();
    public abstract bool shouldJump();
    public abstract bool useBasicAbility();
    public abstract bool useAbilityOne();
    public abstract bool useAbilityTwo();
    public abstract bool useAbilityThree();
    public abstract int UseAbility();
}
