using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GetThings : MonoBehaviour
{
    public Text Tx_CoinNum;//�������
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    private void OnTriggerEnter(Collider other)
    {
        Vector3 PicupVec = other.transform.position;//ʰȡ�����λ��
        GameObject PickupEffect;//ʰȡЧ��

        switch (other.gameObject.name){
            case "StartPlace":
                //print("��ʼ");
                //�����������StartPlace����Ч�㣬��ʼ��ʱ���������Ϸ��ʼ
                StartCoroutine(delaythreeSec());
                //��������ʱ����Ȼ��ʼ����Ч
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
            case "GoldBox":
                other.gameObject.SetActive(false);
                //Pick up effect
                PickupEffect = Instantiate(Resources.Load("PickupBox", typeof(GameObject))) as GameObject;
                PickupEffect.transform.position = PicupVec;
                Destroy(other.gameObject);//��������
                break;
            case "Coin":
                other.gameObject.SetActive(false);
                //Pick up effect
                PickupEffect = Instantiate(Resources.Load("PickupCoin", typeof(GameObject))) as GameObject;
                PickupEffect.transform.position = PicupVec;
                StaticData.CoinNum++;//ȫ�ֵĽ������һ
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
    //��ʾ����ʱ���붯��
    private IEnumerator delaythreeSec()
    {
        Vector3 NumberVec = this.transform.position;
        NumberVec.z += 2.5f;//ͼ����������λ��
        bool isMerge = true;//�ж��Ƿ��Ѿ�����������
        float time = 0;//��ʱ
        GameObject Number3Object = null, Number2Object = null, Number1Object = null, GoObject = null;
        while (time < 3)
        {
            time += Time.deltaTime;
            
            //���ɼ�ʱͼ��
            if (time <= 1&&time>=0&&isMerge)
            {
                //3
                
                isMerge = false;
                Number3Object = Instantiate(Resources.Load("number3", typeof(GameObject))) as GameObject;
                Number3Object.transform.position = NumberVec;
                Number3Object.transform.parent = this.transform;
            }
            
            if (time > 1&&time<=2&&isMerge==false)
            {
                //2
                
                isMerge = true;
                Number2Object = Instantiate(Resources.Load("number2", typeof(GameObject))) as GameObject;
                NumberVec.z += 0.01f;
                Number2Object.transform.position = Number3Object.transform.position;
                Number2Object.transform.parent = this.transform;
                Destroy(Number3Object);//��������
            }
            if (time > 2&&time<3&&isMerge==true)
            {
                //1
                
                isMerge =false;
                
                Number1Object = Instantiate(Resources.Load("number1", typeof(GameObject))) as GameObject;
                NumberVec.z -= 0.01f;
                Number1Object.transform.position = Number2Object.transform.position;
                Number1Object.transform.parent = this.transform;
                Destroy(Number2Object);
            }
            yield return null;
        }
        Destroy(Number1Object);
        
        GoObject = Instantiate(Resources.Load("go_2", typeof(GameObject))) as GameObject;
        NumberVec.z += 0.01f;
        GoObject.transform.position = Number1Object.transform.position;
        GoObject.transform.parent = this.transform;
        Destroy(Number1Object);
        Destroy(GoObject, 0.3f);
        TrialMgr.Instance.startCountTime();//�����ʼ��ʱ
    }
   
}
