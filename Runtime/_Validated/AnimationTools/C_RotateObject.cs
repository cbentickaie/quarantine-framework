using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LocalDirection {Forward, Up, Right, Left, Down, Backward}

public class C_RotateObject : MonoBehaviour
{
    [SerializeField] LocalDirection RotationAxis = LocalDirection.Forward;
    [SerializeField] float RotateSpeed = 1.0f;
    Vector3 spinAxis;
    [SerializeField] bool pulseSpin = false;
    float rotationMultiplier = 1;
    //s Start is called before the first frame update
    void Start()
    {
        switch (RotationAxis) 
        {
            case LocalDirection.Up:
                spinAxis = transform.up;
                break;
            case LocalDirection.Forward:
                spinAxis = transform.forward;
                break;
            case LocalDirection.Right:
                spinAxis = transform.right;
                break;

            case LocalDirection.Down:
                spinAxis = (transform.up * -1);
                break;
            case LocalDirection.Backward:
                spinAxis = (transform.forward * -1);
                break;
            case LocalDirection.Left:
                spinAxis = (transform.right * -1); ;
                break;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pulseSpin) 
        {
            rotationMultiplier += Time.deltaTime * Mathf.Cos(Time.fixedTime);
            rotationMultiplier = Mathf.Clamp(rotationMultiplier, 1.0f, 3.14f);
        }
        transform.Rotate(spinAxis, Time.deltaTime * (RotateSpeed * rotationMultiplier), Space.World );
    }
}
