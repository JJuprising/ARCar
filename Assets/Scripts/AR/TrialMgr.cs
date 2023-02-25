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
            for (int i = 0; i < StaticData.GateObserved.Length; i++)
            {
                Debug.Log(i + ":" + StaticData.GateObserved[i]);
                count += StaticData.GateObserved[i];
            }
            if (count == 4)
            {
                StaticData.isObservedFinshed = true;//ʶ�����4�������Ϊtrue
                
            }

        }
        if (isShowMlSec&& StaticData.EndTimeTrial==false)//������ʱ��Ȧ��û����
        {
            // ��ʱʱ��
            CountTime += Time.deltaTime;
            hour = (int)CountTime / 3600;
            min = (int)(CountTime - hour * 3600) / 60;
            sec = (int)(CountTime - hour * 3600 - min * 60);
            msecStr = isShowMlSec ? ("." + ((int)((CountTime - (int)CountTime) * 10)).ToString("D1")) : "";
            TimeText.text =  min.ToString("D2") + ":" + sec.ToString("D2") + msecStr;
        }
        if(StaticData.GateObserved[3] == 4)
        {
            //����Ȧ�������ĸ��ţ������һ�����µ�Finish����
            GameObject FinishObject;
            FinishObject = GameObject.Find("Gate1_ImageTarget").transform.GetChild(0).gameObject;
            FinishObject.SetActive(true);
        }
        if (StaticData.EndTimeTrial)
        {
            //GetThing.cs��⵽Finish��ײ����˵���Ѿ������յ㣬����������
            endTrial();
        }
    }
   
  
    public void startCountTime()
    {

        //if (StaticData.isObservedFinshed)
        //{
            isShowMlSec = true;//������ʱ
        //}
    }
    public void endTrial()
    {
        //������������
        
        //����ʱ���ݽ��д洢�������¼�����鲻Ϊ�գ���Ƚ��Ƿ���Ƽ�¼
        int MinTime = StaticData.TimeRecord[0];
        for(int i = 0; i < StaticData.TimeRecord.Length; i++)
        {
            if (StaticData.TimeRecord[i] == 0)
            {
                //Ϊ0ʱ��¼,תΪ��
                StaticData.TimeRecord[i] = min * 60 + sec;
                if (StaticData.TimeRecord[i] < MinTime)
                {
                    //���Ƽ�¼����
                    GameObject BreakRecordObject;
                    BreakRecordObject = GameObject.Find("ARCamera").transform.GetChild(1).gameObject;
                    BreakRecordObject.SetActive(true);
                }
            }
            else if(StaticData.TimeRecord[i]<MinTime) {
                MinTime = StaticData.TimeRecord[i];
            }
        }
        //��ʾ������棬��ʾ��Ȧ���ѵ�ʱ��

    }
}
