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
    private Vector3 startLocation;

    void Start()
    {
        // Keep a note of the time the movement started.
        startTime = Time.time;
        startLocation = gameObject.transform.position;
        // Calculate the journey length.
        CalculateEndLocation();
        journeyLength = Vector3.Distance(startLocation, endLocation);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            
            // Distance moved equals elapsed time times speed..
            distCovered = (Time.time - startTime) * MovementSpeed;

            // Fraction of journey completed equals current distance divided by total distance.
            fractionOfJourney = distCovered / journeyLength;
            //fractionOfJourney = Mathf.Clamp(Mathf.Sin(Time.time), 0, 1.0f); //Mathf.Sin(Time.time);
            if (isReversing)
            {
                transform.position = Vector3.Lerp(endLocation, startLocation, fractionOfJourney);
            }
            else 
            {
                transform.position = Vector3.Lerp(startLocation, endLocation, fractionOfJourney);
            }           
            if (fractionOfJourney >= 1.0)
            {
                isReversing = !isReversing;
                startTime = Time.time;
                distCovered = 0;
                fractionOfJourney = 0;

                //DisableMovement if only once

            }
            else if (fractionOfJourney <= 0)
            {
                isReversing = !isReversing;
                startTime = Time.time;
                distCovered = 0;
                fractionOfJourney = 0;
            }
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
}
