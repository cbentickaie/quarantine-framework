using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemInventory : MonoBehaviour
{
    int activeItemIndex;

    public int bulletAmmocount = 32;
    public int grenadeAmmoCount = 6;

    public List<GameObject> heldItems;
    [SerializeField] bool AutoGatherHeldItems = false;
    // Start is called before the first frame update
    void Awake()
    {
        if (AutoGatherHeldItems) 
        {
            heldItems.Clear();
            foreach (Transform child in GetComponentInChildren<Camera>().gameObject.transform) 
            {
                heldItems.Add(child.gameObject);
            }
        }
    }


    bool hasAmmo;

    public void RemoveAmmo(ammoTypes ammo, int ammoUsed = 1)
    {
        switch (ammo)
        {

            case ammoTypes.Bullets:
                bulletAmmocount--;
                if (bulletAmmocount < 0)
                {
                    bulletAmmocount = 0;
                }
                break;

            case ammoTypes.Grenades:
                grenadeAmmoCount--;
                if (grenadeAmmoCount < 0)
                {
                    grenadeAmmoCount = 0;
                }
                break;
        }

    }
    public bool checkAmmo(ammoTypes ammo)
    {
        

        switch (ammo)
        {

            case ammoTypes.Bullets:
                Debug.Log("Checkling Bullet Ammot");
                if (bulletAmmocount > 0)
                {
                    hasAmmo = true;
                }
                else
                {
                    hasAmmo = false;
                }
                break;

            case ammoTypes.Grenades:
                Debug.Log("Checkling 'Nades!");
                if (grenadeAmmoCount > 0)
                {
                    hasAmmo = true;
                }
                else
                {
                    hasAmmo = false;
                }
                break;
        }
        return hasAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        EvaluateItemSwap();
    }

    #region Item Switching
    

    void swapItem(int index)
    {
        if (index < heldItems.Count)
        {
            heldItems[activeItemIndex].SetActive(false);
            heldItems[index].SetActive(true);
            activeItemIndex = index;
        }
        else { print("No valid HeldItem at that Index"); }
    }
    #endregion

    #region Input Mapping - Hacky
    //This is pretty heinous but it works, just dont look at it too closely :S
    void EvaluateItemSwap() 
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            swapItem(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            swapItem(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            swapItem(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            swapItem(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            swapItem(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            swapItem(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            swapItem(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            swapItem(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            swapItem(8);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            swapItem(9);
        }
    }
#endregion
}