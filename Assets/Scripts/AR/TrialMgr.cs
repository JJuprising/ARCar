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
    private int hour, min, sec;
    private string msecStr;
    private bool isShowMlSec = false;//��ʼ��Ϊ0
    private string[] cirTime = new string[3];//��¼ÿһȦʱ����ַ���
    private float CountTime2 = 0;//��Ȧ��ʱ��
    private int hour2, min2, sec2;
    private string mesecStr2;
    private bool setZero;//��¼��ʱ��������ֹ�ظ���0
    private bool endsign = true;//��ǽ�����������һ��
    private float m_timer = 0;//�������ʱ��
    public GameObject gate1 = null;
    public GameObject gate2 = null;
    public GameObject gate3 = null;
    public GameObject gate4 = null;
    public GameObject CarOwn;//�����ǰ�ĳ�
    //��Ч
    public AudioClip CountDownSound;  //3�뵹��ʱ
    public AudioClip WinSound;//������Ϸ��Ч
    private AudioSource source;   //���붨��AudioSource���ܵ���AudioClip
    private AudioSource CarSounce;//����������

    // Start is called before the first frame update
    void Start()
    {

        CarSounce = CarOwn.GetComponent<AudioSource>();
        source = GetComponent<AudioSource>();  //��this Object �����Component��ֵ�������AudioSource


    }

    // Update is called once per frame
    void Update()
    {
        //���ѡ�����Ϸģʽ 1�Ǿ����� 2�Ǽ�ʱ��

        if (StaticData.racemode == 1)
        {
            //������
        }
        else if (StaticData.racemode == 2)
        {
            //��ʱ��
            TimeRace();
            StaticData.racemode *= 4;//ӳ�䵽4����ֵ�������ظ������ʱ
        }
        //GetThing.cs������ǰ�ļ���壬Ȧ����һ
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
            if (count >= 4)
            {
                StaticData.isObservedFinshed = true;//ʶ�����4�������Ϊtrue

            }

        }

        //����ʱ��
        if (isShowMlSec && StaticData.EndTimeTrial == false)//������ʱ��Ȧ��û����
        {
            // ��ʱ��ʱ��
            CountTime += Time.deltaTime;
            hour = (int)CountTime / 3600;
            min = (int)(CountTime - hour * 3600) / 60;
            sec = (int)(CountTime - hour * 3600 - min * 60);
            msecStr = isShowMlSec ? ("." + ((int)((CountTime - (int)CountTime) * 10)).ToString("D1")) : "";
            TimeText.text = min.ToString("D2") + ":" + sec.ToString("D2") + msecStr;
            
        }
        //��Ϸ����
        if (StaticData.EndTimeTrial == false)
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
                    //print("��һȦ��ɣ���¼ʱ��:"+ cirTime[0]);
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
                        setZero = false;
                    }
                    CountTime2 += Time.deltaTime;
                    //print("�ڶ�Ȧ��ɣ���¼ʱ��:"+ cirTime[1]);
                    ////��Ȧ֮������checker 
                    //GameObject checker = Instantiate(Resources.Load("checker", typeof(GameObject)), transform) as GameObject;
                    //Vector3 GateVec = gate1.transform.position;
                    //Vector3 CheckerPos = GateVec - Vector3.up * 0.95f;
                    //checker.transform.parent = gate1.transform;
                    //checker.transform.position = CheckerPos;
                    break;
                case 4:
                    hour2 = (int)CountTime2 / 3600;
                    min2 = (int)(CountTime2 - hour2 * 3600) / 60;
                    sec2 = (int)(CountTime2 - hour2 * 3600 - min * 60);
                    msecStr = "." + ((int)((CountTime2 - (int)CountTime2) * 10)).ToString("D1");
                    cirTime[2] = min2.ToString("D2") + ":" + sec2.ToString("D2") + msecStr;//�ڶ�Ȧ��ʱ


                    totalTime.text = TimeText.text;//��¼��ʱ��
                    //StaticData.EndTimeTrial = true;//��Ϸ�������
                    //print("����Ȧ��ɣ���¼ʱ��:"+ cirTime[2]);
                    break;

            }
        }

        //������Ϸ
        if (StaticData.EndTimeTrial && endsign)
        {
            source.PlayOneShot(WinSound, 1F);   //���Ž�����Ϸ����
            //GetThing.cs��⵽Finish��ײ����˵���Ѿ������յ㣬����������
            if (m_timer == 0)
            {
                //Finish����
                GameObject Canvas = GameObject.Find("Canvas");
                GameObject finishimg = Canvas.transform.Find("finishimg").gameObject;
                finishimg.SetActive(true);
                //������Ч
                GameObject Camera = GameObject.Find("AR Camera");
                GameObject PickupEffect = Instantiate(Resources.Load("finishlight", typeof(GameObject))) as GameObject;
                Vector3 pos = Camera.transform.position + Camera.transform.forward * 1.28f;
                PickupEffect.transform.position = pos;
                Destroy(finishimg, 2);
            }
            m_timer += Time.deltaTime;
            if (m_timer >= 2)
            {
                print(m_timer);
                //����֮������������
                endsign = false;
                EndTrial();
            }
        }
    }

    //��ʱ����ʼ����
    public void TimeRace()
    {

        //����ʱ
        StartCountTime();
        //��ʱ����Ϸ�߼�
    }
    //��ʼ��ʱ����
    public void StartCountTime()
    {
        source.PlayOneShot(CountDownSound, 1F);   //��������
        //���ѡ���ʱ������ʼ��ʱ���������Ϸ��ʼ
        //��ʾ��Ϸ���
        GameObject Canvas = GameObject.Find("Canvas");
        Canvas.transform.Find("ToolsPanel").gameObject.SetActive(true);
        Canvas.transform.Find("CoinPanel").gameObject.SetActive(true);
        Canvas.transform.Find("CirPanel").gameObject.SetActive(true);
        Canvas.transform.Find("CountPanel").gameObject.SetActive(true);
        Canvas.transform.Find("CountDownPanel").gameObject.SetActive(true);
        StartCoroutine(DelaythreeSec());

        
    }
    //��ʱ����
    //��ʾ����ʱ���붯��
    private IEnumerator DelaythreeSec()
    {
        
        Vector3 NumberVec = this.transform.position;
        NumberVec.z += 2.5f;//ͼ����������λ��
        bool isMerge = true;//�ж��Ƿ��Ѿ�����������
        float time = 0;//��ʱ
        //���صĲ�����gameobject.find,��Ҫ���ڿɼ����棬��tranform.find
        GameObject canvas = GameObject.Find("CountDownPanel");
        GameObject number1 = canvas.transform.Find("number1").gameObject;
        GameObject number2 = canvas.transform.Find("number2").gameObject;
        GameObject number3 = canvas.transform.Find("number3").gameObject;
        GameObject GO = canvas.transform.Find("GO").gameObject;
        //AIControl.Instance.SetCarActive();//��ʾС��
        while (time < 3)
        {
            time += Time.deltaTime;

            //���ɼ�ʱͼ��
            if (time <= 1 && time >= 0 && isMerge)
            {
                //3

                isMerge = false;
                //Number3Object = Instantiate(Resources.Load("number3", typeof(GameObject))) as GameObject;
                //Number3Object.transform.position = NumberVec;
                //Number3Object.transform.parent = this.transform;

                number3.SetActive(true);
            }

            if (time > 1 && time <= 2 && isMerge == false)
            {
                //2

                isMerge = true;
                //Number2Object = Instantiate(Resources.Load("number2", typeof(GameObject))) as GameObject;
                //NumberVec.z += 0.01f;
                //Number2Object.transform.position = Number3Object.transform.position;
                //Number2Object.transform.parent = this.transform;
                //Destroy(Number3Object);//��������
                number3.SetActive(false);
                number2.SetActive(true);
            }
            if (time > 2 && time < 3 && isMerge == true)
            {
                //1

                isMerge = false;

                //Number1Object = Instantiate(Resources.Load("number1", typeof(GameObject))) as GameObject;
                //NumberVec.z -= 0.01f;
                //Number1Object.transform.position = Number2Object.transform.position;
                //Number1Object.transform.parent = this.transform;
                //Destroy(Number2Object);
                number3.SetActive(false);
                number2.SetActive(false);
                number1.SetActive(true);
            }
            yield return null;
        }
        //GoObject = Instantiate(Resources.Load("go_2", typeof(GameObject))) as GameObject;
        //NumberVec.z += 0.01f;
        //GoObject.transform.position = Number1Object.transform.position;
        //GoObject.transform.parent = this.transform;
        number1.SetActive(false);
        GO.SetActive(true);
        Destroy(GO, 0.3f);
        isShowMlSec = true;//��Ϊtrue��ʼ��ʱ
        //AIControl.Instance.StartMove(); //���ֿ�ʼ�ƶ�                  
        //��������Ч
        CarSounce.Play();
    }
    //������������
    public void EndTrial()
    {
        //������������
        //��Ϸ����finish

        //���Ƽ�¼Ч��
        GameObject Camera;
        GameObject Canvas = GameObject.Find("Canvas");
        //����ʱ���ݽ��д洢�������¼�����鲻Ϊ�գ���Ƚ��Ƿ���Ƽ�¼
        string MinTime = StaticData.TimeRecord[0];
        for (int i = 0; i < StaticData.TimeRecord.Length; i++)
        {

            if (StaticData.TimeRecord[i] == null)
            {
                //Ϊ0ʱ��¼,תΪ��
                StaticData.TimeRecord[i] = totalTime.text;

                if (StaticData.TimeRecord[i].CompareTo(MinTime) < 0)
                {

                    Camera = GameObject.Find("AR Camera");
                    //���صĲ�����gameobject.find,��Ҫ���ڿɼ����棬��tranform.find
                    GameObject newrecord = Camera.transform.Find("newrecord").gameObject;
                    newrecord.SetActive(true);
                    Destroy(newrecord, 2);
                }
                break;
            }
            else if (StaticData.TimeRecord[i].CompareTo(MinTime) < 0 && i != 0)
            {
                //��¼����ʱ��Ȧ��
                MinTime = StaticData.TimeRecord[i];
            }
        }
        //��ʾ������棬��ʾ��Ȧ���ѵ�ʱ��
        //���صĲ�����gameobject.find,��Ҫ���ڿɼ����棬��tranform.find
        Canvas.transform.Find("BillingPanel").gameObject.SetActive(true);
        for (int i = 0; i < cirTime.Length; i++)
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
        Canvas.transform.Find("CountPanel").gameObject.SetActive(false);
    }

    //������Ϸ
    public void ResetGame()
    {
        CountCir = 0;
        CountTime = 0;
        hour = min = sec = 0;
        CirText.text = "0";
        totalTime.text = "0";
        TimeText.text = "0:00.0";
        msecStr = "";
        isShowMlSec = false;

    cirTime = new string[3];//��¼ÿһȦʱ����ַ���
    CountTime2 = 0;//��Ȧ��ʱ��
    hour2= min2= sec2=0;


    setZero = false;//��¼��ʱ��������ֹ�ظ���0
    endsign = true;//��ǽ�����������һ��
    m_timer = 0;//�������ʱ��


        //�������
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
        StaticData.SaveDataToFile();//������Ϸ
        SceneManager.LoadScene("UI");
        StaticData.RestGame();
        SceneManager.UnloadSceneAsync("AR_Scene");
    }
    public void Btn_Reset()
    {
        StaticData.SaveDataToFile();//������Ϸ
        ResetGame();
        GetThings.Instance.ResetGame();
        StaticData.RestGame();
    }
    public void Btn_UseItem()
    {
        //��ť�������ߣ����ݲ���
        StaticData.UseItemSign = true;
    }
    //race menuѡ����Ϸģʽ
    int Temracemode = 0;
    public void Btn_ChooseRace(int target)
    {
        if (target == 1)
        {
            Temracemode = 1;//����ģʽ
        }
        else
        {
            Temracemode = 2;//��ʱģʽ
        }

    }
    public void Btn_OK()
    {
        GameObject.Find("RaceMenuPanel").SetActive(false);
        StaticData.racemode = Temracemode;
    }
}
