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
   
    private void OnTriggerEnter(Collider other)
    {
        Vector3 PicupVec = other.transform.position;//拾取对象的位置
        GameObject PickupEffect;//拾取效果

        switch (other.gameObject.name){
            case "StartPlace":
                //print("开始");
                //如果碰到的是StartPlace的特效点，开始计时，两秒后游戏开始
                StartCoroutine(delaythreeSec());
                //出发倒计时三秒然后开始的特效
                Destroy(other.gameObject, 3f);//销毁物体
                break;

            case "Gate1_ImageTarget":
                if (StaticData.CheckGatePassingValidity(0))
                {
                    StaticData.GateObserved[0]++;
                    //StaticData.printScore();
                }
                
                break;
            case "Gate2_ImageTarget":
                if (StaticData.CheckGatePassingValidity(1))
                {
                    StaticData.GateObserved[1]++;
                    //StaticData.printScore();
                }
                break;
            case "Gate3_ImageTarget":
                if (StaticData.CheckGatePassingValidity(2))
                {
                    StaticData.GateObserved[2]++;
                    //StaticData.printScore();
                }
                break;
            case "Gate4_ImageTarget":
                if (StaticData.CheckGatePassingValidity(3))
                {
                    StaticData.GateObserved[3]++;
                    //StaticData.printScore();
                }
                break;
        }
        switch (other.gameObject.tag) {
            case "GoldBox":
                other.gameObject.SetActive(false);
                //Pick up effect
                PickupEffect = Instantiate(Resources.Load("PickupBox", typeof(GameObject))) as GameObject;
                PickupEffect.transform.position = PicupVec;
                Destroy(other.gameObject);//销毁物体
                break;
            case "Coin":
                other.gameObject.SetActive(false);
                //Pick up effect
                PickupEffect = Instantiate(Resources.Load("PickupCoin", typeof(GameObject))) as GameObject;
                PickupEffect.transform.position = PicupVec;
                StaticData.CoinNum++;//全局的金币数加一
                Tx_CoinNum.text = StaticData.CoinNum.ToString();//UI金币数文本变化
                Destroy(other.gameObject);//销毁物体
                break;
            case "Finish":
                print("通过Finish物体");
                Destroy(other.gameObject);
                StaticData.EndTimeTrial = true;//TimeTrial结束标记，交给TrialMgr处理结束界面
                                               //生成finish的提示
                GameObject finishObject = this.transform.GetChild(0).gameObject;
                finishObject.SetActive(true);
                Destroy(finishObject, 2);
                //打破记录动画
                break;
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
