﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_QuitGameTrigger : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {

            if (collision.gameObject.tag == "Player")
            {
            Application.Quit();
            }
    }

    public void ExitGameDirect() 
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_WEBPLAYER
                 Application.OpenURL(webplayerQuitURL);
        #else
                 Application.Quit();
        #endif
    }
}
