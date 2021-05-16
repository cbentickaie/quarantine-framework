using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowObjectiveExample : MonoBehaviour {

    [SerializeField] string ObjectiveTitle = "Your objective is to graps the ramblings of Craig!";

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (PlayerHudManager.instance != null)
            {
                PlayerHudManager.instance.DisplayPlayerObjective(ObjectiveTitle);
            }
        }
    }
}
