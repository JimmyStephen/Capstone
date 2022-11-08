using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupCharacterInfo : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text CharacterName;
    [SerializeField] TMPro.TMP_Text Basic;
    [SerializeField] TMPro.TMP_Text AbilityOne;
    [SerializeField] TMPro.TMP_Text AbilityTwo;
    [SerializeField] TMPro.TMP_Text UltimateAbility;
    void Start()
    {
        if (GameManager.Instance.currentSelectedInfo.TryGetComponent<CharacterTemplate>(out CharacterTemplate ct))
        {
            CharacterName.SetText(ct.characterName);
            Basic.SetText("Basic Ability\n" + ct.BasicAbilityDesc);
            AbilityOne.SetText("Ability One\n" + ct.AbilityOneDesc);
            AbilityTwo.SetText("Ability Two\n" + ct.AbilityTwoDesc);
            UltimateAbility.SetText("Ultimate Ability\n" + ct.UltimateAbilityDesc);
            GameManager.Instance.currentSelectedInfo = null;
        }
        else
        {
            Debug.Log("Data was Null");
            SceneLoader.Instance.LoadScene(1);
        }
    }
}
