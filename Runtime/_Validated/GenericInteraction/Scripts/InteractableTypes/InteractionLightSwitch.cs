using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionLightSwitch : Interactable
{
    public Light targetLight;

    private FlickeringLight flickerComp;

    private void Awake()
    {
        if (!targetLight)
        {
            if (targetLight = GetComponent<Light>())
            {

            }
            else if (!targetLight) 
            {
                print("This InteractableLightSwitch has no Light Specified, and cannot find a LightComponent on the Gameobject it is attached to. Please add this component to the Light your wish to affect or specific the light in the Inspector.");
            }
        }
    }

    public override void StartInteraction(bool isPlayer = false)
    {
        base.StartInteraction();

        if (flickerComp = targetLight.gameObject.GetComponent<FlickeringLight>())
        {
            flickerComp.toggleLight();
        }
        else 
        {
            targetLight.enabled = (!targetLight.enabled);
        }
    }
}
