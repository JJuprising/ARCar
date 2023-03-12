using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCustomize : MonoBehaviour
{
    

    [SerializeField]GameObject[] availableCarColor;//��ѡ����ɫ�Ķ���
    [SerializeField] GameObject[] availableTool;//��ѡ����ɫ�Ķ���
    [SerializeField]CarColor[] TargetCarColors;//��ѡ����ɫ�����Ϸ���Ӧ
    [SerializeField]int[] targetCoin;
    int[] ImageUV;//ͼ�������

    int colorCount;
    int toolCount;
    int selectingColorIndex = 0;//ѡ����ɫ���±�

    [SerializeField] Button chooseButton;//ѡ�����İ�ť
    [SerializeField] Text coinText;//��������ı�
    [SerializeField] Text selectText;//��ѡ�������ı�


    private void Start()
    {
        colorCount = availableCarColor.Length;
        toolCount = availableTool.Length;   
        CarColor currentCarColor = StaticData.carColor;//��ȡ
        selectText.text = "Present:" + currentCarColor;
        ImageUV = new int[colorCount];
        for (int i = 0; i < colorCount; i++)
        {
            if (TargetCarColors[i] == currentCarColor)
            {
                selectingColorIndex = i;
                break;
            }
        }
        InitCarSelection();
    }
    private void InitCarSelection()
    {
        for(int i = 0;i< colorCount; i++)
        {
            ImageUV[i] = i - selectingColorIndex;
            RawImage img = availableCarColor[i].GetComponent<RawImage>();
            img.uvRect = new Rect(ImageUV[i], 0, img.uvRect.width, img.uvRect.height);
        }
    }

    /// <summary>
    /// ������ɫ�����θ�ȫ�ֱ���
    /// </summary>
    [SerializeField]
    public void SetCarColor()
    {
        if (IsCarAvailable())
        {
            StaticData.carColor = TargetCarColors[selectingColorIndex] ;
            selectText.text = "Present:" + TargetCarColors[selectingColorIndex];
        }
        
    }
    [SerializeField]
    public void SwitchCarColor(bool isLeft)
    {
        //InitCarSelection();
        if (isLeft)
        {
            if (selectingColorIndex > 0)
            {
                selectingColorIndex--;
                for(int i = 0; i < colorCount; i++)
                {
                    ImageUV[i]++;
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
                    ImageUV[i]--;
                }
            }
        }
        coinText.text = $"Coin:{StaticData.CoinNum}/{targetCoin[selectingColorIndex]}";
        if (IsCarAvailable())
        {
            chooseButton.gameObject.SetActive(true);
            coinText.gameObject.SetActive(false);
        }
        else
        {
            chooseButton.gameObject.SetActive(false);
            coinText.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        MoveImage();
    }
    bool IsCarAvailable()
    {
        if (StaticData.CoinNum< targetCoin[selectingColorIndex])
        {
            return false;
        }
        else
        {
            return true;
        }
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
        if (!IsToolInMaxLevel(index)) StaticData.ToolLevel[index]++;
        UpdatePanel();

    }

    bool IsToolInMaxLevel(int index)
    {
        if (StaticData.ToolLevel[index] < 5) return false;
        return true;   
    }
    void UpdatePanel()
    {
        for(int i = 0;i < toolCount; i++)
        {
            availableTool[i].transform.GetComponentInChildren<Text>(true).text = $"Level {StaticData.ToolLevel[i]}";
            if(IsToolInMaxLevel(i))
            {
                availableTool[i].transform.GetComponentInChildren<Button>(true).gameObject.SetActive(false);
            }
        }
    }
}
