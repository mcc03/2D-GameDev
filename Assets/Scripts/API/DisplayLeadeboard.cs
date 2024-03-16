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

    // exusting player IDs
    ulong player1ID = 5914478;
    ulong player2ID = 5905490;
    ulong player3ID = 5914407;
    int[] playersIDList = { 5918558, 5914478, 5918601, 5905393, 5905492, 5905374, 5914438, 5905506, 5905508, 5883304 };

    List<int> myList = new List<int>();

    // void Awake()
    // {

    // }

    void Start()
    {
        /* 
         * Override settings to use the Example game setup
         */
        //LootLockerSettingsOverrider.OverrideSettings();
        //Debug.Log("waiting to start session");
        StartGuestSession();

        Debug.Log("==========Getting player names from ID=====================");
        ReturnNameFromID();

        Debug.Log("==========Adding ID To List=====================");
        ReturnNameAddToArray();
    }

    async public void StartGuestSession()
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

    public void ReturnNameFromID()
    {
        LootLockerSDKManager.LookupPlayerNamesByPlayerIds(new ulong[] { player1ID, player2ID, player3ID }, response =>
        {
            if (response.success)
            {
                foreach (var player in response.players)
                {
                    Debug.Log("player ID: " + player.player_id);
                    Debug.Log("player Public UID: " + player.player_public_uid);
                    Debug.Log("player Name: " + player.name);
                    Debug.Log("player Last Active Platform: " + player.last_active_platform);
                    Debug.Log("player Platform Player ID: " + player.platform_player_id);
                    Debug.Log("========================================================");
                }
            } else
            {
                Debug.Log("Error looking up player names");
            }
        }); 
    }

    public void ReturnNameAddToArray()
    {
        LootLockerSDKManager.LookupPlayerNamesByPlayerIds(new ulong[] { player1ID, player2ID, player3ID }, response =>
        {
            if (response.success)
            {
                foreach (var player in response.players)
                {
                    myList.Add((int)player.player_id);
                    Debug.Log("Player ID added to myList: " + player.player_id);
                    Debug.Log("Number of items in myList: " + myList.Count);
                    Debug.Log("========================================================");
                }
            } else
            {
                Debug.Log("Error looking up player names");
            }
        }); 
    }

    public void DisplayMyList()
    {
        for(var i = 0; i < myList.Count; i++)
        {
            Debug.Log(myList[i]);
        }
    }

    public void SearchList()
    {

        // need to make a new ulong from myList (ulong is used for positive and large numbers (64-bit), e.g. playerIDs)
        ulong[] playerIDs = new ulong[myList.Count];

        for (int i = 0; i < myList.Count; i++)
        {
            playerIDs[i] = (ulong)myList[i];
        }

            LootLockerSDKManager.LookupPlayerNamesByPlayerIds(playerIDs, response =>
        {
            if (response.success)
            {
                foreach (var player in response.players)
                {
                    Debug.Log("Player ID added to myList: " + player.name);
                    UpdateLeaderboardTop10();
                }
            } else
            {
                Debug.Log("Error looking up player names");
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

                string leaderboardText = "";

                for (int i = 0; i < response.items.Length; i++)
                {
                    LootLockerLeaderboardMember currentEntry = response.items[i];

                    // leaderboard entries
                    string playerName;
                    if (currentEntry.player.name != "")
                    {
                        playerName = currentEntry.player.name;
                    }
                    else
                    {
                        playerName = currentEntry.member_id;
                    }

                    string rankColor = RankColor(currentEntry.rank);
                    string leaderboardEntry = $"<color={rankColor}>{currentEntry.rank}. {playerName}</color> - {currentEntry.score}";

                    // next line
                    leaderboardText += leaderboardEntry + "\n";
                }

                leaderboardTop10Text.text = leaderboardText; // Assigning the text to leaderboardTop10Text
                Debug.Log("========================= Displaying score from leaderboard key: " + leaderboardKey);
            }
            else
            {
                infoText.text = "Error updating Top 10 leaderboard";
            }
        });
    }

    string RankColor(int rank)
    {
        // if the player rank value is between 1-3 it uses the cases below, if not it uses the default case
        // switch allows to run the specified blocks of code, so rank the rank 1 player will have case 1 executed
        switch (rank)
        {
            case 1:
                return "#FFD700"; // Gold
            case 2:
                return "#C0C0C0"; // Silver
            case 3:
                return "#CD7F32"; // Bronze
            default:
                return "white"; // default color for ranks beyond top 3
        }
    }

    public static void DumpToConsole(object obj)
    {
        var output = JsonUtility.ToJson(obj, true);
        Debug.Log(output);
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
