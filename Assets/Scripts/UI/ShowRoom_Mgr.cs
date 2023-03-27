using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ShowRoom_Mgr : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StaticData.LoadDataFromFile();//加载存档
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Btn_Store()
    {
        SceneManager.LoadScene("CarSwitch");//跳转到开始游戏界面
    }
    /// <summary>
    /// 开始游戏
    /// </summary>
    public void Btn_Start()
    {
        SceneManager.LoadScene("AR_Scene");//跳转到开始游戏界面
    }
    public void Btn_Back()
    {
        SceneManager.LoadScene("UI");//跳转到开始游戏界面
    }
}
