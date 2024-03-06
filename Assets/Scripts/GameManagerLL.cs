using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

public class GameManagerLL : MonoBehaviour
{

    public string playerID = "";


    void Start()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (!response.success)
            {
                Debug.Log("error starting LootLocker session");
                
                return;
            }

            Debug.Log("successfully started LootLocker session");
            playerID = response.player_id.ToString();
            Debug.Log(response.player_id.ToString());
            //TestSubmitLeaderboard(response.player_id.ToString(), 1000);
        });
        
    }

    public void TestSubmitLeaderboard(){

        string leaderboardID = "top_players";
        int count = 50;

        Debug.Log("Getting leaderboard");
        LootLockerSDKManager.GetScoreList(leaderboardID, count, 0, (response) =>
        {
            if (response.statusCode == 200) {
                Debug.Log("Successfullt got leaderboard");
                Debug.Log(response);
            } else {
                Debug.Log("Error getting score:" + response.errorData.message);
            }
        });

        Debug.Log("Submitting score");
        int score = EnemyStats.playerScoreKills; // setting score
        string metadata = Application.systemLanguage.ToString();

        LootLockerSDKManager.SubmitScore(playerID, score, leaderboardID, (response) =>
        {
            if (response.statusCode == 200) {
                Debug.Log("Successfully submitted score of " + score);
            } else {
                Debug.Log("Error submitting score:" + response.errorData.message);
            }
        });
    }

    public void TestOnClick()
    {
        Debug.Log("Clicked from results screen");
    }

    // unity onClick functions can only contain one parameter, this is why this function exists
    public void SubmitScoreOnClick()
    {
        TestSubmitLeaderboard();
    }
}