using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JackAbilityTwo : AbilityTemplate
{
    //Poison Trap
    [SerializeField] GameObject createOnDestroy;
    public override void OnCreation()
    {
        //throw new System.NotImplementedException();
        //
        if (audioOnCreate != null)
        {
            //play
            audioOnCreate.Play();
        }
    }

    public override void OnDestroy()
    {
        var temp = Instantiate(createOnDestroy, this.transform.position, this.transform.rotation);
        temp.GetComponent<JackTrap>().owner = parent;
        if (audioOnDestroy != null)
        {
            //play
            audioOnDestroy.Play();
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        //throw new System.NotImplementedException();
    }
}
