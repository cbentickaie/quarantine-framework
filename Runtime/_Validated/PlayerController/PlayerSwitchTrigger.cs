using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitchTrigger : MonoBehaviour {

    public GameObject PlayerPrefab;
    public GameObject PawnInstance;

    public bool useOnce = true;

	// Use this for initialization
	void Start ()
    {
        PawnInstance = Instantiate(PlayerPrefab, this.transform.position, this.transform.rotation);
        PawnInstance.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController.instance.SwitchAvatarToGaemObject(PawnInstance);
        }
        if (useOnce)
        {
            Destroy(gameObject);
        }
    }
}
