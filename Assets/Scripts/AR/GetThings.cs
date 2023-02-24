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
   
    public void OnCollisionEnter(Collision collision)
    {
        Vector3 PicupVec = collision.transform.position;//ʰȡ�����λ��
        //ʰȡ���ӹ���
        if (collision.gameObject.tag == "GoldBox")
        {
            collision.gameObject.SetActive(false);
            //Pick up effect
            GameObject PickupEffect = Instantiate(Resources.Load("PickupBox", typeof(GameObject))) as GameObject;
            PickupEffect.transform.position = PicupVec;
            Destroy(collision.collider.gameObject);//��������

        }
        //ʰȡ���
        else if (collision.gameObject.tag == "Coin")
        {
            collision.gameObject.SetActive(false);
            //Pick up effect
            GameObject PickupEffect = Instantiate(Resources.Load("PickupCoin", typeof(GameObject))) as GameObject;
            PickupEffect.transform.position = PicupVec;
            StaticData.CoinNum++;//ȫ�ֵĽ������һ
            Tx_CoinNum.text = StaticData.CoinNum.ToString();//UI������ı��仯
            Destroy(collision.collider.gameObject);//��������
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        
        
        if (other.gameObject.name == "StartPlace")
        {
            print("����");
            //�����������StartPlace����Ч�㣬��ʼ��ʱ���������Ϸ��ʼ
            StartCoroutine(delaythreeSec());
            //��������ʱ����Ȼ��ʼ����Ч

            Destroy(other.gameObject, 3f);//��������

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
            print(time);
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
