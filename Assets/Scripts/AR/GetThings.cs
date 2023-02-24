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
    private void OnTriggerEnter(Collider other)
    {
        
        
        if (other.gameObject.name == "StartPlace")
        {
            print("进入");
            //如果碰到的是StartPlace的特效点，开始计时，两秒后游戏开始
            StartCoroutine(delaythreeSec());
            //出发倒计时三秒然后开始的特效

            Destroy(other.gameObject, 3f);//销毁物体

        }
        
    }
    //显示倒计时三秒动画
    private IEnumerator delaythreeSec()
    {
        Vector3 NumberVec = this.transform.position;
        NumberVec.z += 2.5f;//图标相对摄像机位置
        bool isMerge = true;//判断是否已经生成了数字
        float time = 0;//计时
        GameObject Number3Object = null, Number2Object = null, Number1Object = null, GoObject = null;
        while (time < 3)
        {
            print(time);
            time += Time.deltaTime;
            
            //生成计时图标
            if (time <= 1&&time>=0&&isMerge)
            {
                //3
                
                isMerge = false;
                Number3Object = Instantiate(Resources.Load("number3", typeof(GameObject))) as GameObject;
                Number3Object.transform.position = NumberVec;
                Number3Object.transform.parent = this.transform;
            }
            
            if (time > 1&&time<=2&&isMerge==false)
            {
                //2
                
                isMerge = true;
                Number2Object = Instantiate(Resources.Load("number2", typeof(GameObject))) as GameObject;
                NumberVec.z += 0.01f;
                Number2Object.transform.position = Number3Object.transform.position;
                Number2Object.transform.parent = this.transform;
                Destroy(Number3Object);//销毁物体
            }
            if (time > 2&&time<3&&isMerge==true)
            {
                //1
                
                isMerge =false;
                
                Number1Object = Instantiate(Resources.Load("number1", typeof(GameObject))) as GameObject;
                NumberVec.z -= 0.01f;
                Number1Object.transform.position = Number2Object.transform.position;
                Number1Object.transform.parent = this.transform;
                Destroy(Number2Object);
            }
            yield return null;
        }
        Destroy(Number1Object);
        
        GoObject = Instantiate(Resources.Load("go_2", typeof(GameObject))) as GameObject;
        NumberVec.z += 0.01f;
        GoObject.transform.position = Number1Object.transform.position;
        GoObject.transform.parent = this.transform;
        Destroy(Number1Object);
        Destroy(GoObject, 0.3f);
        TrialMgr.Instance.startCountTime();//三秒后开始计时
    }
}
