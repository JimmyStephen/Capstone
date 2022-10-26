using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] GameObject[] playerCharacters;
    [SerializeField] GameObject[] aiCharacters;

    public GameObject playerOne = null;
    public GameObject playerTwo = null;

    [SerializeField] Transform playerOneSpawn;
    [SerializeField] Transform playerTwoSpawn;

    [SerializeField] TMPro.TMP_Text PlayerOneHealthDisplay;
    [SerializeField] TMPro.TMP_Text PlayerOneEnergyDisplay;

    [SerializeField] TMPro.TMP_Text PlayerTwoHealthDisplay;
    [SerializeField] TMPro.TMP_Text PlayerTwoEnergyDisplay;

    [SerializeField] GameObject cursor;

    // Start is called before the first frame update
    void Start()
    {
        //find the text boxes
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectCharacter(int characterSelect, int player)
    {
        if(player == 1)
        {
            playerOne = playerCharacters[characterSelect];
        }
        else
        {
            playerTwo = playerCharacters[characterSelect];
        }
    }

    public void startGame()
    {
        //if nothing selected choose random
        if(playerOne == null)
        {
            playerOne = aiCharacters[Random.Range(0, aiCharacters.Length)];
        }
        if(playerTwo == null)
        {
            playerTwo= aiCharacters[Random.Range(0, aiCharacters.Length)];
        }

        cursor.SetActive(false);
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
