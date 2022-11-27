using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Storage : MonoBehaviour
{
    [Header("Character Select Scene")]
    [SerializeField] TMPro.TMP_Text playerOneText;
    [SerializeField] TMPro.TMP_Text playerTwoText;
    
    [SerializeField] Image playerOneImage;
    [SerializeField] Image playerTwoImage;

    [SerializeField] Button characterOneSelectedButton;
    [SerializeField] Button characterOnePlayerButton;
    [SerializeField] Button characterOneAIButton;
    [SerializeField] Button characterTwoSelectedButton;
    [SerializeField] Button characterTwoPlayerButton;
    [SerializeField] Button characterTwoAIButton;

    //-------
    [Header("Gameplay Scene")]
    [SerializeField] FocusCamera fc;

    private void Start()
    {
        if(fc != null)
        {
            StartCoroutine(fc.Init());
        }
    }

    /// <summary>
    /// Called during runtime to change the selected character
    /// </summary>
    /// <param name="character">What character you are going to play as/against</param>
    public void SelectCharacter(int character)
    {
        if (playerSelecting == 1)
        {
            if (characterOnePlayer) SelectPlayer(character);
            else SelectAI(character);
        }
        else
        {
            if (characterTwoPlayer) SelectPlayer(character);
            else SelectAI(character);
        }
    }
    private void SelectPlayer(int character)
    {
        CharacterTemplate ret = GameManager.Instance.SelectCharacter(character, playerSelecting);
        if(playerSelecting == 1)
        {
            if(playerOneImage != null) playerOneImage.sprite = ret.CharacterImage;
            if(playerOneText != null) playerOneText.text = ret.characterName;
        }
        else
        {
            if (playerTwoImage != null) playerTwoImage.sprite = ret.CharacterImage;
            if (playerTwoText != null) playerTwoText.text = ret.characterName;
        }
    }
    private void SelectAI(int character)
    {
        CharacterTemplate ret = GameManager.Instance.SelectAI(character, playerSelecting);
        if (playerSelecting == 1)
        {
            if (playerOneImage != null) playerOneImage.sprite = ret.CharacterImage;
            if (playerOneText != null) playerOneText.text = ret.characterName;
        }
        else
        {
            if (playerTwoImage != null) playerTwoImage.sprite = ret.CharacterImage;
            if (playerTwoText != null) playerTwoText.text = ret.characterName;
        }
    }

    public void ChangeScene(int sceneNum)
    {
        SceneLoader.Instance.LoadScene(sceneNum);
    }
    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }
    public void ResetGame()
    {
        GameManager.Instance.ResetGame();
    }
    public void ToCharacterInfo(int index)
    {
        GameManager.Instance.ToCharacterInfoScene(index);
    }
    public void CloseApp()
    {
        GameManager.Instance.CloseApp();
    }

    //int what player you are selecting for
    int playerSelecting = 1;
    bool characterOnePlayer = true;
    bool characterTwoPlayer = false;
    //toggle what player you are selecting for (int param for player)
    public void SetPlayerBeingSelected(int playerToSelect)
    {
        playerSelecting = playerToSelect;
        if (playerSelecting == 1)
        {
            characterOneSelectedButton.interactable = false;
            characterTwoSelectedButton.interactable = true;
        }
        else
        {
            characterOneSelectedButton.interactable = true;
            characterTwoSelectedButton.interactable = false;
        }
    }
    //set player (int param) to be a player
    public void SetPlayerToPlayer(int playerToChange)
    {
        switch (playerToChange)
        {
            case 1:
                //set player 1 to be a player
                characterOnePlayer = true;
                characterOnePlayerButton.interactable = !characterOnePlayer;
                characterOneAIButton.interactable = characterOnePlayer;
                break;
            case 2:
                //Set player 2 to be a player
                characterTwoPlayer = true;
                characterTwoPlayerButton.interactable = !characterOnePlayer;
                characterTwoAIButton.interactable = characterOnePlayer;
                break;
            default:
                Debug.Log("Invalid Player Selected");
                break;
        }
    }
    //set player (int param) to be a AI
    public void SetPlayerToAI(int playerToChange)
    {
        switch (playerToChange)
        {
            case 1:
                //Set player 1 to be an AI
                characterOnePlayer = false;
                characterOnePlayerButton.interactable = !characterOnePlayer;
                characterOneAIButton.interactable = characterOnePlayer;
                break;
            case 2:
                //Set player 2 to be an AI
                characterTwoPlayer = false;
                characterTwoPlayerButton.interactable = !characterOnePlayer;
                characterTwoAIButton.interactable = characterOnePlayer;
                break;
            default:
                Debug.Log("Invalid Player Selected");
                break;
        }
    }
}
