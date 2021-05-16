using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponScript : MonoBehaviour
{
    [Header("Variables")]
    Animator MeleeAnimator;
    public GameObject impactFx;
    [SerializeField] GameObject WeaponMesh;
    [SerializeField] float meleeDamage = 16.0f;
    public bool useTriggerForOverlap = false;
    //[Space(10)]
    [SerializeField]bool isAttacking = false;
    [SerializeField] LayerMask playerLayerMask;
    Material indicatorMat;
    public float aiMeleeAttackDuration = 2.4f;

    // Start is called before the first frame update
    void Start()
    {
        MeleeAnimator = GetComponent<Animator>();
        if (!MeleeAnimator)
        {
            this.enabled = false;
        }
        if (useTriggerForOverlap)
        {
            //GetComponent<Collider>().isTrigger = true;
        }
        if (WeaponMesh)
        {
            indicatorMat = WeaponMesh.GetComponent<Renderer>().material;
        }
        else if (!WeaponMesh)
        {
            print("You have not assigned a Gameobject to the WeaponMesh variable in the Melee Weapon Script. If you want debug on the weapon's state drop your Weapon Mesh (the object with the desired Mesh renderer component) here");
        }
        Collider col;
        if (col = GetComponent<Collider>()) 
        {
            //col.ma
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //Handle input when the weapon is fired, and call our ActivateWeapon method.
        if (Input.GetButtonDown("Fire1"))
        {
            if (!isAttacking)
            {
                ActivateMeleeWeapon();
                isAttacking = true;
                //print("Melee Attacking!");

                indicatorMat.color = Color.yellow;
            }
            //meleeCollision
        }

        //Handle when the player releases the Fire input, in this case stop the auto firing coroutines
        if (Input.GetButtonUp("Fire1"))
        {
            StartCoroutine(DeactivateMeleeWeapon());
            //isAttacking = false;            
            //print("Melee Ceasfire!");
        }

        if (isAttacking)
        {
            if (MeleeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
            {                
                //isAttacking = false;
               // indicatorMat.color = Color.green;
            }
        }
    }

    public void AIMeleeAttack()
    {
        ActivateMeleeWeapon();
        isAttacking = true;
        indicatorMat.color = Color.yellow;
        StartCoroutine(AIMeleeStopAttack());
    }

    IEnumerator AIMeleeStopAttack()
    {
        //MeleeAnimator.GetCurrentAnimatorStateInfo(0).length * MeleeAnimator.GetCurrentAnimatorStateInfo(0)
        
        yield return new WaitForSeconds(aiMeleeAttackDuration);
        MeleeAnimator.SetBool("isAttacking", false);
        isAttacking = false;
        if (!useTriggerForOverlap)
        {
            gameObject.GetComponent<Collider>().isTrigger = false;
        }
    }

    void ActivateMeleeWeapon()
    {
        MeleeAnimator.SetBool("isAttacking", true);
    }

    IEnumerator DeactivateMeleeWeapon()
    {
        //MeleeAnimator.GetCurrentAnimatorStateInfo(0).length * MeleeAnimator.GetCurrentAnimatorStateInfo(0)
        MeleeAnimator.SetBool("isAttacking", false);
        yield return new WaitForSeconds(MeleeAnimator.GetCurrentAnimatorStateInfo(0).length - (MeleeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime * MeleeAnimator.GetCurrentAnimatorStateInfo(0).length));
        
        isAttacking = false;

        if (!useTriggerForOverlap)
        {
            gameObject.GetComponent<Collider>().isTrigger = false;
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("Collision Entered"); indicatorMat.color = Color.blue;
        if (isAttacking && collision.gameObject.tag != "Player")
        {
            gameObject.GetComponent<Collider>().isTrigger = true;
            if (impactFx) { GameObject.Instantiate(impactFx, collision.GetContact(0).point, collision.transform.rotation); }            
            collision.gameObject.GetComponent<DamageHandler>().ApplyDamage(meleeDamage);
            indicatorMat.color = Color.red;
            print("Collided with: " + collision.collider.gameObject.name);
            


        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (isAttacking && collision.gameObject.tag != "Player")
        {
            gameObject.GetComponent<Collider>().isTrigger = false;
            //GameObject.Instantiate(impactFx, collision.GetContact(0).point, collision.transform.rotation);
            //collision.gameObject.GetComponent<DamageHandler>().ApplyDamage(meleeDamage);
            indicatorMat.color = Color.blue;
            print("Ended collision with: " + collision.collider.gameObject.name);



        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.name);
        if (isAttacking && (other.gameObject.tag != "Player"))
        {
            GameObject.Instantiate(impactFx, other.transform);
            if (other.gameObject.GetComponent<DamageHandler>())
            {
                other.gameObject.GetComponent<DamageHandler>().ApplyDamage(meleeDamage);
                print(other.gameObject.name);
            }            
            indicatorMat.color = Color.red;

        //Optional impulse application
            //other.gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * meleeDamage);
            //isAttacking = false;
            //DeactivateMeleeWeapon();
        }
        
    }
}
