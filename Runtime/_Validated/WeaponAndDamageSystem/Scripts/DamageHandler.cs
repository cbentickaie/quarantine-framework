using UnityEngine;
using System.Collections;

//Damage types compiled from 
//https://forums.giantitp.com/showthread.php?379165-MM-Resistances-Immunities-Vulnerabilities-and-Damage
//Stanard DnD Damage types, not inclusive.
public enum DamageTypes { _Default, Piercing, Bludgeoning, Slashing, Radiant, Explosive, Fire, Cold, Acid, Psychic, Laser, Electricity}

public class DamageHandler : MonoBehaviour {

    //public delegate void OnDeathDelegate();
    // public static OnDeathDelegate DamageDeathDelegate;

    public float CurrentHealth = 100;
    public float MaxHealth = 100;
    public float HealthRatio = 1.0f;
    public bool AppliesDamage = false;
    public float hitDamageThreshold = 8.0f;
    public DamageTypes DamageTypeToApply = DamageTypes._Default;
    public bool DestroySelf = false;
    public float DamageAmount = 10;
    public ParticleSystem DamageFX;

    [SerializeField] bool isBouncer = false;
    int numBounces = 0;
    int maxBounces = 2;

    [SerializeField] AudioSource damageAudioSrc;
    [SerializeField] AudioClip DeathSound;
    [SerializeField] AudioClip[] DamageSounds;
    [SerializeField]bool useAudioforDesctruction = false;
    [SerializeField]
    PlayerController myPcon;

    public bool AppliesRepeatedDamage = false;
    public float frequency = 0.5f;
    private bool isContacting = false;
    [SerializeField]
    GameObject contactObject;
    DamageHandler contactDamageH;
    [SerializeField]
    bool isApplyingDamage = false;
    private void Awake()
    {

        if (GetComponent<AudioSource>() && useAudioforDesctruction)
        {
            damageAudioSrc = GetComponent<AudioSource>();
        }
        else if (!GetComponent<AudioSource>())
        {
            useAudioforDesctruction = false;
        }
        Rigidbody selfRb;
        if (selfRb = gameObject.GetComponent<Rigidbody>())
        {
            //print("found rigidbody");
            selfRb.sleepThreshold = 0f;
        }
    }

    private void Start()
    {
        /*if (myPcon = gameObject.GetComponent<PlayerController>())
        {
            print("Playercontroller Located in this GameObject");
        }
        else if (myPcon = gameObject.GetComponentInParent<PlayerController>())
        {
            //print("Playercontroller Located in Parent");
        }
        else if (myPcon == null)
        {
            print("No PlayerController Found, Health values will not be passed outside this script");
        }*/

        StartCoroutine("initPlayerDamage");

    }

    IEnumerator initPlayerDamage() 
    {
        yield return new WaitForSeconds(0.1f);


        if (this.gameObject.tag == "Player") 
        {
            myPcon = PlayerController.instance;
            print("Playercontroller found - damageHandler is applied to Player");
        }
    }

    public IEnumerator DelayNextDamage() 
    {
        //print("Llocking damage");
        yield return new WaitForSeconds(frequency);
        isApplyingDamage = false;
        //print("Unllocking damage");
    }

    #region Collision Based Damage Handling
    void OnCollisionEnter(Collision other)
    {
        //Apply regular damage if NOT repeating
        if (AppliesDamage && !AppliesRepeatedDamage && other.relativeVelocity.magnitude > hitDamageThreshold)
        {
            print("IMPACT: "+ other.relativeVelocity.magnitude);
            //isApplyingDamage = true;
            //StartCoroutine("DelayNextDamage");
            DamageHandler dh;
            
            if (!isBouncer)
            {
               // Debug.Log("Hit Something");
                if (other.gameObject.GetComponent<DamageHandler>() != null)
                {
                    dh = other.gameObject.GetComponent<DamageHandler>();
                    ApplyDamageToTarget(dh, DamageTypeToApply);
                }  
            }
            else if (isBouncer)
            {
                numBounces++;
                if (numBounces >= maxBounces)
                {
                    if (other.gameObject.GetComponent<DamageHandler>() != null)
                    {
                        other.gameObject.GetComponent<DamageHandler>().ReceiveDamage(DamageAmount, DamageTypeToApply);
                    }

                    if(DamageFX){ ParticleSystem sparks = (ParticleSystem)Instantiate(DamageFX, transform.position, transform.rotation) as ParticleSystem;}
                    if (useAudioforDesctruction)
                    {
                        damageAudioSrc.Play();
                        if (DestroySelf)
                        {
                            Destroy(gameObject, damageAudioSrc.clip.length);
                        }
                        
                    }
                    else if (!useAudioforDesctruction)
                    {
                        if (DestroySelf)
                        {
                            Destroy(gameObject);
                        }
                    }
                }
            }

            //InvokeRepeating("ApplyDamage(DamageAmount)", 0.5f, 0.5f);
        }


        //Start Applying Continuous damage over time
        if (AppliesDamage && AppliesRepeatedDamage) 
        {
            if (other.gameObject.GetComponent<DamageHandler>() != null) 
            {
                contactDamageH = other.gameObject.GetComponent<DamageHandler>();
                contactObject = other.gameObject;
                isContacting = true;
                StartCoroutine("DoRepeatDamage");
            }
;
        }

    }
    IEnumerator DoRepeatDamage() 
    {
        while (isContacting && contactDamageH) 
        {
            contactDamageH.ReceiveDamage(DamageAmount, DamageTypes._Default);
            print("REPEAT DAMAGE APPLIED");
            yield return new WaitForSeconds(frequency);
        }
    
    }


    void ApplyDamageToTarget(DamageHandler dh, DamageTypes dType) 
    {
        dh.ReceiveDamage(DamageAmount, dType);
        
        if (DamageFX) { ParticleSystem sparks = (ParticleSystem)Instantiate(DamageFX, transform.position, transform.rotation) as ParticleSystem; }
        if (DestroySelf)
        {
            Destroy(gameObject);
        }

    }
    
    private void OnCollisionStay(Collision collision)
    {
        //InvokeRepeating("ApplyDamage(DamageAmount)", 0.5f, 0.5f);
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == contactObject) 
        {
            //CancelInvoke();
            if (AppliesDamage && AppliesRepeatedDamage)
            {
                contactDamageH = null;
                contactObject = null;
                isContacting = false;
                StopCoroutine("DoRepeatDamage");
            }
        }


    }
    #endregion

    void updateHealthRatio() 
    {
        HealthRatio = CurrentHealth / MaxHealth;
    }
    public void ApplyHealth(float newhealth, bool setNewHealth = false)
    {
        if (!setNewHealth) 
        {
            //CurrentHealth += newhealth;
            CurrentHealth += Mathf.Clamp(CurrentHealth + newhealth, 0, MaxHealth);
            updateHealthRatio();
        }else if (setNewHealth) 
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + newhealth, 0, MaxHealth);
            updateHealthRatio();
        }
        
    }
    public void playDamageSound() 
    {
        print(DamageSounds.Length);
        if (DamageSounds.Length > 0)
        {
            damageAudioSrc.clip = DamageSounds[Random.Range(0, DamageSounds.Length)];
            damageAudioSrc.Play();
        }
        else 
        {
            Debug.Log("Missing Audio Clips for Damage Sounds - please add them in the Inspector");
        }

    }

    public void playDeathSound()
    {
        damageAudioSrc.clip = DeathSound;
        damageAudioSrc.Play();
    }
    public void ReceiveDamage(float DamageAmount, DamageTypes dType)
    {
        #region Boilerplate Damage Handling
        if (CurrentHealth - DamageAmount > 0)
        {
            CurrentHealth = CurrentHealth - DamageAmount;
            updateHealthRatio();
            print(DamageAmount + " Damage Applied to: " + gameObject.name);
            playDamageSound();
        }
        else
        {
            CurrentHealth = 0;
            updateHealthRatio();
            DeathEvent();
        }
        if (DamageFX) { ParticleSystem DamageParticles = (ParticleSystem)Instantiate(DamageFX, gameObject.GetComponent<Collider>().bounds.center, transform.rotation) as ParticleSystem; }

        if (myPcon)
        {
            myPcon.currentHealth = CurrentHealth;
            
        }
        //if a playerController is available on this actor, we want to reach into it and adjust its health value to match the new value specified here.
        if (gameObject.tag == "Player") 
        {
            PlayerHudManager.instance.ShowDamageFlash(HealthRatio);
        }
        if (useAudioforDesctruction) 
        {
           // playDeathSound();
        }
        #endregion
        #region Damage Type Handling
        switch (dType)
        {
            //print(other.collider.name + "damaged for " + DamageAmount + DamageTypeToApply.ToString);
            case DamageTypes._Default:
                //other.gameObject.GetComponent<DamageHandler>().ReceiveDamage(DamageAmount);
                //if (DamageFX) { ParticleSystem sparks = (ParticleSystem)Instantiate(DamageFX, transform.position, transform.rotation) as ParticleSystem; }
                //if (DestroySelf)
                //{
                //    Destroy(gameObject);
                //}
                print("Did Default Damage");
                break;
            case DamageTypes.Bludgeoning:
                
                print("Did Bludgeon Damage");
                if (!gameObject.GetComponent<C_StunnedStatus>())
                {
                    gameObject.AddComponent<C_StunnedStatus>();
                }
                break;
            case DamageTypes.Laser:

                print("Did Laser Damage");
                break;
            case DamageTypes.Electricity:
                if (!gameObject.GetComponent<C_ShockedStatus>()) 
                {
                    gameObject.AddComponent<C_ShockedStatus>();
                }                
                break;
        }
        #endregion
    }


    void DeathEvent()
    {
       // DamageDeathDelegate();
        Debug.Log("This Thang He Deeed");
        if (gameObject.tag == "Player")
        {
            //Relocate the Player to the Controllers Original Location
            PlayerController.instance.RespawnPlayerAtControllerLocation();
            ApplyHealth(MaxHealth);
            if (damageAudioSrc) 
            {
                playDeathSound();
            }
        }
        else if(DestroySelf)
        {            
            Destroy(gameObject);            
        }
        //SendMessage("DamageDeath");
    }
}


