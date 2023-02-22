using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarOwnCtrl : MonoBehaviour
{
    private GameObject Car;//存放驾驶的车
    public MeshRenderer[] wheelMesh=new MeshRenderer[2];//车轮模型
    public WheelCollider[] wheel=new WheelCollider[2];//车轮碰撞体
    private float maxAngle;//最大转向角
    private float maxToque;
    private float h,v;
    //public Transform PosOfCar;//存放驾驶车的生成位置
    // Start is called before the first frame update
    void Start()
    {
        //加载resources下的预制体CarOwn
        //Car = Resources.Load<GameObject>("CarOwn");
        //insCar();//生成驾驶车
        //最大角度
        maxAngle = 20;
        maxToque = 100;
        //最大扭矩
        //找到轮子
        //wheelMesh = GameObject.Find("CarOwn").transform.GetChild(0).GetComponentsInChildren<MeshRenderer>();
        //wheel = GameObject.Find("CarOwn").transform.GetChild(1).GetComponentsInChildren<WheelCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");//键盘上下键
        v = Input.GetAxis("Vertical");//键盘左右键
        //if (0 == mathf.abs(h) && 0 == mathf.abs(v)) return;
        //move();
        animateWheels();
        if (Input.GetKey(KeyCode.W))
        {
            foreach (var o in wheel)
            {
                o.motorTorque = maxToque;
            }
        }
        if (Input.GetAxis("Horizontal") != 0)
        {
            for(int i = 0; i < 2; i++)
            {
                wheel[i].steerAngle = h * maxAngle;
            }
        }
        else
        {
            for(int i = 0; i < 2; i++)
            {
                wheel[i].steerAngle = 0;
            }
        }
    }
    //车轮转动动画
    private void animateWheels()
    {
        Vector3 wheelPosition = Vector3.zero;
        Quaternion wheelRotation = Quaternion.identity;

        for(int i = 0; i < 2; i++)
        {
            wheel[i].GetWorldPose(out wheelPosition, out wheelRotation);
            wheelMesh[i].transform.position = wheelPosition;
            //wheelRotation.eulerAngles = new Vector3(90f, 0f, 0f);
            wheelMesh[i].transform.rotation = wheelRotation;
        }
    }
    //移动
    private void Move()
    {
        //轮子转动
        for(int i = 0; i < 2; i++)
        {
            wheel[i].steerAngle = h * maxAngle;
        }
        
        //轮子转动
        for (int i = 0; i <2; i++){
            //轮子沿x轴转动要匹配转速，设定每分钟360转的话那么每秒的转速就要再除个60；同时要有转弯的表现
            wheelMesh[i].transform.localRotation = Quaternion.Euler(wheel[i].rpm * 180 / 60, 0, wheel[i].steerAngle);
        }
    }
   
    //生成驾驶车
    //private void inscar()
    //{
    //    gameobject _car=instantiate(car, posofcar.position, posofcar.rotation);
    //    _car.transform.parent = posofcar;
    //    _car.addcomponent<shotbullet>();
    //}
}
