using UnityEngine;
using System.Collections;

public class TimedParticleDestructor : MonoBehaviour {

    public float Lifetime = 3.0f;
	// Use this for initialization
	void Start () {
        StartCoroutine(TimedDeactivation(Lifetime));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator TimedDeactivation(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        print("Timer Up");
        Destroy(gameObject);
    }
}
