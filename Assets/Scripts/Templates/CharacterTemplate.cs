using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class CharacterTemplate : MonoBehaviour
{
    [HideInInspector] public bool playerOne = false;
    [Header("Character Info")]
    public string characterName = "";
    public string BasicAbilityDesc = "";
    public string AbilityOneDesc = "";
    public string AbilityTwoDesc = "";
    public string UltimateAbilityDesc = "";
    public Sprite CharacterImage = null;
    [Header("Animator")]
    public Animator animator;
    [Header("Movement")]
    public float speed = 1;
    public float jumpCD = 1;
    [Header("Resources")]
    public Resource health;
    public Resource energy;
    [Header("Damage Resistance")]
    public float resistanceFlat = 0;
    [Tooltip("Enter a value from -100 (double dmg) - 100 (no dmg)"), Range(-100,100)]public float resistancePercent = 1;
    [Header("Projectiles")]
    public GameObject BasicAttackObject;
    public GameObject abilityOneProjectile;
    public GameObject abilityTwoProjectile;
    public GameObject abilityThreeProjectile;
    [Header("Transforms")]
    public GameObject BasicAttackPosition;
    public GameObject abilityOneProjectilePosition;
    public GameObject abilityTwoProjectilePosition;
    public GameObject abilityThreeProjectilePosition;
    [Header("Times")]
    public float basicAttackDuration = 0;
    public float animationOneDuration = 0;
    public float animationTwoDuration = 0;
    public float animationThreeDuration = 0;
    public float basicAttackDelay = 0;
    public float abilityOneDelay = 0;
    public float abilityTwoDelay = 0;
    public float abilityThreeDelay = 0;

    [HideInInspector] public CharacterController2D characterController;

    [HideInInspector] public List<Effect> effects = new();
    [HideInInspector] public float currentBasicAttackCooldown = 0;
    [HideInInspector] public float currentAbilityOneCooldown = 0;
    [HideInInspector] public float currentAbilityTwoCooldown = 0;
    [HideInInspector] public float currentAbilityThreeCooldown = 0;
    [HideInInspector] public float animationTimer = 0;

    [HideInInspector] public float currentDamageMultiplier = 1;
    [HideInInspector] public float currentSpeedMultiplier = 1;

    [HideInInspector] public bool isImmune = false;
    [HideInInspector] public bool CCImmune = false;
    [HideInInspector] public bool effectImmune = false;
    
    [HideInInspector] public Scrollbar HealthSlider;
    [HideInInspector] public Scrollbar EnergySlider;

    //used by AI to find the opponent
    [HideInInspector] public GameObject opponent;
    //used by the AI to find the wall
    [HideInInspector] public GameObject leftWall;
    [HideInInspector] public GameObject rightWall;


    public IEnumerator SpawnAfterDelay(GameObject owner, GameObject location, GameObject spawnObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        bool right = owner.transform.rotation.y < 0;
        float angle = (right) ? 0 : 180;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);

        GameObject temp = Instantiate(spawnObject, location.transform.position, rotation);
        temp.GetComponent<AbilityTemplate>().parentTag = owner.transform.tag;
        temp.GetComponent<AbilityTemplate>().parent = owner;
    }
    public IEnumerator SpawnAfterDelayParent(GameObject owner, GameObject location, GameObject spawnObject, float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject temp = Instantiate(spawnObject, location.transform);
        temp.GetComponent<AbilityTemplate>().parentTag = owner.transform.tag;
        temp.GetComponent<AbilityTemplate>().parent = owner;
        temp.GetComponent<AbilityTemplate>().damageMultiplier = owner.GetComponent<CharacterTemplate>().currentDamageMultiplier;
    }

    /// <summary>
    /// Triggers all the effects
    /// </summary>
    public void TriggerEffects()
    {
        currentDamageMultiplier = 1;
        currentSpeedMultiplier = 1;

        List<Effect> eRemove = new();
        foreach(Effect e in effects)
        {
            //Debug.Log("Health: " + health + " Energy: " + energy);
            if (e == null)
            {
                eRemove.Add(e);
            }
            else
            {
                e.UpdateTrigger(health, energy);
                if (e.GetRemainingDuration() > 0)
                {
                    currentDamageMultiplier *= e.GetDamageMultipler();
                    currentSpeedMultiplier *= e.GetSpeedMultiplier();
                }
                else
                {
                    eRemove.Add(e);
                }
            }
        }

        foreach(Effect e in eRemove)
        {
            effects.Remove(e);
        }
    }
    /// <summary>
    /// Checks if there are any stuns affecting the character
    /// </summary>
    /// <returns>If there are any stuns</returns>
    public bool CheckForStun()
    {
        foreach (Effect effect in effects)
        {
            if (effect.IsStunned())
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// Checks if there are any debuffs affecting the character
    /// </summary>
    /// <returns>If there are debuffs</returns>
    public bool CheckForDebuff()
    {
        foreach (Effect effect in effects)
        {
            if (effect.CheckIsDebuff())
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// Removes all effects clasified as debuffs
    /// </summary>
    /// <returns>If the method succeeded</returns>
    public bool CleanseDebuffs()
    {
        //cleanse all debuffs
        List<Effect> effects = new();
        foreach (var e in effects)
        {
            if (e.CheckIsDebuff())
            {
                effects.Add(e);
            }
        }

        foreach (var e in effects)
        {
            effects.Remove(e);
        }

        return true;
    }


    public void SetDisplay(Scrollbar healthSlider, Scrollbar energySlider)
    {
        HealthSlider = healthSlider;
        EnergySlider = energySlider;
    }
    public void SetWalls(GameObject setLeftWall, GameObject setRightWall)
    {
        leftWall = setLeftWall;
        rightWall = setRightWall;
    }

    public string GetCharacterName()
    {
        return characterName;
    }
    public string GetBasicAbilityDesc()
    {
        return BasicAbilityDesc;
    }
    public string GetAbilityOneDesc()
    {
        return AbilityOneDesc;
    }
    public string GetAbilityTwoDesc()
    {
        return AbilityTwoDesc;
    }
    public string GetUltimateAbilityDesc()
    {
        return UltimateAbilityDesc;
    }

    public float GetDamagePercentReduction()
    {
        return (100 - resistancePercent) / 100;
    }

    private void OnGUI()
    {
        Vector2 screen = Camera.main.WorldToScreenPoint(transform.position);

        Rect position = new(screen.x - 50, Screen.height - screen.y - 90, 110, 25);

        //For text
        GUIStyle style = new();
        style.fontSize = 20;
        style.fontStyle = FontStyle.Bold;
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = Color.white;
        style.normal.background = MakeTex(2, 2, new Color(0f, 0f, 0f, 1f));
        

        GUI.Box(position, "", style);
       
        if (playerOne)
        {
            GUI.Label(position, "Player One", style);
        }
        else
        {
            GUI.Label(position, "Player Two", style);
        }
    }
    private Texture2D MakeTex(int width, int height, Color color)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i<pix.Length; ++i)
        {
            pix[i] = color;
        }
        Texture2D result = new(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    //Abstract

    /// <summary>
    /// What will happen when you die
    /// </summary>
    abstract public void OnDeath();

    /// <summary>
    /// What will happen when you press the button for the basic attack
    /// </summary>
    abstract public void BasicAttack();

    /// <summary>
    /// What will happen when you press the button for the primary ability
    /// </summary>
    abstract public void AbilityOne();
    /// <summary>
    /// What will happen when you press the button for your secondary ability
    /// </summary>
    abstract public void AbilityTwo();
    /// <summary>
    /// What will happen when you press the button for your ultimate ability
    /// </summary>
    abstract public void AbilityThree();

    /// <summary>
    /// Stuff that MUST happen every update
    /// </summary>
    abstract public void CharacterRequiredUpdates();
}
