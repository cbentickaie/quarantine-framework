using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringMaterial : MonoBehaviour
{
    //Gather initial variables to allow for user editing of light appearance
    Material sourceMaterial;
    [SerializeField] Color initialColour;

    //Prepare for the Light Component we wish to draw our Intensity Value from
    [SerializeField]Light sourceLight;

    //Provide an option for the user to Auto-Disable Shadow Casting, this ensures that the Mesh doesn't interfere with the Light
    [SerializeField] bool disableShadows = true;

    //Provide an Option for the user to use the Light's Colour for the Emissive Colour
    [SerializeField] bool useLightColour = true;

    // Start is called before the first frame update
    void Start()
    {
        ValidateLightSource();
        if (sourceLight) 
        {

            if (sourceMaterial = GetComponent<MeshRenderer>().material)
            {

                if (disableShadows) 
                {
                    DisableShadowCasting();
                }
                if (useLightColour)
                {
                    //Store material's initial Emissive Colour
                    initialColour = sourceLight.color;
                }
                else 
                {
                    //Store material's initial Emissive Colour
                    initialColour = sourceMaterial.GetColor("_EmissionColor");
                }
            }            
        }
    

    }

    // Update is called once per frame
    void Update()
    {
        sourceMaterial.SetColor("_EmissionColor", (initialColour * sourceLight.intensity));
        print(sourceLight.intensity);
    }

    private void ValidateLightSource() 
    {
        if (!sourceLight) 
        {
            if (sourceLight = GetComponent<Light>())
            {
                return;
            }
            else if (sourceLight = GetComponentInParent<Light>())
            {
                return;
            } else if (sourceLight = GetComponentInChildren<Light>()) 
            {
                return;
            }
            else
            {
                print("There is no Light component found, please specify one in the Inspector ``OR`` You can Parent or Child this Mesh Renderer to a Gameobject with a Light Component, set to Realtime or Mixed ");
            }
        }
    }
    private void DisableShadowCasting() 
    {
        GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }
}
