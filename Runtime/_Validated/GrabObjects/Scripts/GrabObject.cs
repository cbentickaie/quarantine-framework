using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour {

    public float traceDistance = 3.0f;
    bool impartForceonRelease = true;
    GameObject GrabbedObject = null;
    bool isPhysicsObject = false;
    FixedJoint PhysicsHandle;
    Rigidbody GrabbedRB;
    float currentGrabDistance = 1.0f;

    //Throwing Variables
    float throwPower = 0.0f;
    //The rate multipler for increasing our throwPower
    public float powerRate = 25.0f;
    bool isThrowing = false;
    public bool showPowerMaterial = true;
    public float maxThrowForce = 50.0f;

    // Use this for initialization
    void Start ()
    {
        //Determine if this GameObject already has a Fixed Joint
        if (!GetComponent<FixedJoint>())
        {
            PhysicsHandle = gameObject.AddComponent<FixedJoint>();
        }
        else
        //Use the existing Fixed Joint if so.
        {
            PhysicsHandle = gameObject.GetComponent<FixedJoint>();
        }
	}	

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            TraceForGrabbableObject();
        }
        //Work-In-Progress Grab Distance Adjustment
        if (GrabbedRB)
        {
            currentGrabDistance = Mathf.Clamp(currentGrabDistance + (Input.GetAxis("Mouse ScrollWheel")), 1, 5);
            //print(currentGrabDistance);
            //PhysicsHandle.transform.localPosition = PhysicsHandle.transform.localPosition + new Vector3(0, Input.GetAxis("Mouse ScrollWheel"), 0);

            //print( PhysicsHandle.connectedAnchor);
            //print(Input.GetAxis("Mouse ScrollWheel"));

            //print(PhysicsHandle.connectedAnchor - transform.position * currentGrabDistance);            
            //PhysicsHandle.connectedAnchor = (transform.position + (transform.forward * currentGrabDistance));

            //Debug.Log(GrabbedRB.inertiaTensor.magnitude);


        }

        //Alternate Fire Throw Mode functionality
        //the Input.getbutton method (as distinct from GetButtonDown) will continue to fire for every frame the button is held down.
        if (Input.GetButton("Fire2") && GrabbedRB)
        {
            isThrowing = true;

            throwPower = Mathf.Clamp((throwPower + (Time.deltaTime * powerRate)), 0, maxThrowForce);
                      
            //print(throwPower);

            //Optional Material Coloring to indicate Power
            if (showPowerMaterial)
            {                
                gameObject.GetComponent<MeshRenderer>().material.color = Color.Lerp(Color.green, Color.red, Mathf.InverseLerp(0, maxThrowForce, throwPower));
            }

        }
        
        if (Input.GetButtonUp("Fire2") && isThrowing)
        {
            //throw object on release
            DropObject(true);
            throwPower = 0.0f;
            isThrowing = false;
        }

	}

    void TraceForGrabbableObject()
    {
        //If there is no currently Grabbed Object, then try to Trace and Grab
        if (GrabbedObject == null)
        {        
            RaycastHit GrabHit;

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out GrabHit, traceDistance))
            {
                print(GrabHit.collider.gameObject.name);
                Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward,Color.red, traceDistance);
                //Check against a specific Tag on the Hit Gameobject, to ensure we don't accidentally Grab parts of the world, like the floor (totally happened to me ;))
                //If the object does NOT have the appropriate tage, we use 'return' to immediately break out of this function.
                if (GrabHit.collider.gameObject.tag != "Grabbable")
                    return;

                //Determine if this object has a Rigidbody component, and therefore can participate in Physics
                if (GrabHit.collider.gameObject.GetComponent<Rigidbody>())
                {
                    isPhysicsObject = true;
                    currentGrabDistance = GrabHit.distance;
                }
                else
                {
                    isPhysicsObject = false;                               
                }
            
                TryGrabObject(GrabHit.collider.gameObject);
            }
            else
            {
                //if we hit nothing, reset variables to default to prevent confusion (grabbing an object looked at previously for example.)
                isPhysicsObject = false;                
            }
        }
        else
        //If already holding an object, this will release it.
        {
            print("Droping Object");
            DropObject(false);
        }
    }

    float InitialRbDamping = 1.0f;
    float InitialRbAngularDamping = 0.0f;

    void TryGrabObject(GameObject GrabCandidate)
    {
        GrabbedObject = GrabCandidate;
        if (isPhysicsObject)
        {

            //GrabbedObject.GetComponent<Rigidbody>().isKinematic = true;
            GrabbedRB = GrabbedObject.GetComponent<Rigidbody>();
            
            PhysicsHandle.connectedBody = GrabbedRB;
            //store Grabbed Rb initial Damping values
            InitialRbAngularDamping = GrabbedRB.angularDrag;
            InitialRbDamping = GrabbedRB.drag;
            //check distance and mass on grabbed RB and adjust Damping appropriately

        }
        else
        {
            homePosition = GrabbedObject.transform.position;
            HomeRotation = GrabbedObject.transform.rotation;

            GrabbedObject.transform.parent = this.transform;
            GrabbedObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            GrabbedObject.transform.rotation = this.transform.rotation;
        }

    }
    public bool dropReturnToHome = false;
    Vector3 homePosition;
    Quaternion HomeRotation;
    void DropObject(bool throwObjectOnRelease)
    {
        if (isPhysicsObject)
        {
            Vector3 throwForce;
            throwForce = GrabbedRB.velocity;
            
            PhysicsHandle.connectedBody = null;
            GrabbedRB.velocity = throwForce;

            if (throwObjectOnRelease)
            {
                GrabbedRB.AddForce(this.transform.forward * throwPower, ForceMode.Impulse);
            }

            GrabbedObject = null;
            GrabbedRB = null;
        }
        if (!isPhysicsObject)
        {
            GrabbedObject.transform.parent = null;
            if (dropReturnToHome) 
            {
                GrabbedObject.transform.SetPositionAndRotation(homePosition, HomeRotation);
            }
            GrabbedObject.transform.SetPositionAndRotation(homePosition, HomeRotation);
        }        
        GrabbedObject = null;
    }
}
