using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class C_LoadLevelButtonScript : MonoBehaviour
{

    public void LoadNewLevel(string newLevelName) 
    {
        SceneManager.LoadScene(newLevelName);
        Debug.Log("Button Pressed!");
    }
}
