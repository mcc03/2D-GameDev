using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;
using UnityEngine.UI;
using System;
using TMPro;

namespace LootLocker {
public class GenericTypeLeaderboard : MonoBehaviour
{
    //public InputField scoreInputField;
    //public InputField playerNameInputField;
    public Text infoText;
    //public Text playerIDText;
    public TMP_Text leaderboardTop10Text;
    //public Text leaderboardCenteredText;

    /*
    * leaderboardKey or leaderboardID can be used.
    * leaderboardKey can be the same between stage and live /development mode on/off.
    * So if you use the key instead of the ID, you don't need to change any code when switching development_mode.
    */
    string leaderboardKey = "top_players";
    // int leaderboardID = 4705;

    string memberID;

    // Start is called before the first frame update
    void Start()
    {
        /* 
         * Override settings to use the Example game setup
         */
        //LootLockerSettingsOverrider.OverrideSettings();
        StartGuestSession();
        Debug.Log("hello from start");
    }

    public void StartGuestSession()
    {
        /* Start guest session without an identifier.
         * LootLocker will create an identifier for the user and store it in PlayerPrefs.
         * If you want to create a new player when testing, you can use PlayerPrefs.DeleteKey("LootLockerGuestPlayerID");
         */
        PlayerPrefs.DeleteKey("LootLockerGuestPlayerID");
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                infoText.text = "Guest session started";
                //playerIDText.text = "Player ID:" + response.player_id.ToString();
                memberID = response.player_id.ToString();
                UpdateLeaderboardTop10();
                //UpdateLeaderboardCentered();
                Debug.Log("hello from start guest");
            }
            else
            {
                infoText.text = "Error" + response.errorData.message;
            }
        });
    }

    void UpdateLeaderboardTop10()
    {
        LootLockerSDKManager.GetScoreList(leaderboardKey, 10, (response) =>
        {
            if (response.success)
            {
                infoText.text = "Top 10 leaderboard updated";
                infoText.text = "Centered scores updated";

                /*
                 * Format the leaderboard
                 */
                string leaderboardText = "";
                for (int i = 0; i < response.items.Length; i++)
                {
                    LootLockerLeaderboardMember currentEntry = response.items[i];
                    leaderboardText += currentEntry.rank + ".";
                    leaderboardText += currentEntry.metadata;
                    leaderboardText += " - ";
                    leaderboardText += currentEntry.score;
                    leaderboardText += "\n";
                }
                leaderboardTop10Text.text = leaderboardText;

                //Debug.Log("logging response" + response.success);
                Debug.Log("========================= Displaying score from leaderboard key: " + leaderboardKey);

            }
            else
            {
                infoText.text = "Error updating Top 10 leaderboard";
            }
        });
    }

    // Increment and save a string that goes from a to z, then za to zz, zza to zzz etc.
    string GetAndIncrementScoreCharacters()
    {
        // Get the current score string
        string incrementalScoreString = PlayerPrefs.GetString(nameof(incrementalScoreString), "a");

        // Get the current character
        char incrementalCharacter = PlayerPrefs.GetString(nameof(incrementalCharacter), "a")[0];

        // If the previous character we added was 'z', add one more character to the string
        // Otherwise, replace last character of the string with the current incrementalCharacter
        if (incrementalScoreString[incrementalScoreString.Length - 1] == 'z')
        {
            // Add one more character
            incrementalScoreString += incrementalCharacter;
        }
        else
        {
            // Replace character
            incrementalScoreString = incrementalScoreString.Substring(0, incrementalScoreString.Length - 1) + incrementalCharacter.ToString();
        }

        // If the letter int is lower than 'z' add to it otherwise start from 'a' again
        if ((int)incrementalCharacter < 122)
        {
            incrementalCharacter++;
        }
        else
        {
            incrementalCharacter = 'a';
        }

        // Save the current incremental values to PlayerPrefs
        PlayerPrefs.SetString(nameof(incrementalCharacter), incrementalCharacter.ToString());
        PlayerPrefs.SetString(nameof(incrementalScoreString), incrementalScoreString.ToString());

        // Return the updated string
        return incrementalScoreString;
    }
}
}
