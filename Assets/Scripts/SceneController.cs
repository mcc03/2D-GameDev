using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//this script handles scene changes from the menu (character selection)
public class SceneController : MonoBehaviour
{

    public void SceneChange(string name)
    {
        SceneManager.LoadScene(name);
        Time.timeScale = 1; // resume game
    }
}
