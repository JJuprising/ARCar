using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TrialMgr : MonoBehaviour
{
    public Text TimeText;//��ʱUI
    private float CountTime;//��ʱ
    private int hour,min,sec;
    private string msecStr;
    private bool isShowMlSec=false;//��ʼ��Ϊ0
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (StaticData.GateObserved[3] == 1)
        {
            //���ĸ����Ѿ�ʶ�𣬽����ʱ����
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
        //ͳ��Ϊ4˵���ĸ��Ŷ�ʶ����һ�飬��Ϸ���Կ�ʼ
        if (count == 4)
        {
            isShowMlSec = true;
            //��ʱʱ��
            CountTime += Time.deltaTime;
            hour = (int)CountTime / 3600;
            min = (int)(CountTime - hour * 3600) / 60;
            sec = (int)(CountTime - hour * 3600 - min * 60);
            msecStr = isShowMlSec ? ("." + ((int)((CountTime - (int)CountTime) * 10)).ToString("D1")) : "";
            TimeText.text = hour.ToString("D2") + ":" + min.ToString("D2") + ":" + sec.ToString("D2") + msecStr;
        }
    }
}
