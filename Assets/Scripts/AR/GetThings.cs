using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GetThings : MonoBehaviour
{
    public Text Tx_CoinNum;//�������
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    public void OnCollisionEnter(Collision collision)
    {
        Vector3 PicupVec = collision.transform.position;//ʰȡ�����λ��
        //ʰȡ���ӹ���
        if (collision.gameObject.tag == "GoldBox")
        {
            collision.gameObject.SetActive(false);
            //Pick up effect
            GameObject PickupEffect = Instantiate(Resources.Load("PickupBox", typeof(GameObject))) as GameObject;
            PickupEffect.transform.position = PicupVec;
            Destroy(collision.collider.gameObject);//��������

        }
        //ʰȡ���
        else if (collision.gameObject.tag == "Coin")
        {
            collision.gameObject.SetActive(false);
            //Pick up effect
            GameObject PickupEffect = Instantiate(Resources.Load("PickupCoin", typeof(GameObject))) as GameObject;
            PickupEffect.transform.position = PicupVec;
            StaticData.CoinNum++;//ȫ�ֵĽ������һ
            Tx_CoinNum.text = StaticData.CoinNum.ToString();//UI������ı��仯
            Destroy(collision.collider.gameObject);//��������
        }
        
    }


}
