using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour {

    public bool isInteractionEnabled = true;
    public float interactionDistance = 3.0f;
    RaycastHit traceHit;
    Ray interactionRay;
    public bool useHover = true;
    bool isHovering = false;
    [SerializeField]
    Interactable hoverTarget;
    GameObject hoverGO;
    public List<Interactable> interactableComponents;
    public List<string> Hovertags;
    public bool isPlayerInteractor = false;

    private void Awake()
    {
        Hovertags.Add("Grabbable");
        Hovertags.Add("Interactable");
        
         
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out traceHit, interactionDistance);
        //Physics.SphereCast(Camera.main.transform.position, 0.5f, Camera.main.transform.forward, out traceHit, interactionDistance);
        if (useHover && traceHit.collider) 
        {

            //Alternate trace that searches parent objects for an interactable??
            if (traceHit.collider.gameObject.GetComponentInParent<Interactable>() && !isHovering)
            {
                hoverTarget = traceHit.collider.gameObject.GetComponentInParent<Interactable>();
                hoverTarget.StartHover();
                isHovering = true;
                PlayerHudManager.instance.toggleHover(true);
            }

            //Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out traceHit, interactionDistance);
           /* if (traceHit.collider.gameObject.GetComponent<Interactable>() && !isHovering)
            {
                hoverTarget = traceHit.collider.gameObject.GetComponent<Interactable>();
                hoverTarget.StartHover();
                isHovering = true;
                PlayerHudManager.instance.toggleHover(true);
            }*/
            else if (traceHit.collider.gameObject && !isHovering) 
             {
                 foreach (string _tag in Hovertags) 
                 {
                     if (traceHit.collider.gameObject.tag == _tag) 
                     {
                        hoverGO = traceHit.collider.gameObject;
                        isHovering = true;
                        PlayerHudManager.instance.toggleHover(true);
                     }
                 }

             }



            else if (isHovering && hoverTarget)
            {
                if (traceHit.collider.gameObject != hoverTarget.gameObject)
                {
                    hoverTarget.StopHover();
                    hoverTarget = null;
                    PlayerHudManager.instance.toggleHover(false);
                }
            }
            else if(isHovering && traceHit.collider.gameObject != hoverGO)
            {
                hoverGO = null;
                isHovering = false;
                PlayerHudManager.instance.toggleHover(false);
            }
            else
            {                
               // isHovering = false;
               // PlayerHudManager.instance.toggleHover(false);
            }

        }
        if (useHover && !traceHit.collider) 
        {                        
            isHovering = false;
            PlayerHudManager.instance.toggleHover(false);            
        }




        if (Input.GetButtonDown("Fire1"))
        {
            //if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out traceHit, interactionDistance))
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out traceHit, interactionDistance))
            {
                if (traceHit.collider.gameObject.GetComponent<Interactable>())
                {
                    foreach (Interactable intComp in traceHit.collider.gameObject.GetComponents<Interactable>())
                    {
                        intComp.StartInteraction(true);
                        interactableComponents.Add(intComp);
                    }
                }
                else if (traceHit.collider.gameObject.tag == "Grabbable") 
                {
                //Debug.Log("Grabbable Clicked");
                }
                //print(traceHit.collider.name);
            }
            //print("No Hit Detected");
        }
        if (Input.GetButton("Fire1"))
        {
            if (interactableComponents.Count >-1)
            {
                foreach (Interactable intComp in interactableComponents)
                {
                    intComp.TickInteraction();
                }
            }

        }
        if (Input.GetButtonUp("Fire1"))
        {
            if (interactableComponents.Count > -1)
            {
                foreach (Interactable intComp in interactableComponents)
                {
                    intComp.StopInteraction(true);
                }
            }

            interactableComponents.Clear();
        }            
	}
}
