using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TrialMgr : Singleton<TrialMgr>
{
    public Text TimeText;//计时UI
    public Text Cir1Time;//第一圈的时间
    public Text Cir2Time;//第二圈的时间
    public Text Cir3Time;//第三圈的时间
    public Text totalTime;//总时间
    public Text CirText;//圈数文本
    private int CountCir;//圈数
    private float CountTime;//计时
    private int hour,min,sec;
    private string msecStr;
    private bool isShowMlSec=false;//初始化为0
    private string []cirTime=new string[3];//记录每一圈时间的字符串
    private float CountTime2 = 0;//单圈计时器
    private int hour2, min2, sec2;
    private string mesecStr2;
    private bool setZero;//记录计时器清理，方式重复清0
    public GameObject gate1 = null;
    public GameObject gate2 = null;
    public GameObject gate3 = null;
    public GameObject gate4 = null;
    public GameObject StartPlace = null;

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        //给圈数赋值
        CountCir = int.Parse(CirText.text);

        if (StaticData.GateObserved[3] == 1)
        {
            //第四个门已经识别，判断是否四个门都是别好的
            int count = 0;
            for (int i = 0; i < StaticData.GateObserved.Length; i++)
            {
                //Debug.Log(i + ":" + StaticData.GateObserved[i]);
                count += StaticData.GateObserved[i];
            }
            if (count == 4)
            {
                StaticData.isObservedFinshed = true;//识别好了4个，标记为true
                
            }

        }
        
        if (isShowMlSec&& StaticData.EndTimeTrial==false)//触发计时且圈还没跑完
        {
            // 计时总时间
            CountTime += Time.deltaTime;
            hour = (int)CountTime / 3600;
            min = (int)(CountTime - hour * 3600) / 60;
            sec = (int)(CountTime - hour * 3600 - min * 60);
            msecStr = isShowMlSec ? ("." + ((int)((CountTime - (int)CountTime) * 10)).ToString("D1")) : "";
            TimeText.text =  min.ToString("D2") + ":" + sec.ToString("D2") + msecStr;
        }
        if(StaticData.EndTimeTrial == false)
        {
            //记录每圈用时
            switch (CountCir)
            {
                case 2:
                    //第一圈完成，记录时间
                    if (setZero == false)
                    {
                        cirTime[0] = TimeText.text;
                        setZero = true;
                    }
                    
                    //记录第二圈用时
                    CountTime2 += Time.deltaTime;
                    print("第一圈完成，记录时间:"+ cirTime[0]);
                    break;
                case 3:
                    //第二圈完成，记录时间
                    hour2 = (int)CountTime2 / 3600;
                    min2 = (int)(CountTime2 - hour2 * 3600) / 60;
                    sec2 = (int)(CountTime2 - hour2 * 3600 - min * 60);
                    msecStr = "." + ((int)((CountTime2 - (int)CountTime2) * 10)).ToString("D1");

                    if (setZero)
                    {
                        CountTime2 = 0;//清0重新计时
                        cirTime[1] = min2.ToString("D2") + ":" + sec2.ToString("D2") + msecStr;//第二圈用时
                        setZero =false;
                    }
                    CountTime2 += Time.deltaTime;
                    print("第二圈完成，记录时间:"+ cirTime[1]);
                    break;
                case 4:
                    hour2 = (int)CountTime2 / 3600;
                    min2 = (int)(CountTime2 - hour2 * 3600) / 60;
                    sec2 = (int)(CountTime2 - hour2 * 3600 - min * 60);
                    msecStr = "." + ((int)((CountTime2 - (int)CountTime2) * 10)).ToString("D1");
                    cirTime[2] = min2.ToString("D2") + ":" + sec2.ToString("D2") + msecStr;//第二圈用时

                    // 第三圈经过第四个门，激活第一个门下的Finish物体
                    GameObject Camera;
                    Camera = GameObject.Find("ARCamera");
                    //隐藏的不能用gameobject.find,需要挂在可见下面，用tranform.find
                    GameObject FinishObject = Camera.transform.Find("finish").gameObject;
                    FinishObject.SetActive(true);
                    Destroy(FinishObject, 1);
                    totalTime.text = TimeText.text;//记录总时间
                    StaticData.EndTimeTrial = true;//游戏结束标记
                    print("第三圈完成，记录时间:"+ cirTime[2]);
                    break;

            }
        }
        
       
        if (StaticData.EndTimeTrial)
        {
            //GetThing.cs检测到Finish被撞击，说明已经经过终点，进入结算界面
            EndTrial();
        }
    }
   
  
    public void StartCountTime()
    {

        //if (StaticData.isObservedFinshed)
        //{
            isShowMlSec = true;//开启计时
        //}
    }
    public void EndTrial()
    {
        //比赛结束函数
        //打破纪录效果
        GameObject Camera;

        //将用时数据进行存储，如果记录的数组不为空，则比较是否打破记录
        string MinTime = StaticData.TimeRecord[0];
        for(int i = 0; i < StaticData.TimeRecord.Length; i++)
        {
            
            if (StaticData.TimeRecord[i] == null)
            {
                //为0时记录,转为秒
                StaticData.TimeRecord[i] = totalTime.text;
                
                if (StaticData.TimeRecord[i] .CompareTo(MinTime)<0)
                {
                    
                    Camera = GameObject.Find("ARCamera");
                    //隐藏的不能用gameobject.find,需要挂在可见下面，用tranform.find
                    GameObject newrecord=Camera.transform.Find("newrecord").gameObject;
                    newrecord.SetActive(true);
                    Destroy(newrecord, 2);
                }
                break;
            }else if(StaticData.TimeRecord[i].CompareTo(MinTime)<0&&i!=0) {
                //记录最少时间圈数
                MinTime = StaticData.TimeRecord[i];
            }
        }
        //显示结算界面，显示三圈花费的时间
        GameObject Canvas;
        Canvas = GameObject.Find("Canvas");
        //隐藏的不能用gameobject.find,需要挂在可见下面，用tranform.find
        Canvas.transform.Find("BillingPanel").gameObject.SetActive(true);
        for(int i = 0; i < cirTime.Length; i++)
        {
            Debug.Log("第" + i + "圈:" + cirTime[i]);
        }
        Cir1Time.text = cirTime[0];
        Cir2Time.text = cirTime[1];
        Cir3Time.text = cirTime[2];
        //隐藏其余面板
        Canvas.transform.Find("CoinPanel").gameObject.SetActive(false);
        Canvas.transform.Find("CirPanel").gameObject.SetActive(false);
        Canvas.transform.Find("ToolsPanel").gameObject.SetActive(false);
        Canvas.transform.Find("CountTime").gameObject.SetActive(false);
    }

    public void ResetGame()
    {
        StaticData.CoinNum = 0;
        StaticData.isObservedFinshed = false;
        for (int i = 0; i < StaticData.GateObserved.Length; i++)
        {
            StaticData.GateObserved[i] = 0;
        }
        isShowMlSec = false;
        CountTime = 0;
        TimeText.text = "";
        hour = min = sec = 0;
        msecStr = "";
        CountCir = 0;

        //重置面板
        GameObject Camera = GameObject.Find("ARCamera");
        GameObject Canvas = GameObject.Find("Canvas");
        //显示其余面板
        Canvas.transform.Find("CoinPanel").gameObject.SetActive(true);//金币栏
        Canvas.transform.Find("CirPanel").gameObject.SetActive(true);//圈数栏
        Canvas.transform.Find("ToolsPanel").gameObject.SetActive(true);//道具栏
        Canvas.transform.Find("CountTime").gameObject.SetActive(true);//计时栏
        Canvas.transform.Find("BillingPanel").gameObject.SetActive(false);//隐藏结算栏
    }
    //按钮事件
    public void Btn_GoToLanuch()
    {
        SceneManager.LoadScene("UI");
    }
    public void Btn_Reset()
    {
        ResetGame();
    }
    public void Btn_UseItem()
    {
        //按钮触发道具，传递参数
        StaticData.UseItemSign = true;
    }
}
