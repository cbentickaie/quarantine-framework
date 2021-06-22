using UnityEngine;
using System.Collections;

public class FlickeringLight : MonoBehaviour {

    public float MinDarkTime = 0.1f;
    public float MaxDarkTime = 0.25f;

    public float MinLightTime = 0.1f;
    public float MaxLightTime = 0.5f;

    private bool isSwitchedOn = false;
    private float tgtIntensity = 4.0f;
    public Vector2 minMaxIntensity = new Vector2(1.0f, 4.0f);
    public float LightSpeed = 0.314f;
    // Use this for initialization
    void Start () {
        gameObject.GetComponent<Light>().intensity = 0;
        //Turn on Flickering
        toggleLight();
        
    }
    public void toggleLight()
    {
        if (isSwitchedOn == false)
        {
            StartCoroutine(FlickerLight());
            isSwitchedOn = true;
        }
        else if(isSwitchedOn == true)
        {
            //StopCoroutine(FlickerLight());
            StopAllCoroutines();
            gameObject.GetComponent<Light>().intensity = 0;
            tgtIntensity = 0;
            isSwitchedOn = false;

        }

    }

    private void Update()
    {
        gameObject.GetComponent<Light>().intensity = Mathf.Lerp(gameObject.GetComponent<Light>().intensity, tgtIntensity, Time.deltaTime * LightSpeed);
    }

    IEnumerator FlickerLight()
    {
        while (true)
        {
            //gameObject.GetComponent<Light>().intensity = 0;
            tgtIntensity = 0;
            yield return new WaitForSeconds(Random.Range(MinDarkTime, MaxDarkTime));
            
            //tgtIntensity = 10.0f;
            tgtIntensity = Random.Range(minMaxIntensity.x, minMaxIntensity.y);

            //gameObject.GetComponent<Light>().intensity = 10;
            yield return new WaitForSeconds(Random.Range(MinLightTime, MaxLightTime));

        }
    }

}
