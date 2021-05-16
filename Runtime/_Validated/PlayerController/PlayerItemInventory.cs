using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemInventory : MonoBehaviour
{
    int activeItemIndex;

    public int bulletAmmocount = 32;
    public int grenadeAmmoCount = 6;
    // Start is called before the first frame update
    void Start()
    {
        
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
    }

    #region Item Switching
    public List<GameObject> heldItems;

    void swapItem(int index)
    {
        heldItems[activeItemIndex].SetActive(false);
        heldItems[index].SetActive(true);
        activeItemIndex = index;
    }
    #endregion
}
