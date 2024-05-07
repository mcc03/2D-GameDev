using UnityEngine;
using System;
using UnityEngine.UI;
using LootLocker.Requests;
using UnityEngine.SceneManagement;
using TMPro;

namespace LootLocker {
public class WhiteLabelLogin : MonoBehaviour
{
    // Input fields
    [Header("New User")]
    public InputField newUserEmailInputField;
    public InputField newUserPasswordInputField;

    [Header("Existing User")]
    public InputField existingUserEmailInputField;
    public InputField existingUserPasswordInputField;

    public Text infoText;

    [Header("Button")]
    public GameObject continueButton;

    private void Awake()
    {
        LootLockerSettingsOverrider.OverrideSettings();
        continueButton = GameObject.Find("ContinueButton");
        Debug.Log("API key set");
    }

    // Called when pressing "Log in"
    public void Login()
    {
        string email = existingUserEmailInputField.text;
        string password = existingUserPasswordInputField.text;
        LootLockerSDKManager.WhiteLabelLogin(email, password, false, loginResponse =>
        {
            Debug.Log("--------------------------------------------------");
            infoText.text = "test";
            Debug.Log("-------------------------------------------------------");
            //AssignName();

            if (!loginResponse.success)
            {
                // Error
                infoText.text = "Error logging in:" + loginResponse.errorData.message;
            
                Debug.Log("------------------------------Error loggin in-----------------------------");
                return;
            }
            else
            {
                infoText.text = "Logging in...";
            }
            if (continueButton != null)
            {
                continueButton.SetActive(true);
                Debug.Log("Continue Button found and activated.");
            }
            else
            {
                Debug.LogWarning("Continue Button not found.");
            }

            // Is the account verified?
            if (loginResponse.VerifiedAt == null)
            {
                // Stop here if you want to require your players to verify their email before continuing,
                // verification must also be enabled on the LootLocker dashboard
            }

            startSession(email);
            //AssignName(email);
            
        });
    }

    public void startSession(string playerName = null)
    {
        // Player is logged in, now start a game session
            LootLockerSDKManager.StartWhiteLabelSession((startSessionResponse) =>
            {
                if (startSessionResponse.success)
                {
                    // Session was succesfully started;
                    // After this you can use LootLocker features
                    infoText.text = "Session started successfully";
                    if(playerName != null){
                        AssignName(playerName);
                    }
                    
                }
                else
                {
                    // Error
                    infoText.text = "Error starting LootLocker session:" + startSessionResponse.errorData.message;
                }
            });
    }

    // Called when pressing "Create account"
    public void CreateAccount()
    {
        string email = newUserEmailInputField.text;
        string password = newUserPasswordInputField.text;

        LootLockerSDKManager.WhiteLabelSignUp(email, password, (response) =>
        {
            if (!response.success)
            {
                infoText.text = "Error signing up:"+response.errorData.message;

                return;
            }
            else
            {
                // Succesful response
                // AssignName(existingUserEmailInputField.text.ToString());
                
                infoText.text = "Account created";
                startSession(email);
            }
        });
    }

    public void AssignName(string name)
    {
        LootLockerSDKManager.SetPlayerName(name, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully set player name");
            } else
            {
                Debug.Log("Error setting player name");
            }
        });
    }


    // verification
    // public void ResendVerificationEmail()
    // {
    //     // Player ID can be retrieved when starting a session or creating an account.
    //     int playerID = 0;
    //     // Request a verification email to be sent to the registered user, 
    //     LootLockerSDKManager.WhiteLabelRequestVerification(playerID, (response) =>
    //     {
    //         if(response.success)
    //         {
    //             Debug.Log("Verification email sent!");
    //         }
    //         else
    //         {
    //             Debug.Log("Error sending verification email:" + response.errorData.message);
    //         }

    //     });
    // }

    // public void SendResetPassword()
    // {
    //     // Sends a password reset-link to the email
    //     LootLockerSDKManager.WhiteLabelRequestPassword("email@email-provider.com", (response) =>
    //     {
    //         if(response.success)
    //         {
    //             Debug.Log("Password reset link sent!");
    //         }
    //         else
    //         {
    //             Debug.Log("Error sending password-reset-link:" + response.errorData.message);
    //         }
    //     });
    // }
}
}
