using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_BasicCollectibleItem : MonoBehaviour
{
    [Header("Ensure your Collectible Manager uses the 'CollectibleManager' tag!")]
    [SerializeField]C_CollectibleManager Collectionmanager;
    

    private void Start()
    {
        if (!Collectionmanager)
        {
            Collectionmanager = GameObject.FindGameObjectWithTag("CollectibleManager").GetComponent<C_CollectibleManager>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            ///print("We hit an ITEM");

            Collectionmanager.registerItemPickedUp();

            Destroy(gameObject);
        }
    }
}
