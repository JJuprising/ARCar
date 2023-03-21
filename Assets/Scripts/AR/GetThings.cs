using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GetThings : MonoBehaviour
{
    public Text Tx_CoinNum;//金币数量
    public Text CirText;//记录圈数
    private int CountCir=0;//记录当前圈数
    
    [SerializeField] private Transform ScrollImageParent;

    [SerializeField]private RewardItem[] rewards;

    Reward currentItem;

    private void Update()
    {
        //使用道具
        if (StaticData.UseItemSign)
        {
            StaticData.UseItemSign = false;
            UseItem();
        }
    }


    private void OnTriggerEnter(Collider other)
    {

        Vector3 PicupVec = other.transform.position;//拾取对象的位置
        GameObject PickupEffect;//拾取效果

        switch (other.gameObject.name){
            case "StartPlace(Clone)":
                print("开始");
                //触发RaceMenu
                GameObject Canvas = GameObject.Find("Canvas");
                Canvas.transform.Find("RaceMenuPanel").gameObject.SetActive(true);
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
            case "Check":
                //记录圈数
                CountCir++;//碰到门前的检测体，圈数加一
                print("CountCir" + CountCir);
                CirText.text = CountCir.ToString();

                break;
            case "GoldBox":
                other.gameObject.SetActive(false);
                GetGoldenBoxReward();
                //Pick up effect
                PickupEffect = Instantiate(Resources.Load("PickupBox", typeof(GameObject))) as GameObject;
                PickupEffect.transform.position = PicupVec;
                Destroy(other.gameObject);//销毁物体
                break;
            case "Coin":
                Destroy(other.gameObject);
                Debug.Log("捡到金币");
                //Pick up effect
                PickupEffect = Instantiate(Resources.Load("PickupCoin", typeof(GameObject))) as GameObject;
                PickupEffect.transform.position = PicupVec;
                StaticData.CoinNum++;//全局的金币数加一
                StaticData.totalCoinNum++;
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
    

    Coroutine rolling;//roll道具动画协程
    /// <summary>
    /// 重置抽取动画与物品
    /// </summary>
    private void StopPreviousRolling()
    {
        currentItem = Reward.None;
        if (rolling != null)
        {
            StopCoroutine(rolling);
            for (int i = 0; i < ScrollImageParent.childCount; i++)
            {
                Destroy(ScrollImageParent.GetChild(i).gameObject);
            }
        }
    }
    /// <summary>
    /// 获取随机道具
    /// </summary>
    private void GetGoldenBoxReward()
    {
        RewardItem reward = rewards[Random.Range(0, rewards.Length)];
        rolling = StartCoroutine(RollingAward(reward));
    }
    /// <summary>
    /// roll道具动画
    /// </summary>
    /// <param name="reward">roll到的道具</param>
    IEnumerator RollingAward(RewardItem reward)
    {
        StopPreviousRolling();//先停止之前的rolling
        currentItem = reward.reward;//设置道具
        for (int j = 0; j < 2; j++)//循环播放随机动画
        {
            for (int i = 0; i < rewards.Length; i++)
            {
                GameObject go = Instantiate(Resources.Load<GameObject>("ScrollingItem"),ScrollImageParent);
                go.GetComponent<RawImage>().texture = rewards[i].texture;//设置贴图
                yield return new WaitForSeconds(0.25f);
                Destroy(go, 1f);
            }
        }
        GameObject end = Instantiate(Resources.Load<GameObject>("ScrollingItem"), ScrollImageParent);
        end.GetComponent<RawImage>().texture = reward.texture;
        end.GetComponent<ScrollingRawImage>().isFinalReward = true;//最后一轮动画特殊对待
        //Destroy(end, 5f);
        
    }
    /// <summary>
    /// 使用道具
    /// </summary>
    private void UseItem()
    {
        print("Using" + currentItem);
        switch (currentItem)
        {
            case Reward.bullet:
                UseBullet();
                break;
            case Reward.boost:
                UseBoost();
                break;
            case Reward.banana:
                UseBanana();
                break;
            default:
                break;
        }
        StopPreviousRolling();
        
    }
    private void UseBullet()
    {
        //生成炮弹
        GameObject bullet = Instantiate(Resources.Load("Bullet", typeof(GameObject))) as GameObject;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        Transform bulletRot = Camera.main.transform.GetChild(0);
        Vector3 bulletPos = Camera.main.transform.position;
        bullet.transform.localEulerAngles = new Vector3(bulletRot.rotation.x - 20, bulletRot.rotation.y, bulletRot.rotation.z); ;
        bulletPos.y -= (float)(0.7);
        bullet.transform.position = bulletPos;
        bullet.transform.eulerAngles = Camera.main.transform.forward;
        //加力，推动炮弹
        rb.AddForce(Camera.main.transform.forward * 800f);
        //三秒后销毁炮弹
        Destroy(bullet, 5);
        //音效
        GetComponent<AudioSource>().Play();
    }
    private void UseBoost()
    {

    }
    private void UseBanana()
    {

    }
}