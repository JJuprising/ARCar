using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarOwnCtrl : MonoBehaviour
{
    private GameObject Car;//��ż�ʻ�ĳ�
    public MeshRenderer[] wheelMesh=new MeshRenderer[2];//����ģ��
    public WheelCollider[] wheel=new WheelCollider[2];//������ײ��
    private float maxAngle;//���ת���
    private float maxToque;
    private float h,v;
    //public Transform PosOfCar;//��ż�ʻ��������λ��
    // Start is called before the first frame update
    void Start()
    {
        //����resources�µ�Ԥ����CarOwn
        //Car = Resources.Load<GameObject>("CarOwn");
        //insCar();//���ɼ�ʻ��
        //���Ƕ�
        maxAngle = 20;
        maxToque = 100;
        //���Ť��
        //�ҵ�����
        //wheelMesh = GameObject.Find("CarOwn").transform.GetChild(0).GetComponentsInChildren<MeshRenderer>();
        //wheel = GameObject.Find("CarOwn").transform.GetChild(1).GetComponentsInChildren<WheelCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");//�������¼�
        v = Input.GetAxis("Vertical");//�������Ҽ�
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
    //����ת������
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
    //�ƶ�
    private void Move()
    {
        //����ת��
        for(int i = 0; i < 2; i++)
        {
            wheel[i].steerAngle = h * maxAngle;
        }
        
        //����ת��
        for (int i = 0; i <2; i++){
            //������x��ת��Ҫƥ��ת�٣��趨ÿ����360ת�Ļ���ôÿ���ת�پ�Ҫ�ٳ���60��ͬʱҪ��ת��ı���
            wheelMesh[i].transform.localRotation = Quaternion.Euler(wheel[i].rpm * 180 / 60, 0, wheel[i].steerAngle);
        }
    }
   
    //���ɼ�ʻ��
    //private void inscar()
    //{
    //    gameobject _car=instantiate(car, posofcar.position, posofcar.rotation);
    //    _car.transform.parent = posofcar;
    //    _car.addcomponent<shotbullet>();
    //}
}
