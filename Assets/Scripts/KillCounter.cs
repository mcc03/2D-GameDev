using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KillCounter : MonoBehaviour
{
    [Header("Score Tracker")]
    public TMP_Text killCounterText;
    int kills;

    public void AddKill()
    {
        kills++;
    }

    public void ShowKills()
    {
        killCounterText.text = kills.ToString();
    }

}
