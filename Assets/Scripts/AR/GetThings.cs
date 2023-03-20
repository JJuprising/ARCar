using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GetThings : MonoBehaviour
{
    public Text Tx_CoinNum;//�������
    public Text CirText;//��¼Ȧ��
    private int CountCir=0;//��¼��ǰȦ��
    
    [SerializeField] private Transform ScrollImageParent;

    [SerializeField]private RewardItem[] rewards;

    Reward currentItem;

    private void Update()
    {
        //ʹ�õ���
        if (StaticData.UseItemSign)
        {
            StaticData.UseItemSign = false;
            UseItem();
        }
    }


    private void OnTriggerEnter(Collider other)
    {

        Vector3 PicupVec = other.transform.position;//ʰȡ�����λ��
        GameObject PickupEffect;//ʰȡЧ��

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
                other.gameObject.SetActive(false);
                GetGoldenBoxReward();
                //Pick up effect
                PickupEffect = Instantiate(Resources.Load("PickupBox", typeof(GameObject))) as GameObject;
                PickupEffect.transform.position = PicupVec;
                Destroy(other.gameObject);//��������
                break;
            case "Coin":
                Destroy(other.gameObject);
                Debug.Log("�񵽽��");
                //Pick up effect
                PickupEffect = Instantiate(Resources.Load("PickupCoin", typeof(GameObject))) as GameObject;
                PickupEffect.transform.position = PicupVec;
                StaticData.CoinNum++;//ȫ�ֵĽ������һ
                StaticData.totalCoinNum++;
                Tx_CoinNum.text = StaticData.CoinNum.ToString();//UI������ı��仯
                Destroy(other.gameObject);//��������
                break;
            case "Finish":
                print("ͨ��Finish����");
                Destroy(other.gameObject);
                StaticData.EndTimeTrial = true;//TimeTrial������ǣ�����TrialMgr�����������
                                               //����finish����ʾ
                GameObject finishObject = this.transform.GetChild(0).gameObject;
                finishObject.SetActive(true);
                Destroy(finishObject, 2);
                //���Ƽ�¼����
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
                Destroy(ScrollImageParent.GetChild(i).gameObject);
            }
        }
    }
    /// <summary>
    /// ��ȡ�������
    /// </summary>
    private void GetGoldenBoxReward()
    {
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

    }
    private void UseBanana()
    {

    }
}