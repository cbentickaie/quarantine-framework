using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMover : MonoBehaviour
{

    public float moveSpeed = 3.14f;
    public float strafeSpeed = 1.14f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Move Forward
        transform.Translate((transform.forward * Input.GetAxis("Vertical")) * (moveSpeed * Time.deltaTime));

        //Move Right (Strafing Movement)
        transform.Translate((transform.right * Input.GetAxis("Horizontal")) * (strafeSpeed * Time.deltaTime));


    }
}
