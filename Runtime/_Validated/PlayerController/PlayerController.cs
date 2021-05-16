using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Camera))]

public class PlayerController : MonoBehaviour {

    [Header("Player Pawn Types")]    
    public List<GameObject> AvatarTypes;
    [Space(10)]

    int AvatarIndex = 0;
    public List<GameObject> AvatarInstances;
    public string PlayerTag = "Player";
    Camera PconCamera;
    public float statDepletionRate = 3.14f;
    DamageHandler playerDH;

    public bool reloadOnPlayerDeath = false;

    //Singleton Setup
    public static PlayerController instance = null; //Static instance of PlayerHudManager which allows it to be accessed by any other script.
    Vector3 ControllerStartLocation;
    Quaternion ControllerStartRotation;
    [ExecuteInEditMode]
    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }
        //If instance already exists and it's not this:
        else if (instance != this) 
        {
            //Store information about the specific Controller in this scene and Arrange the Player and Controller to the appropriate locations
            ControllerStartLocation = instance.gameObject.transform.position;
            ControllerStartRotation = instance.gameObject.transform.rotation;

            instance.gameObject.transform.position = this.transform.position;
            instance.gameObject.transform.rotation = this.transform.rotation;
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }



        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        if (GameObject.FindGameObjectWithTag("Player"))
        {
            //Capture any pre-existing player pawns in the scene
            AvatarInstances.Add(GameObject.FindGameObjectWithTag("Player"));
            AvatarIndex = 0;
            AvatarInstances[AvatarIndex].transform.position = this.transform.position;
            AvatarInstances[AvatarIndex].transform.rotation = this.transform.rotation;
            //SwitchAvatarToGaemObject(AvatarIndex[0]);
            //SwitchAvatarToGaemObject(GameObject.FindGameObjectWithTag("Player"));
        }
        else if (!GameObject.FindGameObjectWithTag("Player"))
        {
            Debug.Log("No GameObject with the Player tag found. Spawning Player Prefabs to Enable Play");
            //this.enabled = false;
            //Spawn a pool of Avatars for the player to switch between
            foreach (GameObject prefab in AvatarTypes)
            {
                GameObject tempAvatar = Instantiate(prefab);
                //GameObject tempAvatar = Instantiate(prefab, this.transform);
                tempAvatar.SetActive(false);
                //print(tempAvatar.name);
                AvatarInstances.Add(tempAvatar);
            }
            AvatarInstances[AvatarIndex].transform.position = this.transform.position;
            AvatarInstances[AvatarIndex].transform.rotation = this.transform.rotation;
            AvatarInstances[AvatarIndex].SetActive(true);
            AvatarInstances[AvatarIndex].tag = PlayerTag;
        }
    }
    
    // Use this for initialization
    void Start ()
    {
        PconCamera = gameObject.GetComponent<Camera>();

        currentHealth = maxHealth;
        if (playerDH = AvatarInstances[AvatarIndex].GetComponent<DamageHandler>())
        {
            playerDH.CurrentHealth = currentHealth;
        }
        PlayerHudManager.instance.UpdateStatsPanel();
        PlayerHudManager.instance.DoScreenFade(true);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            switchAvatar();
            print("You hit C");
        }

        tickDownStat(PickupTypes.notoriety);
        tickDownStat(PickupTypes.popularity);
    }

    

    //enable or disable the current player
    public void disablePlayerPawn(bool disable)
    {
        print("disabling player: " + disable);
        //store the current pawn's camera transform
        Transform sourceCamera;
        sourceCamera = AvatarInstances[AvatarIndex].GetComponentInChildren<Camera>().transform;
        PconCamera.transform.SetPositionAndRotation(sourceCamera.position, sourceCamera.rotation);
        PconCamera.gameObject.tag = "MainCamera";
        PconCamera.enabled = !disable;
        AvatarInstances[AvatarIndex].GetComponent<Rigidbody>().isKinematic = disable;

        AvatarInstances[AvatarIndex].SetActive(disable);        
    }

    //This Area controls switching between different Avatars or Pawns during gameplay
    #region Avatar Switching
    void switchAvatar()
    {
        if (AvatarIndex + 1 < AvatarInstances.Count)
        {
            AvatarInstances[AvatarIndex + 1].transform.SetPositionAndRotation(AvatarInstances[AvatarIndex].transform.position, AvatarInstances[AvatarIndex].transform.rotation);
            AvatarInstances[AvatarIndex].SetActive(false);
            AvatarInstances[AvatarIndex].tag = "Untagged";
            AvatarIndex++;
            AvatarInstances[AvatarIndex].SetActive(true);
            AvatarInstances[AvatarIndex].tag = PlayerTag;
        }
        else
        {             
            AvatarInstances[0].transform.SetPositionAndRotation(AvatarInstances[AvatarIndex].transform.position, AvatarInstances[AvatarIndex].transform.rotation);
            AvatarInstances[AvatarIndex].SetActive(false);
            AvatarInstances[AvatarIndex].tag = "Untagged";
            AvatarIndex = 0;
            AvatarInstances[AvatarIndex].SetActive(true);
            AvatarInstances[AvatarIndex].tag = PlayerTag;
        }
    }

    public void SwitchAvatarToGaemObject(GameObject newPlayerObject)
    {
        AvatarInstances[AvatarIndex].SetActive(false);
        
        newPlayerObject.SetActive(true);
        newPlayerObject.tag = PlayerTag;
        AvatarInstances.Add(newPlayerObject);

        //TODO: make this resume from previous index
        AvatarIndex = 0;
    }
    #endregion

    //Player Stats
    #region Player Stats
        
    public float currentHealth = 100.0f;
    public float maxHealth = 100f;

    public int currentLevel = 1;
    public float currentXp = 0;
    public float xpToNextLevel = 100;

    public float currentKeys = 0;
    public float currentMoney = 0;

    public float currentAmmo = 0;
    public float maxAmmo = 120;

    public float currentPopularity = 0;
    public float currentNotoriety = 0;

    public float currentPoints = 0;

    public float currentArmour = 0;
    public float currentFood = 0;
    public float maxFood = 10.0f;

    public GameObject[] heldItems;
    private int activeItemIndex;
    private GameObject currentActiveItem;
    #endregion

    

    //Player Pickups
    #region Player Pickups
    public void ApplyPickuptoPlayerCon(PickupTypes type, float value)
    {
        print("player recieved pickup: " + type.ToString());
        switch (type)
        {
            case PickupTypes.health:
                currentHealth = Mathf.Clamp((currentHealth + value), 0, maxHealth);
                PlayerHudManager.instance.UpdateStatsPanel();
                if (playerDH)
                {
                    playerDH.ApplyHealth(value, true);
                }
                break;
            case PickupTypes.keyPickup:
                currentKeys = (currentKeys + value);
                PlayerHudManager.instance.UpdateStatsPanel();
                break;



            case PickupTypes.xp:
                print("XP FIRED");
                if (currentXp + value >= xpToNextLevel)
                {
                    PlayerLevelup();
                }
                currentXp += value;
                break;
            case PickupTypes.money:
                currentMoney += value;
                break;
            case PickupTypes.ammo:
                currentAmmo += value;
                break;
            case PickupTypes.popularity:
                currentPopularity += value;
                break;
            case PickupTypes.notoriety:
                currentNotoriety += value;
                break;
            case PickupTypes.points:
                currentPoints += value;
                break;
            case PickupTypes.armour:
                currentArmour += value;
                break;
            case PickupTypes.food:
                currentFood += value;

                if (currentFood > maxFood)
                {
                    currentFood = maxFood;
                }
                break;
        }
    }
    #endregion
    //Player Leveling
    #region Player Leveling
    public void PlayerLevelup()
    {
        //Arbitrarily multiplies the required XP for the next level by 1.5 - should be parameterised or improved.
        xpToNextLevel = xpToNextLevel * 1.5f;
        currentLevel++;
    }
    #endregion

    public void RespawnPlayerAtControllerLocation() 
    {
        if (!reloadOnPlayerDeath)
        {
            AvatarInstances[AvatarIndex].GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            AvatarInstances[AvatarIndex].GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
            AvatarInstances[AvatarIndex].transform.position = this.transform.position;
            AvatarInstances[AvatarIndex].transform.rotation = this.transform.rotation;
            //playerDH.CurrentHealth = 0;
            //playerDH.ApplyHealth(playerDH.MaxHealth);
            PlayerHudManager.instance.DoScreenFade(true);
        } 
        else if (reloadOnPlayerDeath) 
        {
            
            //reload the current scene to reset the level
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }


    }

    void tickDownStat(PickupTypes statType)
    {
        switch (statType)
        {
            case PickupTypes.notoriety:
                //We want to operate on our variables in here
                currentNotoriety -= Time.deltaTime;
               // print("Notorious as: " + currentNotoriety);
                break;

            case PickupTypes.popularity:
                //We want to operate on our variables in here
                currentPopularity -= (Time.deltaTime * statDepletionRate) ;
                //print("Popular as: " + currentPopularity);
                break;
        }
    }
}