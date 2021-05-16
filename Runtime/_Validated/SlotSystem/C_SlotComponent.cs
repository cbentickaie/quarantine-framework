using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_SlotComponent : MonoBehaviour
{
    public bool useInteractables = true;
    [SerializeField]
    List<Interactable> Interactables;
    [SerializeField]
    GameObject SlotKeyObject;
    [SerializeField]
    bool isOccupied = false;
    [SerializeField]
    bool snapObjectToSlot = true;
    Rigidbody keyRB;
    public bool useMeshPreview = false;
    Mesh KeyMeshIndicator;
    Collider SlotTriggerVolume;
    [ExecuteInEditMode]


    //THIS IS HACKY DONT EVER TELL ANYONE I DID THIS :S
    public bool useJamesDoorMode = false;
    public C_CheckCommodityToUnlockDoor CommodityChecker;
    public MultiDoor JamesDoor;
    private void Awake()
    {
        if (useMeshPreview && (KeyMeshIndicator = SlotKeyObject.GetComponentInChildren<MeshFilter>().mesh)) 
        {
            gameObject.GetComponentInChildren<MeshFilter>().mesh = KeyMeshIndicator;
            gameObject.GetComponentInChildren<MeshFilter>().gameObject.transform.localPosition = Vector3.zero;
            gameObject.GetComponentInChildren<MeshFilter>().gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
            gameObject.GetComponentInChildren<MeshFilter>().gameObject.transform.localScale = Vector3.one;
        }
        SlotTriggerVolume = GetComponent<Collider>();

    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Interactable i in gameObject.GetComponents<Interactable>()) 
        {
            Interactables.Add(i);
        }
        //Grabs the Parent door - handling very specific case for James/
        JamesDoor = CommodityChecker.gameObject.GetComponent<MultiDoor>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CompareToSlotKey(other.gameObject) && !isOccupied)
        {
            SlotKeyObject.transform.parent = gameObject.transform;
            if (keyRB = SlotKeyObject.GetComponent<Rigidbody>())
            {
                if (keyRB.useGravity) 
                {
                    keyRB.useGravity = false;
                }
                if (!keyRB.isKinematic) 
                {
                    keyRB.isKinematic = true;
                }
            }
            else 
            {
                SlotKeyObject.transform.parent = gameObject.transform;
            }
            if (snapObjectToSlot)
            {
                SlotKeyObject.transform.localPosition = Vector3.zero;
                SlotKeyObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
            }
            isOccupied = true;
            SlotOccupied();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (CompareToSlotKey(other.gameObject))
        {

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (CompareToSlotKey(other.gameObject) && isOccupied) 
        {
            isOccupied = false;
            SlotEmptied();
        }
    }
    bool CompareToSlotKey(GameObject Go) 
    {
        if (Go == SlotKeyObject) 
        {
            print("desired object compared");
            return true;
           
        }
        else 
        {
            print("non desired object compared");
            print(Go.name + " non desired");
            return false;
        }
    }
    void SlotOccupied() 
    {
        if (useInteractables) 
        {
            foreach (Interactable i in Interactables)
            {
                i.StartInteraction();
            }
        }

        if (useJamesDoorMode && CommodityChecker && JamesDoor.isLocked)
        {
            CommodityChecker.openDoorOnUnlock = true;
        }
        else if (useJamesDoorMode && CommodityChecker && !JamesDoor.isLocked) 
        {
            JamesDoor.OpenDoor();
        }
        SlotTriggerVolume.enabled = false;
    }

    void SlotEmptied() 
    {
        if (useInteractables)
        {
            foreach (Interactable i in Interactables)
            {
                i.StopInteraction();
            }
        }
        SlotTriggerVolume.enabled = false;
    }
    void OnDrawGizmos()
    {
        // Draws the desired icon at position of the object.
        // Because we draw it inside OnDrawGizmos the icon is also pickable
        // in the scene view.

        Gizmos.DrawIcon(transform.position, "plug.png", true);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawIcon(transform.position, "key - card.png", true);
        Gizmos.color = Color.yellow;
        foreach (Interactable i in Interactables)
        {
            Gizmos.DrawLine(transform.position, i.gameObject.transform.position);
        }
        Gizmos.color = Color.blue;
        if (SlotKeyObject) 
        {
            Gizmos.DrawLine(transform.position, SlotKeyObject.transform.position);
        }
    }

    
}
