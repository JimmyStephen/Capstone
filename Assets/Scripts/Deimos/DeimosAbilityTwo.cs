using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeimosAbilityTwo : AbilityTemplate
{
    [SerializeField] GameObject createOnDestroy;
    [SerializeField] float jumpForce = 300;
    [SerializeField] float dashForce = 300;
    public override void OnCreation()
    {
        float move = (parent.GetComponent<CharacterTemplate>().characterController.GetDirection() ? dashForce : -dashForce);
        parent.GetComponent<CharacterTemplate>().characterController.ForcedMove(move, jumpForce);
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
