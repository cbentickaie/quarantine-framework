using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_CheckCommodityToUnlockDoor : MonoBehaviour
{
    [SerializeField]
    MultiDoor DoorToUnlock;
    public PickupTypes Commodity;
    public float RequiredAmount = 1;
    public bool disableDirectInteractionOnUnlock = false;
    public string commodityName = "Keys";
    public bool hideLocksOnUnlock = false;
    public GameObject lockGameObject;
    public bool openDoorOnUnlock = false;
    public bool showMessageOnUnlock = false;

    private void Awake()
    {
        if (!DoorToUnlock) 
        {
            DoorToUnlock = gameObject.GetComponent<MultiDoor>();
        }        
    }


    void disableDirectInteraction() 
    {
        gameObject.GetComponent<Collider>().enabled = false;
    }

    void hideLockObjects() 
    {
        if (lockGameObject) 
        {
            lockGameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && DoorToUnlock.isLocked == true) 
        {                        
            switch (Commodity)
            {
                case PickupTypes.keyPickup:
                    if (PlayerController.instance.currentKeys >= RequiredAmount)
                    {
                        if (disableDirectInteractionOnUnlock)
                        {
                            disableDirectInteraction();
                        }
                        if (hideLocksOnUnlock)
                        {
                            hideLockObjects();
                        }
                        if (showMessageOnUnlock)
                        {
                            PlayerHudManager.instance.DisplayPlayerPrompt("The Door is now Unlocked", true, 2f);
                        }
                        DoorToUnlock.UnlockDoor();
                        if (openDoorOnUnlock)
                        {
                            DoorToUnlock.OpenDoor();
                        }
                    }
                    break;

                case PickupTypes.health:
                    if (PlayerController.instance.currentHealth >= RequiredAmount) 
                    {
                        if (disableDirectInteractionOnUnlock)
                        {
                            disableDirectInteraction();
                        }
                        if (hideLocksOnUnlock) 
                        {
                            hideLockObjects();
                        }
                        DoorToUnlock.UnlockDoor();
                        if (openDoorOnUnlock) 
                        {
                            DoorToUnlock.OpenDoor();
                        }
                        if (showMessageOnUnlock) 
                        {
                            PlayerHudManager.instance.DisplayPlayerPrompt("The Door is now Unlocked", true, 2f);
                        }
                        
                    }
                    break;

                case PickupTypes.xp:
                    if (PlayerController.instance.currentXp >= RequiredAmount)
                    {
                        if (disableDirectInteractionOnUnlock)
                        {
                            disableDirectInteraction();
                        }
                        if (hideLocksOnUnlock)
                        {
                            hideLockObjects();
                        }
                        if (showMessageOnUnlock)
                        {
                            PlayerHudManager.instance.DisplayPlayerPrompt("The Door is now Unlocked", true, 2f);
                        }
                        DoorToUnlock.UnlockDoor();
                        if (openDoorOnUnlock)
                        {
                            DoorToUnlock.OpenDoor();
                        }
                    }
                    break;
                case PickupTypes.money:
                    if (PlayerController.instance.currentMoney >= RequiredAmount)
                    {
                        if (disableDirectInteractionOnUnlock)
                        {
                            disableDirectInteraction();
                        }
                        if (hideLocksOnUnlock)
                        {
                            hideLockObjects();
                        }
                        if (showMessageOnUnlock)
                        {
                            PlayerHudManager.instance.DisplayPlayerPrompt("The Door is now Unlocked", true, 2f);
                        }
                        DoorToUnlock.UnlockDoor();
                        if (openDoorOnUnlock)
                        {
                            DoorToUnlock.OpenDoor();
                        }
                    }
                    break;
                case PickupTypes.ammo:
                    if (PlayerController.instance.currentAmmo >= RequiredAmount)
                    {
                        if (disableDirectInteractionOnUnlock)
                        {
                            disableDirectInteraction();
                        }
                        if (hideLocksOnUnlock)
                        {
                            hideLockObjects();
                        }
                        if (showMessageOnUnlock)
                        {
                            PlayerHudManager.instance.DisplayPlayerPrompt("The Door is now Unlocked", true, 2f);
                        }
                        DoorToUnlock.UnlockDoor();
                        if (openDoorOnUnlock)
                        {
                            DoorToUnlock.OpenDoor();
                        }
                    }
                    break;
                case PickupTypes.popularity:
                    if (PlayerController.instance.currentPopularity >= RequiredAmount)
                    {
                        if (disableDirectInteractionOnUnlock)
                        {
                            disableDirectInteraction();
                        }
                        if (hideLocksOnUnlock)
                        {
                            hideLockObjects();
                        }
                        if (showMessageOnUnlock)
                        {
                            PlayerHudManager.instance.DisplayPlayerPrompt("The Door is now Unlocked", true, 2f);
                        }
                        DoorToUnlock.UnlockDoor();
                        if (openDoorOnUnlock)
                        {
                            DoorToUnlock.OpenDoor();
                        }
                    }
                    break;
                case PickupTypes.notoriety:
                    if (PlayerController.instance.currentNotoriety >= RequiredAmount)
                    {
                        if (disableDirectInteractionOnUnlock)
                        {
                            disableDirectInteraction();
                        }
                        if (hideLocksOnUnlock)
                        {
                            hideLockObjects();
                        }
                        if (showMessageOnUnlock)
                        {
                            PlayerHudManager.instance.DisplayPlayerPrompt("The Door is now Unlocked", true, 2f);
                        }
                        DoorToUnlock.UnlockDoor();
                        if (openDoorOnUnlock)
                        {
                            DoorToUnlock.OpenDoor();
                        }
                    }
                    break;
                case PickupTypes.points:
                    if (PlayerController.instance.currentPoints >= RequiredAmount)
                    {
                        if (disableDirectInteractionOnUnlock)
                        {
                            disableDirectInteraction();
                        }
                        if (hideLocksOnUnlock)
                        {
                            hideLockObjects();
                        }
                        if (showMessageOnUnlock)
                        {
                            PlayerHudManager.instance.DisplayPlayerPrompt("The Door is now Unlocked", true, 2f);
                        }
                        DoorToUnlock.UnlockDoor();
                        if (openDoorOnUnlock)
                        {
                            DoorToUnlock.OpenDoor();
                        }
                    }
                    break;
                case PickupTypes.armour:
                    if (PlayerController.instance.currentArmour >= RequiredAmount)
                    {
                        if (disableDirectInteractionOnUnlock)
                        {
                            disableDirectInteraction();
                        }
                        if (hideLocksOnUnlock)
                        {
                            hideLockObjects();
                        }
                        if (showMessageOnUnlock)
                        {
                            PlayerHudManager.instance.DisplayPlayerPrompt("The Door is now Unlocked", true, 2f);
                        }
                        DoorToUnlock.UnlockDoor();
                        if (openDoorOnUnlock)
                        {
                            DoorToUnlock.OpenDoor();
                        }
                    }
                    break;
                case PickupTypes.food:
                    if (PlayerController.instance.currentFood >= RequiredAmount)
                    {
                        if (disableDirectInteractionOnUnlock)
                        {
                            disableDirectInteraction();
                        }
                        if (hideLocksOnUnlock)
                        {
                            hideLockObjects();
                        }
                        if (showMessageOnUnlock)
                        {
                            PlayerHudManager.instance.DisplayPlayerPrompt("The Door is now Unlocked", true, 2f);
                        }
                        DoorToUnlock.UnlockDoor();
                        if (openDoorOnUnlock)
                        {
                            DoorToUnlock.OpenDoor();
                        }
                    }
                    break;
            }
            if (DoorToUnlock.isLocked) 
            {
                PlayerHudManager.instance.DisplayPlayerPrompt("This Door Requires " + RequiredAmount + " " + commodityName, false, 2f);
            }
            
        }
        
    }
}
