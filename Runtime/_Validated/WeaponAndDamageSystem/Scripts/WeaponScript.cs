using UnityEngine;
using System.Collections;

public enum ammoTypes { Bullets, Rockets, Grenades, Plasma, EldritchEnergy }
[RequireComponent(typeof(AudioSource))]



public class WeaponScript : MonoBehaviour {
    //Weapon Script example by Craig Bentick, for the Academy of Interactive Entertainment.
    public ammoTypes ammoType;

    public bool useAmmoCounting = false;

    public WeaponTypes weaponType;
    public Transform MuzzlePoint;

    [Header("Prefab Assignments")]
    public Rigidbody BulletPrefab;
    public Rigidbody ProjectilePrefab;
    public float FiringRate = 0.1f;    
    [Space(10)]

    [Header("Trace Weapon Settings")]
    public float TraceDamageAmount = 6.0f;
    public float TraceRange = 10.0f;
    public GameObject TraceImpactFx;
    [Space(10)]

    [Header("Audio Settings")]
    public AudioClip FireSoundClip;
    AudioSource WeaponAudio;
    [Space(10)]

    [Header("WIP: Aim Weapon at Fps Camera Forward")]
    public bool useFpsConvergence = false;

    [SerializeField] PlayerItemInventory pInv;

    //Handle Ammo-Based variables
    bool isFiring = false;

    public bool isAiWeapon = false;


    //This Enumeration serves as a list of 'types' that we can use to switch between different types of outcome or functionality within the same Function or Method 
    //See the ActivateWeapon function for an example of its implementation. 
    //YOU can add new types to this list to serve your own needs, ensuring you handle the Cases for the new entry.
    public enum WeaponTypes 
    {
        SingleShot,
        AutoFire,
        Projectile,
        SingleTrace,
        AutoTrace,
    };

    //Set parameters before the game starts
    void Awake()
    {
            //Automatically get the audiosource component on this object and Populate the WeaponAudio variable.
            WeaponAudio = GetComponent<AudioSource>();
            WeaponAudio.playOnAwake = false;
            WeaponAudio.clip = FireSoundClip;
    }

    // Use this for initialization
    void Start ()
    {
        
        
        if (pInv = gameObject.GetComponentInParent<PlayerItemInventory>())
        {
            print("Yay Weapon Found Inventory");
            isAiWeapon = false;
        }
         
        if (useFpsConvergence)
        {
            //TODO: aim this object at the Player Camera Forward vector projected a given convergence distance.
            //
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Handle input when the weapon is fired, and call our ActivateWeapon method.
        if (Input.GetButtonDown("Fire1"))
        {
            if (!isFiring)
            {
                if (useAmmoCounting && pInv.checkAmmo(ammoType))
                {
                    ActivateWeapon();
                    isFiring = true;
                    StartCoroutine("FiringDelay");
                }
                else if (!useAmmoCounting)
                {
                    ActivateWeapon();
                    isFiring = true;
                    StartCoroutine("FiringDelay");
                }

            }
            
        }

        //Handle when the player releases the Fire input, in this case stop the auto firing coroutines
        if (Input.GetButtonUp("Fire1"))
        {
            DeactivateWeapon();
        }
    }

    //This method handles the actual firing of the Weapon, or Activation of the item
    //Note we have made this method Public, so that we can ask this weapon to fire from elsewhere,
    //you may choose to implement you inputs in another script and can simply reference this weapon and call the activate and deactivate functions from there.
    public void ActivateWeapon()
    {
        //Handle WeaponTypes 'cases' here, call respective function with relevant arguments for each type
        switch (weaponType)
        {
            case WeaponTypes.SingleShot:
                SpawnBullet();
                break;
            case WeaponTypes.AutoFire:
                StartCoroutine("AutoFire");
                break;
            case WeaponTypes.Projectile:
                SpawnProjectile();
                break;
            case WeaponTypes.SingleTrace:
                TraceFire();
                break;
            case WeaponTypes.AutoTrace:
                StartCoroutine("TraceAutoFire");
                break;

        }
        Debug.Log("Weapon Fired");
    }

    //Handle the Deactivation of the weapon or item.
    public void DeactivateWeapon()
    {
        switch (weaponType)
        {
            case WeaponTypes.SingleShot:
                break;

            case WeaponTypes.AutoFire:
                StopCoroutine("AutoFire");
                break;

            case WeaponTypes.AutoTrace:
                StopCoroutine("TraceAutoFire");
                break;

        }
    }

    void handleAmmo()
    {
        if (useAmmoCounting)
        {
            pInv.RemoveAmmo(ammoType);
        }
    }

    void SpawnBullet()
    {
        //this is an incredibly basic Rigidbody projectile method, it is dependant on the bullet prefab handling it's own launch velocity, 
        //see the ProjectileLaunch Script for an example of this implementation, you could also apply a rigidbody impulse here if you wanted to.
        Rigidbody NewBullet = (Rigidbody)Instantiate(BulletPrefab, MuzzlePoint.transform.position, MuzzlePoint.transform.rotation) as Rigidbody;
        PlayGunshotSound();
        handleAmmo();
    }

    //Duplicate method that can be customised for special Projectile based behaviours when firing (apply forces, extra VFX and SFX)
    void SpawnProjectile()
    {
        //this is an incredibly basic Rigidbody projectile method, it is dependant on the bullet prefab handling it's own launch velocity, 
        //see the ProjectileLaunch Script for an example of this implementation, you could also apply a rigidbody impulse here if you wanted to.
        Rigidbody NewProjectile = (Rigidbody)Instantiate(ProjectilePrefab, MuzzlePoint.transform.position, MuzzlePoint.transform.rotation) as Rigidbody;
        PlayGunshotSound();
        handleAmmo();
    }


    //This method uses a Ray Trace, or line from the weapon muzzle out, rather than a physical projectile to determine whether we hit something. 
    //This is advantageous for many situations, including intant-hit weapons, or other scenarios including line-of-sight checks for AI etc..
    void TraceFire()
    {
        //Perform a trace here from the weapon forward, or from the center of the player's viewpoint, or some other location and direction Vector
        //In this case we use the MuzzlePoint position, and project it along the Forward Vector (direction) of the Muzzle Point
        Ray WeaponTrace = new Ray(MuzzlePoint.transform.position, MuzzlePoint.transform.forward);

        //Create a temporary array to store our Hit Information in (we could hit multiple items and might want to iterate over them)
        RaycastHit[] hits;

        //Perform Trace and store the resulting 'Hits' in an array: []
        //We also provide a Length to the raycast, in this case 50, although you could provide that as a 'Range' variable.
        
        hits = Physics.RaycastAll(WeaponTrace, 50.0f);
        

        Debug.DrawRay(MuzzlePoint.transform.position, MuzzlePoint.transform.forward * 50.0f, Color.red);

        //Check whether there was a valid 'First Hit' by checking that length of the array is greater (>) than zero, meaning empty.
        if (hits.Length > 0)
        {
            //Spawn the impact effect at the hit point
            Instantiate(TraceImpactFx, hits[0].point, Quaternion.identity);       

            //Check whether the impacted colliders' gameobject has a valid DamageHandler Component
            if (hits[0].collider.gameObject.GetComponent<DamageHandler>())
            {
                hits[0].collider.gameObject.GetComponent<DamageHandler>().ApplyDamage(TraceDamageAmount);
            }
        }



        //Play audio of weapon firing
        WeaponAudio.pitch = Random.Range(0.75f, 1.1f);
        WeaponAudio.volume = Random.Range(0.75f, 1.0f);
        WeaponAudio.Play();

        //Print to log to indicate we have called this function successfully.
        print("Trace Fired");
        handleAmmo();
    }

    //Create an enumeration for our different Ammo Types
    public enum AmmoTypes
    {
        Bullet,
        Energy,
        Grenade,
        Rockets,
        Water,
        Fire,
        Mana
    }


    IEnumerator TraceAutoFire()
    {
        while (true)
        {
            TraceFire();
            yield return new WaitForSeconds(FiringRate);
            print("Auto Fired");
        }
    }

    IEnumerator AutoFire()
    {
        while (true)
        {
            SpawnBullet();
            yield return new WaitForSeconds(FiringRate);
            print("Auto Bullet Fired");
        }
    }

    //Create a generic delay timer for our firing mechanism
    IEnumerator FiringDelay()
    {
        yield return new WaitForSeconds(FiringRate);
        isFiring = false;
        print("Firing Enabled");
    }

    void PlayGunshotSound()
    {
        //Play audio of weapon firing
        WeaponAudio.pitch = Random.Range(0.75f, 1.1f);
        WeaponAudio.volume = Random.Range(0.75f, 1.0f);
        WeaponAudio.Play();
    }
}
