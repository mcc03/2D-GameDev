using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{

    
    public static CharacterSelector instance; //stores the instance of the class
    public CharacterScriptableObject characterData; //stores selected character data

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); //so character is not destroyed when scene changes
        }
        else
        {
            Debug.LogWarning("EXTRA" + this + "DELETED");
            Destroy(gameObject);
        }
    }

    public static CharacterScriptableObject GetData() //used to get the seleceted character data from other scripts
    {
        return instance.characterData;
    }

    public void SelectCharacter(CharacterScriptableObject character)
    {
        characterData = character;
    }

    public void DestroySingleton() //destroys unwanted game objects in the scene
    {
        instance = null;
        Destroy(gameObject);
    }
}
