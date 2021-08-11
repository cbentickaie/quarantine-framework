using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_ShowHideTargetObjects : MonoBehaviour
{
    //list of objects to disable
    //list of objects to enable
    public GameObject[] ObjectsToDisable;
    public GameObject[] ObjectsToEnable;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            //disable all cosmetic objects
            foreach (GameObject item in ObjectsToDisable)
            {
                item.SetActive(false);
            }
            //Enable all collectible objects
            foreach (GameObject item in ObjectsToEnable)
            {
                item.SetActive(true);
            }
        }
    }
}
