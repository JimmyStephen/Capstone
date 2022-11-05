using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] GameObject[] playerCharacters;
    [SerializeField] GameObject[] aiCharacters;

    public GameObject playerOne = null;
    public GameObject playerTwo = null;
    //private bool characterOnePlayer = true;
    //private bool characterTwoPlayer = false;

    [SerializeField] Transform playerOneSpawn;
    [SerializeField] Transform playerTwoSpawn;

    [SerializeField] TMPro.TMP_Text PlayerOneHealthDisplay;
    [SerializeField] TMPro.TMP_Text PlayerOneEnergyDisplay;
    [SerializeField] Scrollbar PlayerOneHealthSlider;
    [SerializeField] Scrollbar PlayerOneEnergySlider;

    [SerializeField] TMPro.TMP_Text PlayerTwoHealthDisplay;
    [SerializeField] TMPro.TMP_Text PlayerTwoEnergyDisplay;
    [SerializeField] Scrollbar PlayerTwoHealthSlider;
    [SerializeField] Scrollbar PlayerTwoEnergySlider;

    [SerializeField] TMPro.TMP_Text WinnerNameDisplay;
    [SerializeField] TMPro.TMP_Text WinnerHealthDisplay;

    [SerializeField] GameObject[] enableForCursor;
    [SerializeField] GameObject[] enableForGame;

    private CharacterTemplate currentWinner = null;

    public string SelectCharacter(int characterSelect, int player)
    {
        if(player == 1)
        {
            playerOne = playerCharacters[characterSelect];
            //characterOnePlayer = true;
        }
        else
        {
            playerTwo = playerCharacters[characterSelect];
            //characterTwoPlayer = true;
        }
        return playerCharacters[characterSelect].name.ToString();
    }

    public string SelectAI(int characterSelect, int player)
    {
        if (player == 1)
        {
            playerOne = aiCharacters[characterSelect];
            //characterOnePlayer = false;
        }
        else
        {
            playerTwo = aiCharacters[characterSelect];
            //characterTwoPlayer = false;
        }
        return aiCharacters[characterSelect].name.ToString();
    }

    public void StartGame()
    {
        //if nothing selected choose random
        if(playerOne == null)
        {
            playerOne = aiCharacters[Random.Range(0, aiCharacters.Length)];
            //characterOnePlayer = false;
        }
        if(playerTwo == null)
        {
            playerTwo = aiCharacters[Random.Range(0, aiCharacters.Length)];
            //characterTwoPlayer = false;
        }

        SceneLoader.Instance.LoadScene(3);
        StartCoroutine(PlaceCharacters());
    }
    public void EndGame()
    {
        SceneLoader.Instance.LoadScene(5);
        ResetFields();
        ToggleCursor();
        WinnerNameDisplay.SetText("WINNER!!!\n" + currentWinner.name);
        WinnerHealthDisplay.SetText("Remaining Health " + currentWinner.health.GetCurrent());
        Destroy(currentWinner.gameObject);
    }
    public void ResetGame()
    {
        SceneLoader.Instance.LoadScene(1);
        playerOne = null;
        playerTwo = null;
        currentWinner = null;
        ResetFields();
    }
    public void SetWinner(CharacterTemplate winner)
    {
        currentWinner = winner;
    }

    private void ResetFields()
    {
        PlayerOneHealthDisplay.SetText("");
        PlayerOneEnergyDisplay.SetText("");
        PlayerTwoHealthDisplay.SetText("");
        PlayerTwoEnergyDisplay.SetText("");
        WinnerNameDisplay.SetText("");
        WinnerHealthDisplay.SetText("");
        PlayerOneHealthSlider.size = 1;
        PlayerOneEnergySlider.size = 1;
        PlayerTwoHealthSlider.size = 1;
        PlayerTwoEnergySlider.size = 1;
    }
    private void ToggleCursor()
    {
        foreach (var v in enableForCursor)
        {
            v.SetActive(!v.activeSelf);
        }
        foreach (var v in enableForGame)
        {
            v.SetActive(!v.activeSelf);
        }
    }
    IEnumerator PlaceCharacters()
    {
        yield return new WaitForSeconds(3);

        ToggleCursor();

        var p1 = Instantiate(playerOne, playerOneSpawn.position, playerOneSpawn.rotation);
        var p2 = Instantiate(playerTwo, playerTwoSpawn.position, playerTwoSpawn.rotation);

        p1.GetComponent<CharacterTemplate>().SetDisplay(PlayerOneHealthDisplay, PlayerOneEnergyDisplay, PlayerOneHealthSlider, PlayerOneEnergySlider);
        p1.GetComponent<CharacterTemplate>().opponent = p2;

        p2.GetComponent<CharacterTemplate>().SetDisplay(PlayerTwoHealthDisplay, PlayerTwoEnergyDisplay, PlayerTwoHealthSlider, PlayerTwoEnergySlider);
        p2.GetComponent<CharacterTemplate>().opponent = p1;
    }
}
