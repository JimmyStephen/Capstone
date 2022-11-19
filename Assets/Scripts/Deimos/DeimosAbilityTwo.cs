using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeimosAbilityTwo : AbilityTemplate
{

    [SerializeField] GameObject createOnDestroy;

    public override void OnCreation()
    {
        float move = (parent.GetComponent<CharacterTemplate>().characterController.GetDirection() ? 1.5f : -1.5f);
        parent.GetComponent<CharacterTemplate>().StartCoroutine(parent.GetComponent<CharacterTemplate>().characterController.ForcedMovement(move, .5f));
        parent.GetComponent<CharacterTemplate>().characterController.Knockup(300);
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

        Vector3 position = new(transform.position.x, .125f, 0);
        position.x += parent.GetComponent<CharacterTemplate>().characterController.GetDirection() ? 1.5f : -1.5f;
        var temp = Instantiate(createOnDestroy, position, new(0,0,0,0));
        temp.GetComponent<DeimosExplosion>().owner = parent;
    }

    public override void OnTriggerEnter(Collider other)
    {
//        throw new System.NotImplementedException();
    }
}
