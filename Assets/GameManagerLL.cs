using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

public class GameManagerLL : MonoBehaviour
{
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
            Debug.Log(response.player_id.ToString());
            testSubmitLeaderboard(response.player_id.ToString(), 1000);
        });
        
    }

    void testSubmitLeaderboard(string playerID, int playerScore){

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
        string memberID = playerID;
        int score = playerScore;
        string metadata = Application.systemLanguage.ToString();

        LootLockerSDKManager.SubmitScore(memberID, score, leaderboardID, (response) =>
        {
            if (response.statusCode == 200) {
                Debug.Log("Successfully submitted score");
            } else {
                Debug.Log("Error submitting score:" + response.errorData.message);
            }
        });
    }




}