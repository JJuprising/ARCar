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
            for (int i = 0; i < 4; i++)
            {
                Debug.Log(i + ":" + StaticData.GateObserved[i]);
                count += StaticData.GateObserved[i];
            }
            if (count == 4)
            {
                StaticData.isObservedFinshed = true;//识别好了4个，标记为true
                
            }

        }
        if (isShowMlSec)
        {
            // 计时时间
            CountTime += Time.deltaTime;
            hour = (int)CountTime / 3600;
            min = (int)(CountTime - hour * 3600) / 60;
            sec = (int)(CountTime - hour * 3600 - min * 60);
            msecStr = isShowMlSec ? ("." + ((int)((CountTime - (int)CountTime) * 10)).ToString("D1")) : "";
            TimeText.text = hour.ToString("D2") + ":" + min.ToString("D2") + ":" + sec.ToString("D2") + msecStr;
        }

        if(StaticData.GateObserved[3] == 4)
        {
            //判断是否识别到了4次
            //第四个门已经识别，判断是否四个门都是别好的
            int count = 0;
            for (int i = 0; i < 4; i++)
            {
                Debug.Log(i + ":" + StaticData.GateObserved[i]);
                count += StaticData.GateObserved[i];
            }
            if (count == 12)
            {
                //游戏结束，生成结算面板

            }
        }
    }
   
  
    public void startCountTime()
    {
        
        
        //if (StaticData.isObservedFinshed)
        //{
            isShowMlSec = true;//开启计时
            
        //}
    }
}
