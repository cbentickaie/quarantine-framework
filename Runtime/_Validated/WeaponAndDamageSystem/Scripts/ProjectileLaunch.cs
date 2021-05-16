using UnityEngine;
using System.Collections;

public class ProjectileLaunch : MonoBehaviour {

    public float Power = 900;

    // Use this for initialization
    void Start () {
        gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * Power);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
