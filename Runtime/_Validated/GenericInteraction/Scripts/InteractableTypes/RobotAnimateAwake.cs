using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAnimateAwake : Interactable
{
    bool isActive = false;

    public override void StartInteraction(bool isPlayer = false)
    {
        base.StartInteraction();
        print("Robot hit");
        if (!isActive)
        {
            GetComponent<Animator>().SetBool("isActive", true);
            isActive = true;
        }
        else
        {
            GetComponent<Animator>().SetBool("isActive", false);
            isActive = false;
        }

    }
}
