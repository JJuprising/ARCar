using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            
            //�����ڵ�
            GameObject bullet = Instantiate(Resources.Load("Bullet", typeof(GameObject))) as GameObject;
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            Transform bulletRot = Camera.main.transform.GetChild(0);
            Vector3 bulletPos = Camera.main.transform.position;
            bullet.transform.localEulerAngles = new Vector3(bulletRot.rotation.x-20, bulletRot.rotation.y, bulletRot.rotation.z); ;
            bulletPos.y -= (float)(0.7);
            bullet.transform.position = bulletPos;
            //�������ƶ��ڵ�
            rb.AddForce(Camera.main.transform.forward * 800f);
            //����������ڵ�
            Destroy(bullet, 5);
            //��Ч
            GetComponent<AudioSource>().Play();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
