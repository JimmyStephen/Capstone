using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{

    public void SelectCharacter(int character, int player)
    {
        GameManager.Instance.selectCharacter(character, player);
    }

    public void ChangeScene(int sceneNum)
    {
        SceneLoader.Instance.LoadScene(sceneNum);
    }

    public void StartGame()
    {
        GameManager.Instance.startGame();
    }
}
