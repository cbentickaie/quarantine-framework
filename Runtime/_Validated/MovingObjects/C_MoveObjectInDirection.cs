using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_MoveObjectInDirection : MonoBehaviour
{
    public MoveTypes MovementType;
    public MoveDirections MovementDirection;    
    public float MovementDistance = 1.0f;
    public float MovementSpeed = 1.0f;
    public bool useEndLocation = false;
    private Vector3 startLocation;
    public Vector3 endLocation;
    //[SerializeField]
    bool isReversing = false;
    //[SerializeField]
    // Time when the movement started.
    private float startTime;
    //[SerializeField]
    float distCovered = 0;
   // [SerializeField]
    float fractionOfJourney = 0;
    // Total distance between the markers.
    //[SerializeField]
    private float journeyLength;
    //[SerializeField]
    
    [SerializeField]bool isrunning = true;
    bool PlayerAttached = false;

    void Start()
    {
        // Keep a note of the time the movement started.
        startTime = Time.time;
        startLocation = gameObject.transform.position;
        // Calculate the journey length.
        CalculateEndLocation();
        journeyLength = Vector3.Distance(startLocation, endLocation);
    }

    void movePlatform() 
    {
        // Distance moved equals elapsed time times speed..
        distCovered = (Time.time - startTime) * MovementSpeed;
        //distCovered = (transform.position - endLocation).magnitude/journeyLength;
        // Fraction of journey completed equals current distance divided by total distance.
        fractionOfJourney = distCovered / journeyLength;
       // fractionOfJourney = distCovered;
        //fractionOfJourney = Mathf.Clamp(Mathf.Sin(Time.time), 0, 1.0f); //Mathf.Sin(Time.time);

        if (isReversing)
        {
            transform.position = Vector3.Lerp(endLocation, startLocation, fractionOfJourney);
        }
        else
        {
            transform.position = Vector3.Lerp(startLocation, endLocation, fractionOfJourney);
        }

        if (fractionOfJourney > 1.0)
        {
            //Alternate based upon move type behavour here
            isReversing = !isReversing;
            startTime = Time.time;
            distCovered = 0;
            fractionOfJourney = 0;
            HandlemovementType();
            //DisableMovement if only once

        }
        else if (fractionOfJourney < 0)
        {
            //Alternate based upon move type behavour here
            isReversing = !isReversing;
            startTime = Time.time;
            distCovered = 0;
            fractionOfJourney = 0;
            HandlemovementType();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isrunning)
        {
            movePlatform();            
        }
    }

    void HandlemovementType()
    {
        switch (MovementType)
        {
            case MoveTypes.LoopOnce:
                if (isReversing)
                {
                    isrunning = true;
                }
                else 
                {
                    isrunning = false;
                }
                
                break;
            case MoveTypes.MoveOnce:
                isrunning = false;
                break;
            case MoveTypes.PingPong:
                isrunning = true;
                break;
        }
    }
    void CalculateEndLocation()
    {
        if (!useEndLocation)
        {
            switch (MovementDirection)
            {
                case MoveDirections.Forward:
                    endLocation = gameObject.transform.position + gameObject.transform.forward * MovementDistance;
                    break;
                case MoveDirections.Backward:
                    endLocation = gameObject.transform.position + gameObject.transform.forward * -MovementDistance;
                    break;
                case MoveDirections.Up:
                    endLocation = gameObject.transform.position + gameObject.transform.up * MovementDistance;
                    break;
                case MoveDirections.Down:
                    endLocation = gameObject.transform.position + (gameObject.transform.up * -MovementDistance);
                    break;
                case MoveDirections.Right:
                    endLocation = gameObject.transform.position + (gameObject.transform.right * MovementDistance);
                    break;
                case MoveDirections.Left:
                    endLocation = gameObject.transform.position + (gameObject.transform.right * -MovementDistance);
                    break;
            }
        }        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerAttached = true;
            print("Player is Attached");
            other.gameObject.transform.parent = transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerAttached = false;
            print("Player is Attached");
            other.gameObject.transform.parent = null;
        }
    }
}
