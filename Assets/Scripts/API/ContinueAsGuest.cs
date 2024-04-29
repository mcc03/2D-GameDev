using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LootLocker.Requests;

    public class ContinueAsGuest : MonoBehaviour
    {


        public string playerID = "";

        // Start is called before the first frame update
        public void StartAsGuest()
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
    }

