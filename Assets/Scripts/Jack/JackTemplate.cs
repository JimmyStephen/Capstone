using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class JackTemplate : CharacterTemplate
{
    public override void BasicAttack()
    {
        Debug.Log("Stabby stab");
    }

    public override void AbilityOne()
    {
        Debug.Log("Place Trap");
    }

    public override void AbilityTwo()
    {
        Debug.Log("Place Trap");
    }

    public override void AbilityThree()
    {
        Debug.Log("Vanish");
    }

    public override void OnDeath()
    {
        throw new System.NotImplementedException();
    }
}
