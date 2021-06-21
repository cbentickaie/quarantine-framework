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
            print("Projectile Launched");
        }
    }

    void SpawnProjectile()
    {
        //this is an incredibly basic Rigidbody projectile method, we Instantiate or 'Spawn' a new Gameobject from our specified Prefab. This prefab must use a Rigidbody component in order to apply Physics.
        
        Rigidbody NewProjectile = (Rigidbody)Instantiate(ProjectilePrefab, this.transform.position, this.transform.rotation) as Rigidbody;

        //We then apply a rigidbody impulse, forcing the Rigidbody to move as if launched at the velocity we specify for launchSpeed.
        NewProjectile.AddForce(NewProjectile.transform.forward * launchSpeed, ForceMode.Impulse);
    }
}
