﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowObjectiveCompleteExample : MonoBehaviour {

    
    

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (PlayerHudManager.instance != null)
            {
                PlayerHudManager.instance.DisplayObjectiveComplete();
            }
        }
    }
}
