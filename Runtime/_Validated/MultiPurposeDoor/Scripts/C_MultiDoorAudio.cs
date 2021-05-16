using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(AudioSource))]
public class C_MultiDoorAudio : MonoBehaviour
{
    
    [SerializeField]
    MultiDoor TargetDoor;
    [SerializeField]
    AudioSource DoorAudioSource;

    public AudioClip DoorOpenClip;
    public AudioClip DoorClosingClip;
    public AudioClip DoorLockedClip;
    public AudioClip DoorUnlockedClip;
    private void Awake()
    {
        if (TargetDoor = GetComponent<MultiDoor>())
        {
            print("Multi door found for audio ");
        }
        else 
        {
            this.enabled = false;
        }
        if (DoorAudioSource = GetComponent<AudioSource>())
        {
            print("Source found for Dooraudio ");
            DoorAudioSource.spatialize = true;
            DoorAudioSource.spatialBlend = 1.0f;
            //Bind events to MultiDoor
            TargetDoor.DoorOpeningEvent.AddListener(PlayOpenSound);
            TargetDoor.DoorClosingEvent.AddListener(PlayCloseSound);
            TargetDoor.DoorLockedEvent.AddListener(PlayLockedSound);
            TargetDoor.DoorUnlockedEvent.AddListener(PlayUnlockedSound);

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayOpenSound() 
    {
        DoorAudioSource.PlayOneShot(DoorOpenClip);
    }
    void PlayCloseSound()
    {
        DoorAudioSource.PlayOneShot(DoorClosingClip);
    }
    void PlayLockedSound()
    {
        DoorAudioSource.PlayOneShot(DoorLockedClip);
    }
    void PlayUnlockedSound()
    {
        DoorAudioSource.PlayOneShot(DoorUnlockedClip);
    }
}
