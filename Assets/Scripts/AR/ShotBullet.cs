using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnMouseDown()//���(��ָ)���
    {
        Debug.Log("1");
        //�����ڵ�
        GameObject bullet = Instantiate(Resources.Load("Bullet", typeof(GameObject))) as GameObject;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        bullet.transform.rotation = Camera.main.transform.rotation;
        bullet.transform.position = Camera.main.transform.position;
        //�������ƶ��ڵ�
        rb.AddForce(Camera.main.transform.forward * 500f);
        //����������ڵ�
        Destroy(bullet, 3);
    }
}
