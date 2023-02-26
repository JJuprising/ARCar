using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TrialMgr : Singleton<TrialMgr>
{
    public Text TimeText;//��ʱUI
    public Text Cir1Time;//��һȦ
    public Text Cir2Time;//�ڶ�Ȧ
    public Text Cir3Time;//����Ȧ
    private float CountTime;//��ʱ
    private int hour,min,sec;
    private string msecStr;
    private bool isShowMlSec=false;//��ʼ��Ϊ0
    private string []cirTime=new string[3];//��¼ÿһȦʱ����ַ���
    private float CountTime2 = 0;//��Ȧ��ʱ��
    private int hour2, min2, sec2;
    private string mesecStr2;
    private bool setZero;//��¼��ʱ��������ʽ�ظ���0
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
                //Debug.Log(i + ":" + StaticData.GateObserved[i]);
                count += StaticData.GateObserved[i];
            }
            if (count == 4)
            {
                StaticData.isObservedFinshed = true;//ʶ�����4�������Ϊtrue
                
            }

        }
        if (isShowMlSec&& StaticData.EndTimeTrial==false)//������ʱ��Ȧ��û����
        {
            // ��ʱ��ʱ��
            CountTime += Time.deltaTime;
            hour = (int)CountTime / 3600;
            min = (int)(CountTime - hour * 3600) / 60;
            sec = (int)(CountTime - hour * 3600 - min * 60);
            msecStr = isShowMlSec ? ("." + ((int)((CountTime - (int)CountTime) * 10)).ToString("D1")) : "";
            TimeText.text =  min.ToString("D2") + ":" + sec.ToString("D2") + msecStr;
        }
        if(StaticData.EndTimeTrial == false)
        {
            //��¼ÿȦ��ʱ
            switch (StaticData.GateObserved[0])
            {
                case 3:
                    //��һȦ��ɣ���¼ʱ��
                    if (setZero == false)
                    {
                        cirTime[0] = TimeText.text;
                        setZero = true;
                    }
                    
                    //��¼�ڶ�Ȧ��ʱ
                    CountTime2 += Time.deltaTime;
                    print("��һȦ��ɣ���¼ʱ��:"+ cirTime[0]);
                    break;
                case 4:
                    //�ڶ�Ȧ��ɣ���¼ʱ��
                    hour2 = (int)CountTime2 / 3600;
                    min2 = (int)(CountTime2 - hour2 * 3600) / 60;
                    sec2 = (int)(CountTime2 - hour2 * 3600 - min * 60);
                    msecStr = "." + ((int)((CountTime2 - (int)CountTime2) * 10)).ToString("D1");

                    if (setZero)
                    {
                        CountTime2 = 0;//��0���¼�ʱ
                        cirTime[1] = min2.ToString("D2") + ":" + sec2.ToString("D2") + msecStr;//�ڶ�Ȧ��ʱ
                        setZero =false;
                    }
                    CountTime2 += Time.deltaTime;
                    print("�ڶ�Ȧ��ɣ���¼ʱ��:"+ cirTime[1]);
                    break;
                case 5:
                    hour2 = (int)CountTime2 / 3600;
                    min2 = (int)(CountTime2 - hour2 * 3600) / 60;
                    sec2 = (int)(CountTime2 - hour2 * 3600 - min * 60);
                    msecStr = "." + ((int)((CountTime2 - (int)CountTime2) * 10)).ToString("D1");
                    cirTime[2] = min2.ToString("D2") + ":" + sec2.ToString("D2") + msecStr;//�ڶ�Ȧ��ʱ

                                                                                           // ����Ȧ�������ĸ��ţ������һ�����µ�Finish����
                    GameObject FinishObject;
                    FinishObject = GameObject.Find("finish");
                    FinishObject.SetActive(true);
                    StaticData.EndTimeTrial = true;//��Ϸ�������
                    print("����Ȧ��ɣ���¼ʱ��:"+ cirTime[2]);
                    break;

            }
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
        string MinTime = StaticData.TimeRecord[0];
        for(int i = 0; i < StaticData.TimeRecord.Length; i++)
        {
            if (StaticData.TimeRecord[i] == "")
            {
                //Ϊ0ʱ��¼,תΪ��
                StaticData.TimeRecord[i] = TimeText.text;
                if (StaticData.TimeRecord[i] .CompareTo(MinTime)<0)
                {
                    //���Ƽ�¼Ч��
                     GameObject.Find("newrecord").SetActive(true);
                }
            }
            else if(StaticData.TimeRecord[i].CompareTo(MinTime)<0) {
                //��¼����ʱ��Ȧ��
                MinTime = StaticData.TimeRecord[i];
            }
        }
        //��ʾ������棬��ʾ��Ȧ���ѵ�ʱ��
        GameObject.Find("BillingPanel").SetActive(true);
        Cir1Time.text = cirTime[0];
        Cir2Time.text = cirTime[1];
        Cir3Time.text = cirTime[2];
        //�����������
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
