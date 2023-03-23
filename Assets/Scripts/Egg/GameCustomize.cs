using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameCustomize : Singleton<GameCustomize>
{
    

    [SerializeField]GameObject[] availableCarColor;//��ѡ����ɫ�Ķ���
    [SerializeField] GameObject[] availableTool;//��ѡ����ɫ�Ķ���
    [SerializeField]CarColor[] TargetCarColors;//��ѡ����ɫ�����Ϸ���Ӧ
    
    int[] ImageUV;//ͼ�������

    int colorCount;
    int toolCount;
    int selectingColorIndex = 0;//ѡ����ɫ���±�

    [SerializeField] Button chooseButton;//ѡ�����İ�ť
    [SerializeField] Text TotalCoinText;//��������ı�
    [SerializeField] Text CurrentCoinText;//��������ı�
    [SerializeField] Text selectText;//��ѡ�������ı�




    private void Start()
    {
        colorCount = availableCarColor.Length;
        toolCount = availableTool.Length;
        ImageUV = new int[colorCount];
        UpdatePanel();
    }
    private void InitCarPanel()
    {
        CarColor currentCarColor = StaticData.carColor;//��ȡ
        selectText.text = "Present:" + currentCarColor;
        for (int i = 0; i < colorCount; i++)
        {
            if (TargetCarColors[i] == currentCarColor)
            {
                selectingColorIndex = i;
                break;
            }
        }
    }
    private void InitCarSelection()
    {
        for(int i = 0;i< colorCount; i++)
        {
            ImageUV[i] = selectingColorIndex-i;
            RawImage img = availableCarColor[i].GetComponent<RawImage>();
            img.uvRect = new Rect(ImageUV[i], 0, img.uvRect.width, img.uvRect.height);
        }
    }

    /// <summary>
    /// ������ɫ�����θ�ȫ�ֱ���
    /// </summary>
    
    public void SetCarColor()
    {
        if (IsCarAvailable())
        {
            StaticData.carColor = TargetCarColors[selectingColorIndex] ;
            selectText.text = "Present:" + TargetCarColors[selectingColorIndex];
        }
        
    }
    
    public void SwitchCarColor(bool isLeft)
    {
        InitCarSelection();
        if (isLeft)
        {
            if (selectingColorIndex > 0)
            {
                selectingColorIndex--;
                for(int i = 0; i < colorCount; i++)
                {
                    ImageUV[i]--;
                }
            }
        }
        else
        {
            if(selectingColorIndex < colorCount - 1)
            {
                selectingColorIndex++;
                for (int i = 0; i < colorCount; i++)
                {
                    ImageUV[i]++;
                }
            }
        }
        TotalCoinText.text = $"TotalCoin:{StaticData.CoinNum}/{StaticData.targetCoin[selectingColorIndex]}";
        if (IsCarAvailable())
        {
            chooseButton.gameObject.SetActive(true);
            TotalCoinText.gameObject.SetActive(false);
        }
        else
        {
            chooseButton.gameObject.SetActive(false);
            TotalCoinText.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        MoveImage();
    }
    bool IsCarAvailable()
    {
        if (StaticData.totalCoinNum< StaticData.targetCoin[selectingColorIndex])
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    bool IsToolUpGradeable(Reward item)
    {
        if(item==Reward.None) return false;

        int level =  StaticData.GetToolLevel(item);
        if(level>=5) return false;

        if(StaticData.GetToolUpgradeCost(item)>StaticData.CoinNum) return false;

        return true;
    }
    void MoveImage()
    {
        for (int i = 0; i < colorCount; i++)
        {
            RawImage img = availableCarColor[i].GetComponent<RawImage>();
            float currentX = img.uvRect.x;
            currentX = Mathf.MoveTowards(currentX, ImageUV[i], Time.deltaTime * 5f);
            img.uvRect = new Rect(currentX, 0, img.uvRect.width, img.uvRect.height);
        }
    }
    public void UpdateTool(int index)
    {
        if (IsToolUpGradeable((Reward)(index + 1))) {
            StaticData.CoinNum -= StaticData.UpgradeCost[StaticData.ToolLevel[index]-1];
            StaticData.ToolLevel[index]++;
        }
        

        UpdatePanel();

    }

    
    private void UpdateToolPanel()
    {
        for(int i = 0;i < toolCount; i++)
        {
            availableTool[i].transform.GetComponentInChildren<Text>(true).text = $"Level {StaticData.ToolLevel[i]}";
            if(IsToolUpGradeable((Reward)(i+1)))
            {
                availableTool[i].transform.GetChild(2).gameObject.SetActive(true);
            }
            else
            {
                availableTool[i].transform.GetChild(2).gameObject.SetActive(false);
            }
        }
    }
    public void SaveGame()
    {
        StaticData.SaveDataToFile();
    }
    public void LoadGame()
    {
        StaticData.LoadDataFromFile();
        UpdatePanel();
    }
    private void UpdatePanel()
    {
        CurrentCoinText.text = $"Currency:{StaticData.CoinNum}";

        InitCarPanel();
        InitCarSelection();
        UpdateToolPanel();
    }
    ///�ص�showroom
    public void Btn_Confirm()
    {
        SceneManager.LoadScene("ShowRoom");
    }
}
