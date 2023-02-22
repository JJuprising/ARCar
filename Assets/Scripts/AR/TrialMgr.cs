using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TrialMgr : MonoBehaviour
{
    public Text TimeText;//计时UI
    private float CountTime;//计时
    private int hour,min,sec;
    private string msecStr;
    private bool isShowMlSec=false;//初始化为0
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (StaticData.GateObserved[3] == 1)
        {
            //第四个门已经识别，进入计时函数
            startCountTime();
        }
        

    }
    public void startCountTime()
    {
        int count=0;
        for(int i = 0; i < 4; i++)
        {
            Debug.Log(i+":"+StaticData.GateObserved[i]);
            count += StaticData.GateObserved[i];
        }
        //统计为4说明四个门都识别了一遍，游戏可以开始
        if (count == 4)
        {
            isShowMlSec = true;
            //计时时间
            CountTime += Time.deltaTime;
            hour = (int)CountTime / 3600;
            min = (int)(CountTime - hour * 3600) / 60;
            sec = (int)(CountTime - hour * 3600 - min * 60);
            msecStr = isShowMlSec ? ("." + ((int)((CountTime - (int)CountTime) * 10)).ToString("D1")) : "";
            TimeText.text = hour.ToString("D2") + ":" + min.ToString("D2") + ":" + sec.ToString("D2") + msecStr;
        }
    }
}
