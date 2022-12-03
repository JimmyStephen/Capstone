using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtemisPrimary : AbilityTemplate
{
    [SerializeField] float rollDistance = 1;
    private CharacterController2D cc;


    private void FixedUpdate()
    {
        //if facing right, roll left. if facing left, roll right
        cc.Dash((cc.GetDirection() ? rollDistance : -rollDistance) * Time.deltaTime);
    }

    public override void OnCreation()
    {
        //save character controller
        cc = parent.GetComponent<CharacterTemplate>().characterController;
        //flip
        cc.Flip();
        //make immune
        parent.GetComponent<CharacterTemplate>().isImmune = true;
        //Debug.Log("Immune Start");
        //
        if (audioOnCreate != null)
        {
            //play
            audioOnCreate.Play();
        }
    }

    public override void OnDestroy()
    {
        //remove immunity
        parent.GetComponent<CharacterTemplate>().isImmune = false;
    }

    public override void OnTriggerEnter(Collider other)
    {
        //do nothing
    }
}
