using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeimosAbilityTwo : AbilityTemplate
{
    [SerializeField] GameObject createOnDestroy;
    [SerializeField] float jumpForce = 300;
    [SerializeField] float dashForce = 300;

    private CharacterTemplate ct;
    public override void OnCreation()
    {
        ct = parent.GetComponent<CharacterTemplate>();
        if (!ct.characterController.m_Grounded)
        {
            Debug.Log("Not Grounded? How did this happen??");
        }
        float move = (ct.characterController.GetDirection() ? dashForce : -dashForce);
        ct.characterController.ForcedMove(move, jumpForce);
        if (audioOnCreate != null)
        {
            //play
            audioOnCreate.Play();
        }
    }

    public override void OnDestroy()
    {
        Vector3 position = new(transform.position.x, .125f, 0);
        position.x += parent.GetComponent<CharacterTemplate>().characterController.GetDirection() ? 1.5f : -1.5f;
        var temp = Instantiate(createOnDestroy, position, new(0, 0, 0, 0));
        temp.GetComponent<DeimosExplosion>().owner = parent;
    }

    public override void OnTriggerEnter(Collider other)
    {
/*        //once you get grounded trigger the explosion
        if (ct.characterController.m_Grounded)
        {
            Debug.Log("Grounded!!!");
            //trigger explosion
            Vector3 position = new(transform.position.x, .125f, 0);
            position.x += parent.GetComponent<CharacterTemplate>().characterController.GetDirection() ? 1.5f : -1.5f;
            var temp = Instantiate(createOnDestroy, position, new(0, 0, 0, 0));
            temp.GetComponent<DeimosExplosion>().owner = parent;

            Destroy(this, .05f);
        }*/
    }
}
