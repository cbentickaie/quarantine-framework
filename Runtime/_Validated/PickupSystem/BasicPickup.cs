using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPickup : MonoBehaviour {
    [ExecuteInEditMode]
    public PickupTypes itemType;
    public float itemValue = 0.0f;
    public bool destroyOnPickup = true;
    public PlayerController Pcon;
    public bool respawnItemAfterPickup = false;
    public float reSpawnDelay = 5.0f;
    [SerializeField]
    public AudioClip pickUpSound;
    AudioSource PickUpAudioSource;
    [SerializeField]
    bool useAudio = false;
    private void Awake()
    {
        if (respawnItemAfterPickup) 
        {
            destroyOnPickup = false;
            print("disabling destruction on piuckup");
        }
        if ((PickUpAudioSource = GetComponent<AudioSource>()) && pickUpSound)
        {
            PickUpAudioSource.clip = pickUpSound;
            useAudio = true;
        }
    }
    private void Start()
    {
        if (PlayerController.instance)
        {
            Pcon = PlayerController.instance;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ApplyPickup(itemType, itemValue);
        }
    }

    public virtual void ApplyPickup(PickupTypes thisItemType, float itemValue)
    {
        print("Item Picked up: " + itemValue + thisItemType.ToString());

        if (Pcon)
        {
            Pcon.ApplyPickuptoPlayerCon(thisItemType, itemValue);
        }

        if (destroyOnPickup)
        {
            if (useAudio)
            {
                PickUpAudioSource.Play();
                Destroy(gameObject, pickUpSound.length);
            }
            else 
            {
                Destroy(gameObject);
            }
            
        }
        if (respawnItemAfterPickup) 
        {
            if (useAudio)
            {
                PickUpAudioSource.Play();               
            }
            StartCoroutine("delayRespawnItem");
        }

    }

    IEnumerator delayRespawnItem() 
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(reSpawnDelay);

        gameObject.GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<Collider>().enabled = true;
    }

    
}
