using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ShowRoom_Mgr : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StaticData.LoadDataFromFile();//���ش浵
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Btn_Store()
    {
        SceneManager.LoadScene("CarSwitch");//��ת����ʼ��Ϸ����
    }
    /// <summary>
    /// ��ʼ��Ϸ
    /// </summary>
    public void Btn_Start()
    {
        SceneManager.LoadScene("AR_Scene");//��ת����ʼ��Ϸ����
    }
    public void Btn_Back()
    {
        SceneManager.LoadScene("UI");//��ת����ʼ��Ϸ����
    }
}
