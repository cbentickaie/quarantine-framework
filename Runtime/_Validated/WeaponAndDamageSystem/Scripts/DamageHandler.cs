using UnityEngine;
using System.Collections;

public class DamageHandler : MonoBehaviour {

    //public delegate void OnDeathDelegate();
    // public static OnDeathDelegate DamageDeathDelegate;

    public float CurrentHealth = 100;
    public float MaxHealth = 100;
    public float HealthRatio = 1.0f;
    public bool AppliesDamage = false;
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
    [SerializeField]
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

    void OnCollisionEnter(Collision other)
    {
        
        if (AppliesDamage && !AppliesRepeatedDamage)
        {
            //isApplyingDamage = true;
            //StartCoroutine("DelayNextDamage");
            if (!isBouncer)
            {
               // Debug.Log("Hit Something");
                if (other.gameObject.GetComponent<DamageHandler>() != null)
                {
                    //print(other.collider.name + "damaged for " + DamageAmount);
                    other.gameObject.GetComponent<DamageHandler>().ApplyDamage(DamageAmount);
                    if (DamageFX) { ParticleSystem sparks = (ParticleSystem)Instantiate(DamageFX, transform.position, transform.rotation) as ParticleSystem; }
                    if (DestroySelf) 
                    {
                        Destroy(gameObject);
                    }                    
                }

                


                if (useAudioforDesctruction)
                {
                    if (DestroySelf)
                    {
                        if (damageAudioSrc && damageAudioSrc.clip)
                        {
                            damageAudioSrc.Play();
                            //destroy this actor once the duration of the audio has elapsed
                            Destroy(gameObject, damageAudioSrc.clip.length);
                        }
                        else 
                        {
                            Destroy(gameObject);
                        }

                    }
                }
                else if (!useAudioforDesctruction)
                {
                    if (DestroySelf) 
                    {
                        if (DamageFX) { ParticleSystem sparks = (ParticleSystem)Instantiate(DamageFX, transform.position, transform.rotation) as ParticleSystem; }
                        Destroy(gameObject);
                    }                    
                }
                

                
            }
            else if (isBouncer)
            {
                numBounces++;
                if (numBounces >= maxBounces)
                {
                    if (other.gameObject.GetComponent<DamageHandler>() != null)
                    {
                        other.gameObject.GetComponent<DamageHandler>().ApplyDamage(DamageAmount);
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
            contactDamageH.ApplyDamage(DamageAmount);
            print("REPEAT DAMAGE APPLIED");
            yield return new WaitForSeconds(frequency);
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
        if (DamageSounds[0])
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
    public void ApplyDamage(float DamageAmount)
    {
        
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


