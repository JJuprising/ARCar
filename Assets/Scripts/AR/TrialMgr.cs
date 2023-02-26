using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TrialMgr : Singleton<TrialMgr>
{
    public Text TimeText;//计时UI
    public Text Cir1Time;//第一圈
    public Text Cir2Time;//第二圈
    public Text Cir3Time;//第三圈
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
            switch (StaticData.GateObserved[0])
            {
                case 3:
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
                case 4:
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
                case 5:
                    hour2 = (int)CountTime2 / 3600;
                    min2 = (int)(CountTime2 - hour2 * 3600) / 60;
                    sec2 = (int)(CountTime2 - hour2 * 3600 - min * 60);
                    msecStr = "." + ((int)((CountTime2 - (int)CountTime2) * 10)).ToString("D1");
                    cirTime[2] = min2.ToString("D2") + ":" + sec2.ToString("D2") + msecStr;//第二圈用时

                                                                                           // 第三圈经过第四个门，激活第一个门下的Finish物体
                    GameObject FinishObject;
                    FinishObject = GameObject.Find("finish");
                    FinishObject.SetActive(true);
                    StaticData.EndTimeTrial = true;//游戏结束标记
                    print("第三圈完成，记录时间:"+ cirTime[2]);
                    break;

            }
        }
        
       
        if (StaticData.EndTimeTrial)
        {
            //GetThing.cs检测到Finish被撞击，说明已经经过终点，进入结算界面
            endTrial();
        }
    }
   
  
    public void startCountTime()
    {

        //if (StaticData.isObservedFinshed)
        //{
            isShowMlSec = true;//开启计时
        //}
    }
    public void endTrial()
    {
        //比赛结束函数
        
        
        //将用时数据进行存储，如果记录的数组不为空，则比较是否打破记录
        string MinTime = StaticData.TimeRecord[0];
        for(int i = 0; i < StaticData.TimeRecord.Length; i++)
        {
            if (StaticData.TimeRecord[i] == "")
            {
                //为0时记录,转为秒
                StaticData.TimeRecord[i] = TimeText.text;
                if (StaticData.TimeRecord[i] .CompareTo(MinTime)<0)
                {
                    //打破纪录效果
                     GameObject.Find("newrecord").SetActive(true);
                }
            }
            else if(StaticData.TimeRecord[i].CompareTo(MinTime)<0) {
                //记录最少时间圈数
                MinTime = StaticData.TimeRecord[i];
            }
        }
        //显示结算界面，显示三圈花费的时间
        GameObject.Find("BillingPanel").SetActive(true);
        Cir1Time.text = cirTime[0];
        Cir2Time.text = cirTime[1];
        Cir3Time.text = cirTime[2];
        //隐藏其余面板
        GameObject.Find("CoinPanel").SetActive(false);
        GameObject.Find("CirPanel").SetActive(false);
        GameObject.Find("ToolsPanel").SetActive(false);
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
    }
}
