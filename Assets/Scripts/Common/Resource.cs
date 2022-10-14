using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [Header("A name for the script, this is never used")]
    [SerializeField] string Name;

    [Header("Values")]
    [SerializeField] float MaxValue;
    [SerializeField] float StartValue;
    [SerializeField] float DefaultRegenerationRate;

    private float currentValue;
    private float currentRegenerationRate;
    bool pauseRegeneration = false;

    // Start is called before the first frame update
    void Start()
    {
        currentValue = StartValue;
        currentRegenerationRate = DefaultRegenerationRate;
    }

    // Update is called once per frame
    void Update()
    {
        //Regenerate (if needed)
        if(!pauseRegeneration) currentValue += currentRegenerationRate * Time.deltaTime;

        //Make sure the value isnt over the max
        if(currentValue > MaxValue) currentValue = MaxValue;
    }
    
    /// <summary>
    /// Call this method to heal the player by a flat amount
    /// </summary>
    /// <param name="amount">How much to heal by</param>
    public void Heal(float amount)
    {
        currentValue += amount;
        if (currentValue > MaxValue) currentValue = MaxValue;
    }
    
    /// <summary>
    /// Call this method to damage the player by a flat amount
    /// </summary>
    /// <param name="amount">How much damage to take</param>
    public void Damage(float amount)
    {
        currentValue -= amount;
        if (currentValue < 0) currentValue = 0;
    }
    
    /// <summary>
    /// This method is to make sure you have enough of the current resource to use an ability
    /// </summary>
    /// <param name="test"></param>
    /// <returns></returns>
    public bool CheckEnoughResource(float test)
    {
        return currentValue > test;
    }

    /// <summary>
    /// This method will return the current value of the resource
    /// </summary>
    /// <returns>The Current Value</returns>
    public float GetCurrent()
    {
        return currentValue;
    }

    /// <summary>
    /// Returns the max value of the resource
    /// </summary>
    /// <returns>The Max Value</returns>
    public float GetMax()
    {
        return MaxValue;
    }

    /// <summary>
    /// Method that returns if the current value is 0 or lower
    /// </summary>
    /// <returns>if the current value is less than 0</returns>
    public bool IsEmpty()
    {
        return currentValue <= 0;
    }
}
