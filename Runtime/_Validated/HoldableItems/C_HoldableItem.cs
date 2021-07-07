using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_HoldableItem : MonoBehaviour
{
    [HideInInspector] public bool isHeld = false;
    private PlayerItemInventory pInv;
    BoxCollider pickupCollider;
    Bounds bounds;
    Vector3 localCenter;
    [SerializeField] Vector3 DefaultPositionOffset = new Vector3(0.439999998f, -0.25999999f, 0.790000021f);
    [SerializeField] Vector3 DefaultRotationOffset = new Vector3(0f, 0f, 0f);
    [HideInInspector]public bool isInUse = false;
    // Start is called before the first frame update
    void Start()
    {
        setupCollisionForPickup();
    }
    void setupCollisionForPickup() 
    {
        CalculateLocalBounds();
        pickupCollider = gameObject.AddComponent<BoxCollider>();
        pickupCollider.isTrigger = true;
        pickupCollider.size = (bounds.size * 1.2f);
        pickupCollider.center = localCenter;
    }
    // Update is called once per frame
    public virtual void Update()
    {
        return;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
           // AddToPlayerInventory();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pInv = other.gameObject.GetComponent<PlayerItemInventory>();
            AddToPlayerInventory();
        }
    }
    void AddToPlayerInventory() 
    {
        pInv.heldItems.Add(gameObject);
        transform.SetParent(Camera.main.transform);
        transform.localPosition = DefaultPositionOffset;
        //transform.localRotation = Quaternion.Euler(-30f, 0, 0);
        transform.localRotation = Quaternion.Euler(DefaultRotationOffset);
        gameObject.SetActive(false);
        pInv.swapItem(pInv.heldItems.Count-1);
        print("Added to Player Inventory and Held Items.");
        isHeld = true;
        Destroy(pickupCollider);
    }

    public void ItemDropped() 
    {
        if (isInUse) 
        {
            stopUseItem();
        }
        pInv.dropItem(gameObject);
        isHeld = false;
        setupCollisionForPickup();
    }

    public virtual void startUseItem() 
    {

        print("started using: " + name);
        isInUse = true;
    }

    public virtual void stopUseItem() 
    {
        print("stopped using: " + name);
        isInUse = false;
    }

    private void CalculateLocalBounds()
    {
        Quaternion currentRotation = this.transform.rotation;
        this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        bounds = new Bounds(this.transform.position, Vector3.zero);

        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            bounds.Encapsulate(renderer.bounds);
        }

        localCenter = bounds.center - this.transform.position;
        bounds.center = localCenter;
        Debug.Log("The local bounds of this model is " + bounds);

        this.transform.rotation = currentRotation;
    }
}
