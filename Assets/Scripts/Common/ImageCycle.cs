using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCycle : MonoBehaviour
{
    //serialize fields
    //image box to change (image object)
    [SerializeField] Image imageBox;
    //list of images (array[sprite])
    [SerializeField, Tooltip("The sprites that will be cycled through")] Sprite[] sprites;
    //time between changes (float)
    [SerializeField, Tooltip("Time in seconds")] float timeBetweenChangesMin = 2.5f;
    [SerializeField, Tooltip("Time in seconds")] float timeBetweenChangesMax = 3.5f;

    //private variables
    //current timer (float)
    private float currentTime;
    //current image (int)
    private int currentIndex;
    
    void Start()
    {
        //Set the timer
        currentTime = Random.Range(timeBetweenChangesMin, timeBetweenChangesMax);
        //choose the inital image   
        currentIndex = Random.Range(0, sprites.Length);
        //set the inital image
        imageBox.sprite = sprites[currentIndex];
    }

    void Update()
    {
        currentTime -= Time.deltaTime;
        //change the image as needed
        if(currentTime <= 0)
        {
            //change the index
            currentIndex = (++currentIndex < sprites.Length) ? currentIndex : 0;
            //set the image
            imageBox.sprite = sprites[currentIndex];
            //reset the timer
            currentTime = Random.Range(timeBetweenChangesMin, timeBetweenChangesMax);
        }
    }
}
