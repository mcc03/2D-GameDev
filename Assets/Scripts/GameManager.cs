using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //list of enums for states of the game
    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver,
        LevelUp
    }

    // stores current state of the game
    public GameState currentState;
    // store previous state
    public GameState previousState;

    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;
    public GameObject levelUpScreen;

    // current stat display
    [Header("Current Stat Displays")]
    public Text currentHealthDisplay;
    public Text currentRecoveryDisplay;
    public Text currentMoveSpeedDisplay;
    public Text currentMightDisplay;
    public Text currentProjectileSpeedDisplay;
    public Text currentMagnetDisplay;

    [Header("Results Screen Displays")]
    public Image chosenCharacterImage;
    public Text chosenCharacterName;
    public Text levelReachedDisplay;
    public Text timeSurvivedDisplay;
    public List<Image> chosenWeaponsUI = new List<Image>(6);
    public List<Image> chosenPassiveItemsUI = new List<Image>(6);

    [Header("Stopwatch")]
    public float timeLimit;
    float stopwatchTime; // track elapsed time
    public Text stopwatchDisplay;

    // check if the game is over
    public bool isGamerOver = false;

    // check if the player is choosing an upgrade
    public bool choosingUpgrade = false;

    // reference to the player object
    public GameObject playerObject;

    void Awake()
    {
        // warning check if there is another instance of this kind
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Extra" + this + "deleted");
        }

        DisableScreens();
    }

    // update game based on the current state
    void Update()
    {
        // TestSwtichState();

        switch (currentState)
        {
            case GameState.Gameplay:
            CheckForPauseAndResume();
            UpdateStopwatch();
                break;

            case GameState.Paused:
            CheckForPauseAndResume();
            //code for paused state
                break;

            case GameState.GameOver:
            if(!isGamerOver)
            {
                isGamerOver = true;
                Time.timeScale = 0f; // stop the game
                Debug.Log("Game Over");
                DisplayResults(); // display results of the run
            }
                break;

            case GameState.LevelUp:
            if(!choosingUpgrade)
            {
                choosingUpgrade = true;
                Time.timeScale = 0f; // pause game when choosing upgrade
                Debug.Log("Upgrades available");
                levelUpScreen.SetActive(true);
            }
            break;

                default:
                    Debug.LogWarning("State does not exist");
                    break;
        }
    }

    // method to change state of the game
    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }

    public void PauseGame()
    {

        if(currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused); // switch state to paused
            Time.timeScale = 0f; // pauses the game
            pauseScreen.SetActive(true);
            Debug.Log("Game paused");
        }
    }

    public void ResumeGame()
    {
        if(currentState == GameState.Paused)
        {
            ChangeState(previousState);
            Time.timeScale = 1f; // resumes the game
            pauseScreen.SetActive(false);
            Debug.Log("Game resumed");
        }
    }

    // method for pausing and resuming
    void CheckForPauseAndResume()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(currentState == GameState.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void DisableScreens() // disables UI elements at the start of the game
    {
        pauseScreen.SetActive(false);
        resultsScreen.SetActive(false);
        levelUpScreen.SetActive(false);
    }

    public void GameOver()
    {
        timeSurvivedDisplay.text = stopwatchDisplay.text; // don't have to track it, just assign it at the end
        ChangeState(GameState.GameOver);
    }

    void DisplayResults()
    {
        resultsScreen.SetActive(true);
    }

    public void AssignChosenCharacterUI(CharacterScriptableObject chosenCharacterData) // assigns values to the character variables
    {
        chosenCharacterImage.sprite = chosenCharacterData.Icon;
        chosenCharacterName.text = chosenCharacterData.name;
    }

    public void AssignLevelReachedUI(int levelReachedData)
    {
        levelReachedDisplay.text = levelReachedData.ToString();
    }

    public void AssignChosenWeaponsAndPassiveItemsUI(List<Image> chosenWeaponsData, List<Image> chosenPassiveItemsData)
    {
        if(chosenWeaponsData.Count != chosenWeaponsUI.Count || chosenPassiveItemsData.Count != chosenPassiveItemsUI.Count)
        {
            Debug.Log("Chosen weapons and passive items data lists have different lengths");
            return;
        }

        // iterate through weapons and assign the data
        for (int i = 0; i < chosenWeaponsUI.Count; i++)
        {
            if(chosenWeaponsData[i].sprite)
            {
                // if the field has a sprite assigned to it, enable the sprite
                chosenWeaponsUI[i].enabled = true;
                chosenWeaponsUI[i].sprite = chosenWeaponsData[i].sprite;
            }
            else 
            {
                chosenWeaponsUI[i].enabled = false; // if not, do not display
            }
        }

        // iterate through passive items and assign the data
        for (int i = 0; i < chosenPassiveItemsUI.Count; i++)
        {
            if(chosenPassiveItemsData[i].sprite)
            {
                // if the field has a sprite assigned to it, enable the sprite
                chosenPassiveItemsUI[i].enabled = true;
                chosenPassiveItemsUI[i].sprite = chosenPassiveItemsData[i].sprite;
            }
            else 
            {
                chosenPassiveItemsUI[i].enabled = false; // if not, do not display
            }
        }
    }

    void UpdateStopwatch()
    {
        // increment time
        stopwatchTime += Time.deltaTime;

        UpdateStopwatchDisplay();

        // trigger game over method once time limit is reached
        if(stopwatchTime >= timeLimit)
        {
            GameOver();
        }
    }

    void UpdateStopwatchDisplay()
    {
        int minutes = Mathf.FloorToInt(stopwatchTime / 60);  // rounds down to nearest integer
        int seconds = Mathf.FloorToInt(stopwatchTime % 60); // finds remainder when divided by 60

        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds); // convert to string and display
    }

    public void StartLevelUp()
    {
        ChangeState(GameState.LevelUp); // change game state to level up  
        playerObject.SendMessage("RemoveAndApplyUpgrades"); // call the method without having to reference the whole inventroy manager script
    }

    public void EndLevelUp()
    {
        choosingUpgrade = false;
        Time.timeScale = 1f; // resume once upgrade has been chosen
        levelUpScreen.SetActive(false); // disable UI element
        ChangeState(GameState.Gameplay); // change back to gameplay game state
    }

    // void TestSwtichState()
    // {
    //     if(Input.GetKeyDown(KeyCode.E))
    //     {
    //         currentState++;        
    //     }
    //     else if(Input.GetKeyDown(KeyCode.Q))
    //     {
    //         currentState--;
    //     }
    // }
}
