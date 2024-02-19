using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //list of enums for states of the game
    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver
    }

    // stores current state of the game
    public GameState currentState;
    // store previous state
    public GameState previousState;

    [Header("UI")]
    public GameObject pauseScreen;

    void Awake()
    {
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
            //code for gameplay state
                break;

            case GameState.Paused:
            CheckForPauseAndResume();
            //code for paused state
                break;

            case GameState.GameOver:
            //code for game over state
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

    void DisableScreens()
    {
        pauseScreen.SetActive(false);
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
