using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] characters;

    public GameObject playerOne = null;
    public GameObject playerTwo = null;

    [SerializeField] Transform playerOneSpawn;
    [SerializeField] Transform playerTwoSpawn;

    [SerializeField] TMPro.TMP_Text PlayerOneHealthDisplay;
    [SerializeField] TMPro.TMP_Text PlayerOneEnergyDisplay;

    [SerializeField] TMPro.TMP_Text PlayerTwoHealthDisplay;
    [SerializeField] TMPro.TMP_Text PlayerTwoEnergyDisplay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectCharacter(int characterSelect, int player)
    {
        if(player == 1)
        {
            playerOne = characters[characterSelect];
        }
        else
        {
            playerTwo = characters[characterSelect];
        }
    }

    public void startGame()
    {
        //if nothing selected choose random
        if(playerOne == null)
        {
            playerOne = characters[Random.Range(0, characters.Length)];
        }
        if(playerTwo == null)
        {
            playerTwo= characters[Random.Range(0,characters.Length)];
        }

        var p1 = Instantiate(playerOne, playerOneSpawn.position, playerOneSpawn.rotation);
        var p2 = Instantiate(playerTwo, playerTwoSpawn.position, playerTwoSpawn.rotation);

        p1.GetComponent<CharacterTemplate>().setDesplay(PlayerOneHealthDisplay, PlayerOneEnergyDisplay);
        p1.GetComponent<CharacterTemplate>().opponent = p2;

        p2.GetComponent<CharacterTemplate>().setDesplay(PlayerTwoHealthDisplay, PlayerTwoEnergyDisplay);
        p2.GetComponent<CharacterTemplate>().opponent = p1;
    }
}
