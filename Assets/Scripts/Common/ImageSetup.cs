using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSetup : MonoBehaviour
{
    //the object you are changing the image of
    [SerializeField] Image imageToSet;

    void Start()
    {
        //get the image you are going to set
        if (GameManager.Instance.currentSelectedInfo.TryGetComponent<CharacterTemplate>(out CharacterTemplate ct))
        {
            Sprite newSprite = ct.CharacterImage;
            if(newSprite != null)
            {
                //set the stored object to the image you need
                imageToSet.sprite = newSprite;
            }
        }
    }
}
