using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeimosAbilityOne : AbilityTemplate
{
    //Stomp
    [SerializeField] GameObject createOnDestroy;

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
        Vector3 position = new(transform.position.x, 0, 0);
        var temp = Instantiate(createOnDestroy, position, this.transform.rotation);
        temp.GetComponent<DeimosExplosion>().owner = parent;
    }

    public override void OnTriggerEnter(Collider other)
    {
        //throw new System.NotImplementedException();
    }
}
