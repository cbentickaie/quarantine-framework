using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPromptExample : MonoBehaviour {



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (PlayerHudManager.instance != null)
            {
                PlayerHudManager.instance.DisplayPlayerPrompt("This is a Prompt that Times Out...", false);
            }
        }
    }
}
