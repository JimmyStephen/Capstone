using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] GameObject[] playerCharacters;
    [SerializeField] GameObject[] aiCharacters;

    public GameObject playerOne = null;
    public GameObject playerTwo = null;
    private bool characterOnePlayer = true;
    private bool characterTwoPlayer = false;

    [SerializeField] Transform playerOneSpawn;
    [SerializeField] Transform playerTwoSpawn;

    [SerializeField] TMPro.TMP_Text PlayerOneHealthDisplay;
    [SerializeField] TMPro.TMP_Text PlayerOneEnergyDisplay;

    [SerializeField] TMPro.TMP_Text PlayerTwoHealthDisplay;
    [SerializeField] TMPro.TMP_Text PlayerTwoEnergyDisplay;

    [SerializeField] GameObject cursor;
    [SerializeField] GameObject gpCursor;
    [SerializeField] GameObject destroyOnPlay;

    public string selectCharacter(int characterSelect, int player)
    {
        if(player == 1)
        {
            playerOne = playerCharacters[characterSelect];
            characterOnePlayer = true;
        }
        else
        {
            playerTwo = playerCharacters[characterSelect];
            characterTwoPlayer = true;
        }
        return playerCharacters[characterSelect].name.ToString();
    }

    public string selectAI(int characterSelect, int player)
    {
        if (player == 1)
        {
            playerOne = aiCharacters[characterSelect];
            characterOnePlayer = false;
        }
        else
        {
            playerTwo = aiCharacters[characterSelect];
            characterTwoPlayer = false;
        }
        return aiCharacters[characterSelect].name.ToString();
    }

    public void startGame()
    {
        //if nothing selected choose random
        if(playerOne == null)
        {
            playerOne = aiCharacters[Random.Range(0, aiCharacters.Length)];
            characterOnePlayer = false;
        }
        if(playerTwo == null)
        {
            playerTwo = aiCharacters[Random.Range(0, aiCharacters.Length)];
            characterTwoPlayer = false;
        }

        cursor.SetActive(false);
        Destroy(destroyOnPlay, 0.05f);
        Destroy(gpCursor, 0.05f);
        SceneLoader.Instance.LoadScene(2);
        StartCoroutine(placeCharacters());
    }

    IEnumerator placeCharacters()
    {
        yield return new WaitForSeconds(4);


        var p1 = Instantiate(playerOne, playerOneSpawn.position, playerOneSpawn.rotation);
        var p2 = Instantiate(playerTwo, playerTwoSpawn.position, playerTwoSpawn.rotation);

        p1.GetComponent<CharacterTemplate>().setDisplay(PlayerOneHealthDisplay, PlayerOneEnergyDisplay);
        p1.GetComponent<CharacterTemplate>().opponent = p2;

        p2.GetComponent<CharacterTemplate>().setDisplay(PlayerTwoHealthDisplay, PlayerTwoEnergyDisplay);
        p2.GetComponent<CharacterTemplate>().opponent = p1;
    }
}
