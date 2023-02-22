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
    
    void OnMouseDown()//鼠标(手指)点击
    {
        Debug.Log("1");
        //生成炮弹
        GameObject bullet = Instantiate(Resources.Load("Bullet", typeof(GameObject))) as GameObject;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        bullet.transform.rotation = Camera.main.transform.rotation;
        bullet.transform.position = Camera.main.transform.position;
        //加力，推动炮弹
        rb.AddForce(Camera.main.transform.forward * 500f);
        //三秒后销毁炮弹
        Destroy(bullet, 3);
    }
}
