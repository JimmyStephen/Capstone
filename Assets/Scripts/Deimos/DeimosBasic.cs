using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeimosBasic : AbilityTemplate
{
    public override void OnCreation()
    {
        if (audioOnCreate != null)
        {
            //play
            audioOnCreate.Play();
        }
    }

    public override void OnDestroy()
    {
        if (audioOnDestroy != null)
        {
            //play
            audioOnDestroy.Play();
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        throw new System.NotImplementedException();
    }
}
