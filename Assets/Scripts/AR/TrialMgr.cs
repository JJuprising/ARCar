using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TrialMgr : Singleton<TrialMgr>
{
    public Text TimeText;//��ʱUI
    public Text Cir1Time;//��һȦ��ʱ��
    public Text Cir2Time;//�ڶ�Ȧ��ʱ��
    public Text Cir3Time;//����Ȧ��ʱ��
    public Text totalTime;//��ʱ��
    public Text CirText;//Ȧ���ı�
    private int CountCir;//Ȧ��
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
        //��Ȧ����ֵ
        CountCir = int.Parse(CirText.text);

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
            switch (CountCir)
            {
                case 2:
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
                case 3:
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
                case 4:
                    hour2 = (int)CountTime2 / 3600;
                    min2 = (int)(CountTime2 - hour2 * 3600) / 60;
                    sec2 = (int)(CountTime2 - hour2 * 3600 - min * 60);
                    msecStr = "." + ((int)((CountTime2 - (int)CountTime2) * 10)).ToString("D1");
                    cirTime[2] = min2.ToString("D2") + ":" + sec2.ToString("D2") + msecStr;//�ڶ�Ȧ��ʱ

                    // ����Ȧ�������ĸ��ţ������һ�����µ�Finish����
                    GameObject Camera;
                    Camera = GameObject.Find("ARCamera");
                    //���صĲ�����gameobject.find,��Ҫ���ڿɼ����棬��tranform.find
                    GameObject FinishObject = Camera.transform.Find("finish").gameObject;
                    FinishObject.SetActive(true);
                    Destroy(FinishObject, 1);
                    totalTime.text = TimeText.text;//��¼��ʱ��
                    StaticData.EndTimeTrial = true;//��Ϸ�������
                    print("����Ȧ��ɣ���¼ʱ��:"+ cirTime[2]);
                    break;

            }
        }
        
       
        if (StaticData.EndTimeTrial)
        {
            //GetThing.cs��⵽Finish��ײ����˵���Ѿ������յ㣬����������
            EndTrial();
        }
    }
   
  
    public void StartCountTime()
    {

        //if (StaticData.isObservedFinshed)
        //{
            isShowMlSec = true;//������ʱ
        //}
    }
    public void EndTrial()
    {
        //������������
        //���Ƽ�¼Ч��
        GameObject Camera;

        //����ʱ���ݽ��д洢�������¼�����鲻Ϊ�գ���Ƚ��Ƿ���Ƽ�¼
        string MinTime = StaticData.TimeRecord[0];
        for(int i = 0; i < StaticData.TimeRecord.Length; i++)
        {
            
            if (StaticData.TimeRecord[i] == null)
            {
                //Ϊ0ʱ��¼,תΪ��
                StaticData.TimeRecord[i] = totalTime.text;
                
                if (StaticData.TimeRecord[i] .CompareTo(MinTime)<0)
                {
                    
                    Camera = GameObject.Find("ARCamera");
                    //���صĲ�����gameobject.find,��Ҫ���ڿɼ����棬��tranform.find
                    GameObject newrecord=Camera.transform.Find("newrecord").gameObject;
                    newrecord.SetActive(true);
                    Destroy(newrecord, 2);
                }
                break;
            }else if(StaticData.TimeRecord[i].CompareTo(MinTime)<0&&i!=0) {
                //��¼����ʱ��Ȧ��
                MinTime = StaticData.TimeRecord[i];
            }
        }
        //��ʾ������棬��ʾ��Ȧ���ѵ�ʱ��
        GameObject Canvas;
        Canvas = GameObject.Find("Canvas");
        //���صĲ�����gameobject.find,��Ҫ���ڿɼ����棬��tranform.find
        Canvas.transform.Find("BillingPanel").gameObject.SetActive(true);
        for(int i = 0; i < cirTime.Length; i++)
        {
            Debug.Log("��" + i + "Ȧ:" + cirTime[i]);
        }
        Cir1Time.text = cirTime[0];
        Cir2Time.text = cirTime[1];
        Cir3Time.text = cirTime[2];
        //�����������
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

        //�������
        GameObject Camera = GameObject.Find("ARCamera");
        GameObject Canvas = GameObject.Find("Canvas");
        //��ʾ�������
        Canvas.transform.Find("CoinPanel").gameObject.SetActive(true);//�����
        Canvas.transform.Find("CirPanel").gameObject.SetActive(true);//Ȧ����
        Canvas.transform.Find("ToolsPanel").gameObject.SetActive(true);//������
        Canvas.transform.Find("CountTime").gameObject.SetActive(true);//��ʱ��
        Canvas.transform.Find("BillingPanel").gameObject.SetActive(false);//���ؽ�����
    }
    //��ť�¼�
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
        //��ť�������ߣ����ݲ���
        StaticData.UseItemSign = true;
    }
}
