using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallLauncher : MonoBehaviour
{
    [SerializeField] float FiringRate = 0.3f;
    [SerializeField] Rigidbody ProjectilePrefab;
    [SerializeField] float launchSpeed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("AutoFire");
    }

    IEnumerator AutoFire()
    {
        while (true)
        {
            SpawnProjectile();
            yield return new WaitForSeconds(FiringRate);
            print("Auto Bullet Fired");
        }
    }

    void SpawnProjectile()
    {
        //this is an incredibly basic Rigidbody projectile method, it is dependant on the bullet prefab handling it's own launch velocity, 
        //see the ProjectileLaunch Script for an example of this implementation, you could also apply a rigidbody impulse here if you wanted to.
        Rigidbody NewProjectile = (Rigidbody)Instantiate(ProjectilePrefab, this.transform.position, this.transform.rotation) as Rigidbody;
        NewProjectile.AddForce(NewProjectile.transform.forward * launchSpeed, ForceMode.Impulse);
    }
}
