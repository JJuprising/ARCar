using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TrialMgr : Singleton<TrialMgr>
{
    public Text TimeText;//计时UI
    public Text Cir1Time;//第一圈的时间
    public Text Cir2Time;//第二圈的时间
    public Text Cir3Time;//第三圈的时间
    public Text totalTime;//总时间
    public Text CirText;//圈数文本
    private int CountCir;//圈数
    private float CountTime;//计时
    private int hour, min, sec;
    private string msecStr;
    private bool isShowMlSec = false;//初始化为0
    private string[] cirTime = new string[3];//记录每一圈时间的字符串
    private float CountTime2 = 0;//单圈计时器
    private int hour2, min2, sec2;
    private string mesecStr2;
    private bool setZero;//记录计时器清理，防止重复清0
    private bool endsign = true;//标记结束函数进入一次
    private float m_timer = 0;//结束后计时器
    public GameObject gate1 = null;
    public GameObject gate2 = null;
    public GameObject gate3 = null;
    public GameObject gate4 = null;
    public GameObject CarOwn;//摄像机前的车
    //音效
    public AudioClip CountDownSound;  //3秒倒计时
    public AudioClip WinSound;//结束游戏音效
    private AudioSource source;   //必须定义AudioSource才能调用AudioClip
    private AudioSource CarSounce;//车的引擎声

    // Start is called before the first frame update
    void Start()
    {

        CarSounce = CarOwn.GetComponent<AudioSource>();
        source = GetComponent<AudioSource>();  //将this Object 上面的Component赋值给定义的AudioSource


    }

    // Update is called once per frame
    void Update()
    {
        //检测选择的游戏模式 1是竞速赛 2是计时赛

        if (StaticData.racemode == 1)
        {
            //竞速赛
        }
        else if (StaticData.racemode == 2)
        {
            //计时赛
            TimeRace();
            StaticData.racemode *= 4;//映射到4倍的值，避免重复进入计时
        }
        //GetThing.cs碰到门前的检测体，圈数加一
        //给圈数赋值 
        CountCir = int.Parse(CirText.text);

        if (StaticData.GateObserved[3] == 1)
        {
            //第四个门已经识别，判断是否四个门都是别好的
            int count = 0;
            for (int i = 0; i < StaticData.GateObserved.Length; i++)
            {
                //Debug.Log(i + ":" + StaticData.GateObserved[i]);
                count += StaticData.GateObserved[i];
            }
            if (count >= 4)
            {
                StaticData.isObservedFinshed = true;//识别好了4个，标记为true

            }

        }

        //记总时间
        if (isShowMlSec && StaticData.EndTimeTrial == false)//触发计时且圈还没跑完
        {
            // 计时总时间
            CountTime += Time.deltaTime;
            hour = (int)CountTime / 3600;
            min = (int)(CountTime - hour * 3600) / 60;
            sec = (int)(CountTime - hour * 3600 - min * 60);
            msecStr = isShowMlSec ? ("." + ((int)((CountTime - (int)CountTime) * 10)).ToString("D1")) : "";
            TimeText.text = min.ToString("D2") + ":" + sec.ToString("D2") + msecStr;
            
        }
        //游戏过程
        if (StaticData.EndTimeTrial == false)
        {
            //记录每圈用时
            switch (CountCir)
            {
                case 2:
                    //第一圈完成，记录时间
                    if (setZero == false)
                    {
                        cirTime[0] = TimeText.text;
                        setZero = true;
                    }

                    //记录第二圈用时
                    CountTime2 += Time.deltaTime;
                    //print("第一圈完成，记录时间:"+ cirTime[0]);
                    break;
                case 3:
                    //第二圈完成，记录时间
                    hour2 = (int)CountTime2 / 3600;
                    min2 = (int)(CountTime2 - hour2 * 3600) / 60;
                    sec2 = (int)(CountTime2 - hour2 * 3600 - min * 60);
                    msecStr = "." + ((int)((CountTime2 - (int)CountTime2) * 10)).ToString("D1");

                    if (setZero)
                    {
                        CountTime2 = 0;//清0重新计时
                        cirTime[1] = min2.ToString("D2") + ":" + sec2.ToString("D2") + msecStr;//第二圈用时
                        setZero = false;
                    }
                    CountTime2 += Time.deltaTime;
                    //print("第二圈完成，记录时间:"+ cirTime[1]);
                    ////三圈之后生成checker 
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
                    cirTime[2] = min2.ToString("D2") + ":" + sec2.ToString("D2") + msecStr;//第二圈用时


                    totalTime.text = TimeText.text;//记录总时间
                    //StaticData.EndTimeTrial = true;//游戏结束标记
                    //print("第三圈完成，记录时间:"+ cirTime[2]);
                    break;

            }
        }

        //结束游戏
        if (StaticData.EndTimeTrial && endsign)
        {
            source.PlayOneShot(WinSound, 1F);   //播放结束游戏声音
            //GetThing.cs检测到Finish被撞击，说明已经经过终点，进入结算界面
            if (m_timer == 0)
            {
                //Finish文字
                GameObject Canvas = GameObject.Find("Canvas");
                GameObject finishimg = Canvas.transform.Find("finishimg").gameObject;
                finishimg.SetActive(true);
                //结束特效
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
                //两秒之后进入结束函数
                endsign = false;
                EndTrial();
            }
        }
    }

    //计时赛起始函数
    public void TimeRace()
    {

        //倒计时
        StartCountTime();
        //计时赛游戏逻辑
    }
    //开始计时函数
    public void StartCountTime()
    {
        source.PlayOneShot(CountDownSound, 1F);   //播放声音
        //如果选择计时赛，开始计时，三秒后游戏开始
        //显示游戏面板
        GameObject Canvas = GameObject.Find("Canvas");
        Canvas.transform.Find("ToolsPanel").gameObject.SetActive(true);
        Canvas.transform.Find("CoinPanel").gameObject.SetActive(true);
        Canvas.transform.Find("CirPanel").gameObject.SetActive(true);
        Canvas.transform.Find("CountPanel").gameObject.SetActive(true);
        Canvas.transform.Find("CountDownPanel").gameObject.SetActive(true);
        StartCoroutine(DelaythreeSec());

        
    }
    //计时函数
    //显示倒计时三秒动画
    private IEnumerator DelaythreeSec()
    {
        
        Vector3 NumberVec = this.transform.position;
        NumberVec.z += 2.5f;//图标相对摄像机位置
        bool isMerge = true;//判断是否已经生成了数字
        float time = 0;//计时
        //隐藏的不能用gameobject.find,需要挂在可见下面，用tranform.find
        GameObject canvas = GameObject.Find("CountDownPanel");
        GameObject number1 = canvas.transform.Find("number1").gameObject;
        GameObject number2 = canvas.transform.Find("number2").gameObject;
        GameObject number3 = canvas.transform.Find("number3").gameObject;
        GameObject GO = canvas.transform.Find("GO").gameObject;
        //AIControl.Instance.SetCarActive();//显示小车
        while (time < 3)
        {
            time += Time.deltaTime;

            //生成计时图标
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
                //Destroy(Number3Object);//销毁物体
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
        isShowMlSec = true;//置为true开始计时
        //AIControl.Instance.StartMove(); //对手开始移动                  
        //车引擎音效
        CarSounce.Play();
    }
    //比赛结束函数
    public void EndTrial()
    {
        //比赛结束函数
        //游戏结束finish

        //打破纪录效果
        GameObject Camera;
        GameObject Canvas = GameObject.Find("Canvas");
        //将用时数据进行存储，如果记录的数组不为空，则比较是否打破记录
        string MinTime = StaticData.TimeRecord[0];
        for (int i = 0; i < StaticData.TimeRecord.Length; i++)
        {

            if (StaticData.TimeRecord[i] == null)
            {
                //为0时记录,转为秒
                StaticData.TimeRecord[i] = totalTime.text;

                if (StaticData.TimeRecord[i].CompareTo(MinTime) < 0)
                {

                    Camera = GameObject.Find("AR Camera");
                    //隐藏的不能用gameobject.find,需要挂在可见下面，用tranform.find
                    GameObject newrecord = Camera.transform.Find("newrecord").gameObject;
                    newrecord.SetActive(true);
                    Destroy(newrecord, 2);
                }
                break;
            }
            else if (StaticData.TimeRecord[i].CompareTo(MinTime) < 0 && i != 0)
            {
                //记录最少时间圈数
                MinTime = StaticData.TimeRecord[i];
            }
        }
        //显示结算界面，显示三圈花费的时间
        //隐藏的不能用gameobject.find,需要挂在可见下面，用tranform.find
        Canvas.transform.Find("BillingPanel").gameObject.SetActive(true);
        for (int i = 0; i < cirTime.Length; i++)
        {
            Debug.Log("第" + i + "圈:" + cirTime[i]);
        }
        Cir1Time.text = cirTime[0];
        Cir2Time.text = cirTime[1];
        Cir3Time.text = cirTime[2];
        //隐藏其余面板
        Canvas.transform.Find("CoinPanel").gameObject.SetActive(false);
        Canvas.transform.Find("CirPanel").gameObject.SetActive(false);
        Canvas.transform.Find("ToolsPanel").gameObject.SetActive(false);
        Canvas.transform.Find("CountPanel").gameObject.SetActive(false);
    }

    //重置游戏
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

    cirTime = new string[3];//记录每一圈时间的字符串
    CountTime2 = 0;//单圈计时器
    hour2= min2= sec2=0;


    setZero = false;//记录计时器清理，防止重复清0
    endsign = true;//标记结束函数进入一次
    m_timer = 0;//结束后计时器


        //重置面板
        GameObject Canvas = GameObject.Find("Canvas");
        //显示其余面板
        Canvas.transform.Find("CoinPanel").gameObject.SetActive(true);//金币栏
        Canvas.transform.Find("CirPanel").gameObject.SetActive(true);//圈数栏
        Canvas.transform.Find("ToolsPanel").gameObject.SetActive(true);//道具栏
        Canvas.transform.Find("CountTime").gameObject.SetActive(true);//计时栏
        Canvas.transform.Find("BillingPanel").gameObject.SetActive(false);//隐藏结算栏
    }


    //按钮事件
    public void Btn_GoToLanuch()
    {
        StaticData.SaveDataToFile();//保存游戏
        SceneManager.LoadScene("UI");
        StaticData.RestGame();
        SceneManager.UnloadSceneAsync("AR_Scene");
    }
    public void Btn_Reset()
    {
        StaticData.SaveDataToFile();//保存游戏
        ResetGame();
        GetThings.Instance.ResetGame();
        StaticData.RestGame();
    }
    public void Btn_UseItem()
    {
        //按钮触发道具，传递参数
        StaticData.UseItemSign = true;
    }
    //race menu选择游戏模式
    int Temracemode = 0;
    public void Btn_ChooseRace(int target)
    {
        if (target == 1)
        {
            Temracemode = 1;//竞速模式
        }
        else
        {
            Temracemode = 2;//计时模式
        }

    }
    public void Btn_OK()
    {
        GameObject.Find("RaceMenuPanel").SetActive(false);
        StaticData.racemode = Temracemode;
    }
}
