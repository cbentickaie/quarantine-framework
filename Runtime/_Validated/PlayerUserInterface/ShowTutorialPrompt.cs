using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTutorialPrompt : MonoBehaviour {

    public string tutorialPrompt = "This is a Tutorial Prompt";
    public float PromptDuration = 3.14f;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (PlayerHudManager.instance != null)
            {
                PlayerHudManager.instance.DisplayPlayerPrompt(tutorialPrompt, true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (PlayerHudManager.instance != null)
            {
                PlayerHudManager.instance.StartCoroutine(PlayerHudManager.instance.HidePlayerPrompt(PromptDuration));
            }
        }
    }
}
