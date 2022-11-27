using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [Header("Playable Characters")]
    [SerializeField] GameObject[] playerCharacters;
    [SerializeField] GameObject[] aiCharacters;
    [Header("Gameplay Scene")]
    [SerializeField] Transform playerOneSpawn;
    [SerializeField] Transform playerTwoSpawn;
    [SerializeField] Scrollbar PlayerOneHealthSlider;
    [SerializeField] Scrollbar PlayerOneEnergySlider;
    [SerializeField] Scrollbar PlayerTwoHealthSlider;
    [SerializeField] Scrollbar PlayerTwoEnergySlider;
    [SerializeField] GameObject[] enableForGame;
    [Header("End Game Scene")]
    [SerializeField] TMPro.TMP_Text WinnerNameDisplay;
    [SerializeField] TMPro.TMP_Text WinnerHealthDisplay;

    //currently selected characters
    [HideInInspector] public GameObject playerOne = null;
    [HideInInspector] public GameObject playerTwo = null;

    //Storage Object
    [HideInInspector] public GameObject currentSelectedInfo;

    //Who Won!!!
    private CharacterTemplate currentWinner = null;

    //-------
    //Methods
    int playerOneIndex = -1;
    int playerTwoIndex = -1;
    bool characterOnePlayer = true;
    bool characterTwoPlayer = false;
    public CharacterTemplate SelectCharacter(int characterSelect, int player)
    {
        if (player == 1)
        {
            while(characterSelect == -1)
            {
                int randSelect = Random.Range(0, playerCharacters.Length);
                if (randSelect != playerOneIndex || !characterOnePlayer) characterSelect = randSelect;
            }
            playerOneIndex = characterSelect;
            playerOne = playerCharacters[characterSelect];
            characterOnePlayer = true;
            return playerOne.GetComponent<CharacterTemplate>();
        }
        else
        {
            while (characterSelect == -1)
            {
                int randSelect = Random.Range(0, playerCharacters.Length);
                if (randSelect != playerTwoIndex || !characterTwoPlayer) characterSelect = randSelect;
            }
            playerTwoIndex = characterSelect;
            playerTwo = playerCharacters[characterSelect];
            characterTwoPlayer = true;
            return playerTwo.GetComponent<CharacterTemplate>();
        }
    }
    public CharacterTemplate SelectAI(int characterSelect, int player)
    {
        if (player == 1)
        {
            while (characterSelect == -1)
            {
                int randSelect = Random.Range(0, aiCharacters.Length);
                if (randSelect != playerOneIndex || characterOnePlayer) characterSelect = randSelect;
            }
            playerOneIndex = characterSelect;
            playerOne = aiCharacters[characterSelect];
            characterOnePlayer = false;
            return playerOne.GetComponent<CharacterTemplate>();
        }
        else
        {
            while (characterSelect == -1)
            {
                int randSelect = Random.Range(0, aiCharacters.Length);
                if (randSelect != playerTwoIndex || characterTwoPlayer) characterSelect = randSelect;
            }
            playerTwoIndex = characterSelect;
            playerTwo = aiCharacters[characterSelect];
            characterTwoPlayer = false;
            return playerTwo.GetComponent<CharacterTemplate>();
        }
    }

    float timer = 0;
    private void Update()
    {
        timer -= Time.deltaTime;
    }

    public void StartGame()
    {
        if (timer > 0) return;
        timer = 5;
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
        ToggleGame();
        ResetFields();
        SceneLoader.Instance.LoadScene(5);
        WinnerNameDisplay.SetText("WINNER!!!\nPlayer " + ((currentWinner.playerOne) ? "One\n" : "Two\n") + currentWinner.characterName);
        WinnerHealthDisplay.SetText("Remaining Health " + currentWinner.health.GetCurrent().ToString("F0"));
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
    public void ToCharacterInfoScene(int index)
    {
        currentSelectedInfo = playerCharacters[index];
        SceneLoader.Instance.LoadScene(7);
    }
    public void SetWinner(CharacterTemplate winner)
    {
        currentWinner = winner;
    }
    public void CloseApp()
    {
        Application.Quit();
    }

    //------
    //Private Methods
    private void ResetFields()
    {
        //PlayerOneHealthDisplay.SetText("");
        //PlayerOneEnergyDisplay.SetText("");
        //PlayerTwoHealthDisplay.SetText("");
        //PlayerTwoEnergyDisplay.SetText("");
        WinnerNameDisplay.SetText("");
        WinnerHealthDisplay.SetText("");
        PlayerOneHealthSlider.size = 1;
        PlayerOneEnergySlider.size = 1;
        PlayerTwoHealthSlider.size = 1;
        PlayerTwoEnergySlider.size = 1;
    }
    private void ToggleGame()
    {
        foreach (var v in enableForGame)
        {
            v.SetActive(!v.activeSelf);
        }
    }

    //-----
    //Enumerators
    [HideInInspector] public GameObject playerOneObject;
    [HideInInspector] public GameObject playerTwoObject;
    IEnumerator PlaceCharacters()
    {
        yield return new WaitForSeconds(3);

        ToggleGame();

        var p1 = Instantiate(playerOne, playerOneSpawn.position, playerOneSpawn.rotation);
        var p2 = Instantiate(playerTwo, playerTwoSpawn.position, playerTwoSpawn.rotation);

        p1.GetComponent<CharacterTemplate>().SetDisplay(PlayerOneHealthSlider, PlayerOneEnergySlider);
        p1.GetComponent<CharacterTemplate>().playerOne = true;
        p1.GetComponent<CharacterTemplate>().opponent = p2;

        p2.GetComponent<CharacterTemplate>().SetDisplay(PlayerTwoHealthSlider, PlayerTwoEnergySlider);
        p2.GetComponent<CharacterTemplate>().playerOne = false;
        p2.GetComponent<CharacterTemplate>().opponent = p1;

        playerOneObject = p1;
        playerTwoObject = p2;
    }
}
