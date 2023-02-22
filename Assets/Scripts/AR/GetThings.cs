using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GetThings : MonoBehaviour
{
    public Text Tx_CoinNum;//金币数量
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
        Vector3 PicupVec = collision.transform.position;//拾取对象的位置
        //拾取箱子功能
        if (collision.gameObject.tag == "GoldBox")
        {
            collision.gameObject.SetActive(false);
            //Pick up effect
            GameObject PickupEffect = Instantiate(Resources.Load("PickupBox", typeof(GameObject))) as GameObject;
            PickupEffect.transform.position = PicupVec;
            Destroy(collision.collider.gameObject);//销毁物体

        }
        //拾取金币
        else if (collision.gameObject.tag == "Coin")
        {
            collision.gameObject.SetActive(false);
            //Pick up effect
            GameObject PickupEffect = Instantiate(Resources.Load("PickupCoin", typeof(GameObject))) as GameObject;
            PickupEffect.transform.position = PicupVec;
            StaticData.CoinNum++;//全局的金币数加一
            Tx_CoinNum.text = StaticData.CoinNum.ToString();//UI金币数文本变化
            Destroy(collision.collider.gameObject);//销毁物体
        }
        
    }


}
