using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class CamMove : MonoBehaviour
{
    Vector3 currentAcc;
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        currentAcc = Vector3.zero;
        currentAcc.x = Input.acceleration.z;
        currentAcc.y = Input.acceleration.x;
        currentAcc.z = Input.acceleration.z;
        currentAcc/=rb.mass;
        rb.AddForce(currentAcc);

    }

}
