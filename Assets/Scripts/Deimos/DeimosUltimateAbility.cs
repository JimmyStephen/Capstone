using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeimosUltimateAbility : AbilityTemplate
{
    [SerializeField] float maxSize = 5;
    [SerializeField] float scaleSpeed = 1;

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
    }

    bool hasTriggered = false;
    public override void OnTriggerEnter(Collider other)
    {
        //if triggered, if the object is the parent, is the object is a ability from the same owner,
        //or if the object is a trap; return.
        if (hasTriggered) return;
        if (other.gameObject == parent) return;
        if (other.gameObject.TryGetComponent<AbilityTemplate>(out AbilityTemplate at))
        {
            if (at.parent == parent) return;
        }
        if (other.CompareTag("Trap")) return;

        if (other.TryGetComponent<CharacterTemplate>(out CharacterTemplate player))
        {
            if (!player.CCImmune)
            {
                //make debuff
                Effect apply =  new(true, 4, 0, 0, 0, 0, .75f, .75f, false);
                Effect apply2 = new(true, 2, 0, 0, 0, 0, 0, 1, true);
                //apply debuff
                player.effects.Add(apply);
                player.effects.Add(apply2);
            }
            //set the trigger so that the object knows it has triggered
            hasTriggered = true;
        }
    }

    void Update()
    {
        float currentScale = scaleSpeed * Time.deltaTime;
        float newSize = currentScale;
        
        transform.localScale += new Vector3(newSize, newSize, newSize);

        //Debug.Log("New Scale: " + newSize);
        if (transform.localScale.x > maxSize)
        {
            transform.localScale.Set(maxSize, maxSize, maxSize);
            Destroy(gameObject, .5f);
        }


        //Debug.Log("Scale: " + transform.localScale.x);
    }
}
