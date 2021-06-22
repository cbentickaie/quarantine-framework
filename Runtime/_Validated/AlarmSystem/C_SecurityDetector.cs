using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class C_SecurityDetector : MonoBehaviour
{
    Collider detectionVolume;

    [SerializeField] bool isAlertActive = false;
    [SerializeField] bool isAlarmRaised = false;
    float alertnessLevel = 0;
    [SerializeField]float alertnessThreshold = 3.14f;

    [SerializeField] float alertSpeed = 1.0f;
    [SerializeField] float alertDecaySpeed = 0.5f;
    [SerializeField] float detectionAngle = 0.75f;
    Transform detectedActor;
    Vector3 AngleToTarget;
    float ViewAngleDelta;

    public UnityEvent AlarmRaised;
    public UnityEvent AlarmReset;

    public UnityEvent Alerted;
    public UnityEvent NotAlerted;

    // Start is called before the first frame update
    void Start()
    {
        detectionVolume = GetComponent<SphereCollider>();
        detectionVolume.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlertActive)
        {
            EvaluatePlayerAngleVisibility();
            //print("Alert Level: " + alertnessLevel);
            if (alertnessLevel > alertnessThreshold && !isAlarmRaised)
            {
                print("Alarm RAISED!@");
                AlarmRaised.Invoke();
                isAlarmRaised = true;
            }
            else if (alertnessLevel < alertnessThreshold && isAlarmRaised)
            {
                print("Alarm RESET");
                AlarmReset.Invoke();
                isAlarmRaised = false;
            }
        }
        else if (alertnessLevel >= 0) 
        {
            alertnessLevel -= Time.deltaTime * alertDecaySpeed;
            alertnessLevel = Mathf.Clamp(alertnessLevel, 0, alertnessThreshold * 1.6f);
            //print("Decaying Alert: " + alertnessLevel);
        }
        Debug.DrawLine(transform.position, transform.position + (transform.forward * 10.0f));
        drawDebugElements();
    }
    bool EvaluatePlayerAngleVisibility() 
    {
        AngleToTarget = (detectedActor.position - transform.position).normalized;
        ViewAngleDelta = Vector3.Dot(transform.forward, AngleToTarget);
        if (ViewAngleDelta > detectionAngle)
        {
            alertnessLevel += Time.deltaTime * alertSpeed;
            alertnessLevel = Mathf.Clamp(alertnessLevel, 0, (alertnessThreshold * 1.6f));
        }
        else if (ViewAngleDelta < detectionAngle) 
        {
            alertnessLevel -= Time.deltaTime * alertDecaySpeed;
            alertnessLevel = Mathf.Clamp(alertnessLevel, 0, (alertnessThreshold * 1.6f));
        }
        print("View Delta: " + ViewAngleDelta);
        return false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            Alerted.Invoke();
            isAlertActive = true;
            Debug.Log("Player Within Detection Volume");
            detectedActor = other.gameObject.transform;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            NotAlerted.Invoke();
            isAlertActive = false;
            Debug.Log("Player Left Detection Volume");
            detectedActor = null;
        }
    }
    private void OnDrawGizmos()
    {
        drawDebugElements();
    }
    void drawDebugElements() 
    {
        //float detectAngle = Mathf.Rad2Deg(detectionAngle);
        //Gizmos.DrawFrustum(transform.position, , 10f, 1f, 1.0f);
        // convert 1 radian to degrees

        float rad = detectionAngle;


        float deg = rad * Mathf.Rad2Deg;
        Debug.Log(rad + " radians are equal to " + deg + " degrees.");
       
        //Gizmos.DrawFrustum(transform.position, deg, 10f, 1f, 1.0f);
    }
}
