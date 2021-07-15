using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class C_SlotComponent : MonoBehaviour
{
    [HideInInspector] public bool useInteractables = true;
    [SerializeField]
    [HideInInspector] List<Interactable> Interactables;
    [SerializeField]
    GameObject SlotKeyObject;
    [SerializeField]
    [HideInInspector] bool isOccupied = false;
    [SerializeField]
    bool snapObjectToSlot = true;
    Rigidbody keyRB;
    public bool useMeshPreview = false;
    Mesh KeyMeshIndicator;
    Collider SlotTriggerVolume;
    [ExecuteInEditMode]
    [SerializeField] Material previewMaterial;
    public UnityEvent SlotOccupiedEvent;
    //THIS IS HACKY DONT EVER TELL ANYONE I DID THIS :S
    [HideInInspector] public bool useCommodityDoorMode = false;
    [HideInInspector] public C_CheckCommodityToUnlockDoor DoorCommodityChecker;
    public MultiDoor MultiDoor;
    [SerializeField] List<MeshFilter> SourceMeshes;
    private void Awake()
    {


    }


    void collectAndMergeRenderers() 
    {
        //Add primary Meshfilters to SourceMeshes
        foreach (MeshFilter mesh in SlotKeyObject.GetComponentsInChildren<MeshFilter>()) 
        {
            SourceMeshes.Add(mesh);
            
        }
        for (int i = 0; i < SourceMeshes.Count; i++) 
        {
            GameObject curGO;
            MeshFilter curMF;
            MeshRenderer curMR;
            MeshFilter m = SourceMeshes[i];
            curGO = new GameObject("SlotPreviewmesh_" + m.name);

            curGO.transform.rotation = m.gameObject.transform.rotation;
            Vector3 curOffset = (m.gameObject.transform.position - gameObject.transform.position);
            if (i != 0)
            {
                Vector3 worldpos = m.gameObject.transform.TransformPoint(m.gameObject.transform.position);
                curGO.transform.position = worldpos;
            }
            else 
            {
                curGO.transform.position = transform.position;
            }
            

            //curGO.transform.position = m.gameObject.transform.position + curOffset;
            
            print("SlotPreviewmesh_" + m.name);
            //curGO.transform.parent = gameObject.transform;
            //curGO.transform.SetParent(gameObject.transform);
            curMF = curGO.AddComponent<MeshFilter>();
            curMF.mesh = m.mesh;
            curMR = curGO.AddComponent<MeshRenderer>();
            curMR.material = previewMaterial;

        }

    }

    void setupPreviewMesh() 
    {
        GameObject preview = GameObject.Instantiate(SlotKeyObject, gameObject.transform);
        preview.transform.localPosition = Vector3.zero;

        foreach (Transform T in preview.transform) 
        {            
                foreach (Component c in T.gameObject.GetComponents(typeof(Component))) 
                {
                    if (c is MeshFilter || c is MeshRenderer || c is Transform)
                    {
                    
                    }
                    else 
                    {
                    print("Slot Destroying: " + c.name);
                        Destroy(c);
                    }      
                }            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
       // setupPreviewMesh();
        //collectAndMergeRenderers();
        if (useMeshPreview && (KeyMeshIndicator = SlotKeyObject.GetComponentInChildren<MeshFilter>().mesh))
        {
            //gameObject.GetComponentInChildren<MeshFilter>().mesh = KeyMeshIndicator;
            //gameObject.GetComponentInChildren<MeshFilter>().gameObject.transform.localPosition = Vector3.zero;
            //gameObject.GetComponentInChildren<MeshFilter>().gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
            //gameObject.GetComponentInChildren<MeshFilter>().gameObject.transform.localScale = Vector3.one;
        }
        SlotTriggerVolume = GetComponent<Collider>();

        foreach (Interactable i in gameObject.GetComponents<Interactable>()) 
        {
            Interactables.Add(i);
        }
        if (!MultiDoor) 
        {
            //Grabs the Parent door - handling very specific case for James/
           // MultiDoor = DoorCommodityChecker.gameObject.GetComponent<MultiDoor>();
        }        
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

            if (SlotKeyObject.GetComponent<C_HoldableItem>()) 
            {
                SlotKeyObject.GetComponent<C_HoldableItem>().ItemDropped();
            }
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
        SlotOccupiedEvent.Invoke();
        if (useInteractables) 
        {
            foreach (Interactable i in Interactables)
            {
                i.StartInteraction();
            }
        }

        if (useCommodityDoorMode && DoorCommodityChecker && MultiDoor.isLocked)
        {
            DoorCommodityChecker.openDoorOnUnlock = true;
        }
        else if (useCommodityDoorMode && DoorCommodityChecker && !MultiDoor.isLocked) 
        {
            MultiDoor.OpenDoor();
        }

        if (!useCommodityDoorMode && MultiDoor) 
        {
            MultiDoor.UnlockDoor();
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
