using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollTire : Singleton<RollTire>
{
    [SerializeField] Transform parent;
    //[SerializeField] Transform LeftParent;
    [SerializeField] Transform LeftChild;
    //[SerializeField] Transform RightParent;
    [SerializeField] Transform RightChild;
    public float rollingSpeed = 5f;
    public float turningSpeed = 5f;
    public float turnningAngel = 15f;

    private float currentAngel = 0f;
    private float currentSpeed = 0f;
    private float targetSpeed = 0f;
    private float targetAngel = 0f;
    private void Start()
    {
        parent.eulerAngles = Vector3.zero;
        LeftChild.eulerAngles = Vector3.zero;

        RightChild.eulerAngles = Vector3.zero;
    }
    private void Update()
    {
        InputTest();
        SetRot();
    }
    private void SetRot()
    {
        currentSpeed = Mathf.MoveTowards(currentSpeed,targetSpeed,Time.deltaTime*turningSpeed);
        LeftChild.Rotate(Vector3.right * Time.deltaTime * currentSpeed, Space.Self);
        RightChild.Rotate(Vector3.right * Time.deltaTime * currentSpeed, Space.Self);

        currentAngel = Mathf.MoveTowards(currentAngel, targetAngel, Time.deltaTime * turningSpeed);
        parent.localEulerAngles = Vector3.forward * currentAngel;

    }
    public void InputTest()
    {
        StopRolling();
        TurnFront();
        if (Input.GetKey(KeyCode.W))
        {
            ForwardRolling();
        }else if (Input.GetKey(KeyCode.S))
        {
            BackwardRolling();
        }
        if (Input.GetKey(KeyCode.A))
        {
            TurnLeft();
        }
        else if(Input.GetKey(KeyCode.D))
        {
            TurnRight();
        }
    }
    public void ForwardRolling()
    {
        targetSpeed = rollingSpeed;

    }
    public void BackwardRolling()
    {
        targetSpeed = -rollingSpeed;

    }
    public void StopRolling()
    {
        targetSpeed = 0f;

    }
    public void TurnLeft()
    {
        targetAngel = -turnningAngel;
    }
    public void TurnRight()
    {
        targetAngel = turnningAngel;
    }
    public void TurnFront()
    {
        targetAngel = 0f;
    }
}
