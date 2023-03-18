using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;


    public float maxWheelAngle = 30f;  // 最大转向角度
    public float maxTorque = 100f;  // 最大扭矩

    private Rigidbody rb;
    private float currentWheelAngle = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float steerInput = Input.GetAxis("Horizontal");
        float accelerateInput = Input.GetAxis("Vertical");

        currentWheelAngle = Mathf.Lerp(currentWheelAngle, steerInput * maxWheelAngle, Time.deltaTime * 5f);

        frontLeftWheel.steerAngle = currentWheelAngle;
        frontRightWheel.steerAngle = currentWheelAngle;

        frontLeftWheel.motorTorque = accelerateInput * maxTorque;
        frontRightWheel.motorTorque = accelerateInput * maxTorque;

    }
}


