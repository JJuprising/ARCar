using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TrialMgr : Singleton<TrialMgr>
{
    public Text TimeText;//计时UI
    private float CountTime;//计时
    private int hour,min,sec;
    private string msecStr;
    private bool isShowMlSec=false;//初始化为0

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
                Debug.Log(i + ":" + StaticData.GateObserved[i]);
                count += StaticData.GateObserved[i];
            }
            if (count == 4)
            {
                StaticData.isObservedFinshed = true;//识别好了4个，标记为true
                
            }

        }
        if (isShowMlSec&& StaticData.EndTimeTrial==false)//触发计时且圈还没跑完
        {
            // 计时时间
            CountTime += Time.deltaTime;
            hour = (int)CountTime / 3600;
            min = (int)(CountTime - hour * 3600) / 60;
            sec = (int)(CountTime - hour * 3600 - min * 60);
            msecStr = isShowMlSec ? ("." + ((int)((CountTime - (int)CountTime) * 10)).ToString("D1")) : "";
            TimeText.text =  min.ToString("D2") + ":" + sec.ToString("D2") + msecStr;
        }
        if(StaticData.GateObserved[3] == 4)
        {
            //第三圈经过第四个门，激活第一个门下的Finish物体
            GameObject FinishObject;
            FinishObject = GameObject.Find("Gate1_ImageTarget").transform.GetChild(0).gameObject;
            FinishObject.SetActive(true);
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
        int MinTime = StaticData.TimeRecord[0];
        for(int i = 0; i < StaticData.TimeRecord.Length; i++)
        {
            if (StaticData.TimeRecord[i] == 0)
            {
                //为0时记录,转为秒
                StaticData.TimeRecord[i] = min * 60 + sec;
                if (StaticData.TimeRecord[i] < MinTime)
                {
                    //打破记录反馈
                    GameObject BreakRecordObject;
                    BreakRecordObject = GameObject.Find("ARCamera").transform.GetChild(1).gameObject;
                    BreakRecordObject.SetActive(true);
                }
            }
            else if(StaticData.TimeRecord[i]<MinTime) {
                MinTime = StaticData.TimeRecord[i];
            }
        }
        //显示结算界面，显示三圈花费的时间

    }
}
