using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition
{
    Condition[] conditions;

    public Transition(Condition[] condition)
    {
        conditions = condition;
    }

    public bool ToTransition()
    {
        foreach(var condition in conditions)
        {
            if (!condition.IsTrue()) return false;
        }
        return true;
    }
}
