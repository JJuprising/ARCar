using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GetThings : Singleton<GetThings>
{
    public Text Tx_CoinNum;//�������
    public Text CirText;//��¼Ȧ��
    public int CountCir=0;//��¼��ǰȦ��
    public Reward currentItem;//Ŀǰ���еĵ���

    
    [SerializeField] private Transform ScrollImageParent;

    [SerializeField]private RewardItem[] rewards;

    //��Ч
    //��Ч
    public AudioClip getCoinSound;  //��ȡ��ҵ���Ч
    public AudioClip getBoxSound;  //��ȡ��ҵ���Ч
    public AudioClip boostSound;//������Ч
    public AudioClip rolltoolSound;//������������Ч
    private AudioSource source;   //���붨��AudioSource���ܵ���AudioClip

    private void Start()
    {
        source.GetComponent<AudioSource>();
    }
    private void Update()
    {
        //���������е���ͬʱ��⵽�۵��źţ�ʹ�õ���
        if (StaticData.UseItemSign&&StaticData.EogUseTool)
        {
            StaticData.UseItemSign = StaticData.EogUseTool=false;
            UseItem();
        }
        if(StaticData.UseItemSign==false&& StaticData.EogUseTool)
        {
            //������û�е��ߵ��Ǵ������۵�
            StaticData.EogUseTool = false;
        }
    }


    private void OnTriggerEnter(Collider other)
    {

        Vector3 PicupVec = other.transform.position;//ʰȡ�����λ��
        GameObject PickupEffect;//ʰȡЧ��
        GameObject Camera = GameObject.Find("AR Camera");
        switch (other.gameObject.name){
            case "StartPlace(Clone)":
                print("��ʼ");
                //����RaceMenu
                GameObject Canvas = GameObject.Find("Canvas");
                Canvas.transform.Find("RaceMenuPanel").gameObject.SetActive(true);
                Destroy(other.gameObject, 3f);//��������
                break;
            
            case "Gate1_ImageTarget":
                if (StaticData.CheckGatePassingValidity(0))
                {
                    StaticData.GateObserved[0]++;
                    //StaticData.printScore();
                }
                
                break;
            case "Gate2_ImageTarget":
                if (StaticData.CheckGatePassingValidity(1))
                {
                    StaticData.GateObserved[1]++;
                    //StaticData.printScore();
                }
                break;
            case "Gate3_ImageTarget":
                if (StaticData.CheckGatePassingValidity(2))
                {
                    StaticData.GateObserved[2]++;
                    //StaticData.printScore();
                }
                break;
            case "Gate4_ImageTarget":
                if (StaticData.CheckGatePassingValidity(3))
                {
                    StaticData.GateObserved[3]++;
                    //StaticData.printScore();
                }
                break;
        }
        switch (other.gameObject.tag) {
            case "Check":
                //��¼Ȧ��
                CountCir++;//������ǰ�ļ���壬Ȧ����һ
                print("CountCir" + CountCir);
                CirText.text = CountCir.ToString();

                break;
            case "GoldBox":
                source.PlayClipAtPoint(getBoxSound, 1F);   //�񵽵�����Ч
                other.gameObject.SetActive(false);
                GetGoldenBoxReward();
                //Pick up effect
                PickupEffect = Instantiate(Resources.Load("PickupBox", typeof(GameObject))) as GameObject;
                Vector3 pos1 = Camera.transform.position + Camera.transform.forward * 2;
                PickupEffect.transform.position = pos1;
                
                Destroy(other.gameObject);//��������
                break;
            case "Coin":
                source.PlayOneShot(getCoinSound, 1F);   //�Խ����Ч
                Destroy(other.gameObject);
                Debug.Log("�񵽽��");
                //Pick up effect
                PickupEffect = Instantiate(Resources.Load("PickupCoin", typeof(GameObject))) as GameObject;
                PickupEffect.transform.position = PicupVec;
                StaticData.tempCoinNum++;
                StaticData.CoinNum++;//ȫ�ֵĽ������һ
                StaticData.totalCoinNum++;
                Tx_CoinNum.text = StaticData.tempCoinNum.ToString();//UI������ı��仯
                Destroy(other.gameObject);//��������
                break;
            case "Finish":
                print("ͨ��Finish����");
                Destroy(other.gameObject);
                if (CountCir == 4)
                {
                    StaticData.EndTimeTrial = true;//TimeTrial������ǣ�����TrialMgr�����������
                                                   //����finish����ʾ
                }

                //GameObject finishObject = Instantiate(Resources.Load("finishSign", typeof(GameObject))) as GameObject; ;
                //Vector3 pos2 = Camera.transform.position + Camera.transform.forward * 2;
                //finishObject.transform.position = pos2;
                //Destroy(finishObject, 5);
                //���Ƽ�¼����
                break;
            case "Gate":
                //�����ţ�������Ч
                
                //Pick up effect
                PickupEffect = Instantiate(Resources.Load("PickGate", typeof(GameObject))) as GameObject;
                Vector3 pos = Camera.transform.position+Camera.transform.forward*2;
                PickupEffect.transform.position = pos;
                break;
        }
        
    }
    

    Coroutine rolling;//roll���߶���Э��
    /// <summary>
    /// ���ó�ȡ��������Ʒ
    /// </summary>
    private void StopPreviousRolling()
    {
        currentItem = Reward.None;
        if (rolling != null)
        {
            StopCoroutine(rolling);
            for (int i = 0; i < ScrollImageParent.childCount; i++)
            {
                if (ScrollImageParent.GetChild(i).gameObject.name != "Panel")
                {
                    Destroy(ScrollImageParent.GetChild(i).gameObject);
                }
            }
        }
    }
    /// <summary>
    /// ��ȡ�������
    /// </summary>
    private void GetGoldenBoxReward()
    {
        source.PlayOneShot(rolltoolSound, 1F);   //���߹�����Ч
        RewardItem reward = rewards[Random.Range(0, rewards.Length)];
        rolling = StartCoroutine(RollingAward(reward));
    }
    /// <summary>
    /// roll���߶���
    /// </summary>
    /// <param name="reward">roll���ĵ���</param>
    IEnumerator RollingAward(RewardItem reward)
    {
        StopPreviousRolling();//��ֹ֮ͣǰ��rolling
        currentItem = reward.reward;//���õ���
        for (int j = 0; j < 2; j++)//ѭ�������������
        {
            for (int i = 0; i < rewards.Length; i++)
            {
                GameObject go = Instantiate(Resources.Load<GameObject>("ScrollingItem"),ScrollImageParent);
                go.GetComponent<RawImage>().texture = rewards[i].texture;//������ͼ
                yield return new WaitForSeconds(0.25f);
                Destroy(go, 1f);
            }
        }
        GameObject end = Instantiate(Resources.Load<GameObject>("ScrollingItem"), ScrollImageParent);
        end.GetComponent<RawImage>().texture = reward.texture;
        end.GetComponent<ScrollingRawImage>().isFinalReward = true;//���һ�ֶ�������Դ�
        //Destroy(end, 5f);
        StaticData.UseItemSign = true;//ʰȡ������
    }
    /// <summary>
    /// ʹ�õ���
    /// </summary>
    private void UseItem()
    {
        print("Using" + currentItem);
        switch (currentItem)
        {
            case Reward.bullet:
                UseBullet();
                break;
            case Reward.boost:
                UseBoost();
                break;
            case Reward.banana:
                UseBanana();
                break;
            default:
                break;
        }
        StopPreviousRolling();
        
    }
    private void UseBullet()
    {
        //�����ڵ�
        GameObject bullet = Instantiate(Resources.Load("Bullet", typeof(GameObject))) as GameObject;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        Transform bulletRot = Camera.main.transform.GetChild(0);
        Vector3 bulletPos = Camera.main.transform.position;
        bullet.transform.localEulerAngles = new Vector3(bulletRot.rotation.x - 20, bulletRot.rotation.y, bulletRot.rotation.z); ;
        bulletPos.y -= (float)(0.7);
        bullet.transform.position = bulletPos;
        bullet.transform.eulerAngles = Camera.main.transform.forward;
        //�������ƶ��ڵ�
        rb.AddForce(Camera.main.transform.forward * 800f);
        //����������ڵ�
        Destroy(bullet, 5);
        //��Ч
        GetComponent<AudioSource>().Play();
    }
    private void UseBoost()
    {
        //����
        StartCoroutine(DelayAnimate());
        source.PlayOneShot(boostSound, 1F);   //������Ч
    }
    private IEnumerator DelayAnimate()
    {
        GameObject CarOwn = this.gameObject;
        print("����Э��");
        CarOwn.GetComponent<Animator>().SetBool("isBoost", true);
        //���ٺ�Ч��
        GameObject boostfx = Instantiate(Resources.Load("BoostFx", typeof(GameObject)), transform) as GameObject;
        //���ٺ���Ч��
        GameObject Camera = GameObject.Find("AR Camera");
        boostfx.transform.parent = Camera.transform;
        Vector3 fxpos = new Vector3(0, -0.06f, 0.663f);
        boostfx.transform.localPosition = fxpos;
        yield return new WaitForSeconds(0.25f);
        CarOwn.GetComponent<Animator>().SetBool("isBoost", false);
        yield return new WaitForSeconds(1.25f);
        Destroy(boostfx);//����ǰ���0.25����2������ٻ���Ч��

    }
    private void UseBanana()
    {

    }
    
    public void ResetGame()
    {
        Tx_CoinNum.text = "0";//�������
        CirText.text = "0";//��¼Ȧ��
        CountCir = 0;//��¼��ǰȦ��
        StopPreviousRolling();
    }
}