using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text player1Select;
    [SerializeField] TMPro.TMP_Text player2Select;

    public void SelectCharacter(int character)
    {
        string ret = GameManager.Instance.SelectCharacter(character, 1);
        if (player1Select != null) player1Select.SetText(ret);
    }

    public void SelectAI(int character)
    {
        string ret = GameManager.Instance.SelectAI(character, 2);
        if(player2Select != null) player2Select.SetText(ret);
    }

    public void ChangeScene(int sceneNum)
    {
        SceneLoader.Instance.LoadScene(sceneNum);
    }

    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }
}
