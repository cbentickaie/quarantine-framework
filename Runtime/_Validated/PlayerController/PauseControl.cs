using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseControl : MonoBehaviour
{
    public static bool gameIsPaused = false;
    PlayerController Pcon;

    private void Start()
    {
        Pcon = PlayerController.instance;
    }

    void Update()
    {
        if (Input.GetButtonDown("PauseGame"))
        {
            togglePause();
        }
    }
    public void togglePause() 
    {
        gameIsPaused = !gameIsPaused;
        PauseGame();
    }
    public void PauseGame()
    {
        if (gameIsPaused)
        {
            Time.timeScale = 0;
            if (Pcon)
            {
                //Pcon.disablePlayerPawn
                Pcon.PausePlayer(true);
            }
        }
        else
        {
            Time.timeScale = 1;
            if (Pcon)
            {
                Pcon.PausePlayer(false);
            }
        }
   

    }
}
