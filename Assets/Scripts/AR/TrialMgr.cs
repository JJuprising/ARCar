using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TrialMgr : Singleton<TrialMgr>
{
    public Text TimeText;//��ʱUI
    private float CountTime;//��ʱ
    private int hour,min,sec;
    private string msecStr;
    private bool isShowMlSec=false;//��ʼ��Ϊ0

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
            //���ĸ����Ѿ�ʶ���ж��Ƿ��ĸ��Ŷ��Ǳ�õ�
            int count = 0;
            for (int i = 0; i < 4; i++)
            {
                Debug.Log(i + ":" + StaticData.GateObserved[i]);
                count += StaticData.GateObserved[i];
            }
            if (count == 4)
            {
                StaticData.isObservedFinshed = true;//ʶ�����4�������Ϊtrue
                
            }

        }
        if (isShowMlSec)
        {
            // ��ʱʱ��
            CountTime += Time.deltaTime;
            hour = (int)CountTime / 3600;
            min = (int)(CountTime - hour * 3600) / 60;
            sec = (int)(CountTime - hour * 3600 - min * 60);
            msecStr = isShowMlSec ? ("." + ((int)((CountTime - (int)CountTime) * 10)).ToString("D1")) : "";
            TimeText.text = hour.ToString("D2") + ":" + min.ToString("D2") + ":" + sec.ToString("D2") + msecStr;
        }

        if(StaticData.GateObserved[3] == 4)
        {
            //�ж��Ƿ�ʶ����4��
            //���ĸ����Ѿ�ʶ���ж��Ƿ��ĸ��Ŷ��Ǳ�õ�
            int count = 0;
            for (int i = 0; i < 4; i++)
            {
                Debug.Log(i + ":" + StaticData.GateObserved[i]);
                count += StaticData.GateObserved[i];
            }
            if (count == 12)
            {
                //��Ϸ���������ɽ������

            }
        }
    }
   
  
    public void startCountTime()
    {
        
        
        //if (StaticData.isObservedFinshed)
        //{
            isShowMlSec = true;//������ʱ
            
        //}
    }
}
