using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class StaticData
{
    public static int CoinNum = 100;//�������

    public static int totalCoinNum = 200;

    public static bool isObservedFinshed = false;//if all 4 gates were observed

    public static int[] GateObserved = { 0, 0, 0, 0 };//mark 4 gates observed status

    public static bool EndTimeTrial = false;//��Ȧ�Ƿ����

    public static string[] TimeRecord = new string[4];//Ĭ��ֻ�洢�Ĵ���Ϸ��¼

    public static CarColor carColor = CarColor.RED;

    public static int[] ToolLevel = { 1, 1, 1 ,1};

    public static bool UseItemSign = false;//��ť���������±�Ϊtrue����getthing��useitem�¼�

    public static int racemode = 0;

    
    public static int[] UpgradeCost = {50, 75, 100, 150 };//�������ã���ʱд������

    public static int[] targetCoin = { 0, 100, 300 };//�����������ã���ʱд������

    public static bool isUseTool = false;//�۵紥��SocketClient.cs,��Ϊtrueʹ�õ���
    /// <summary>
    /// ����Ƿ���δ�����һȦ��������ظ�ͨ��ͬһ����
    /// </summary>
    /// <param name="pos">�����±꣬0~3</param>
    /// <returns>���μ�����+1�Ƿ�Ϸ�</returns>
    public static bool CheckGatePassingValidity(int pos)
    {
        int max = GateObserved.Max();
        int min = GateObserved.Min();

        if (max == min && pos == 0)//��һȦ���꣬��һ�ξ���һ����
        {
            return true;
        }
        if (GateObserved[pos] + 1 - min > 1)//�ظ�����ĳ���ŵ��±�����Ȧ����2Ȧ����
        {
            return false;
        }
        if (max - min == 1)//��ĳһȦ;��
        {
            for (int i = 0; i < GateObserved.Length; i++)
            {
                if (GateObserved[i] != max)//Ԥ����Ҫ��������
                {
                    if (i == pos)//��ǰͨ������
                    {
                        return true;
                    }
                    break;
                }
            }
        }
        return false;
    }
    public static void printScore()
    {
        Debug.Log("score��" + GateObserved[0] + "|" + GateObserved[1] + "|" + GateObserved[2] + "|" + GateObserved[3]);
    }
    public static void SaveDataToFile()
    {
        FileStream file = File.Open(Application.persistentDataPath + "/GameData.dat", FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        GameSave gameSave = StoreData();
        bf.Serialize(file, gameSave);
        file.Close();

    }

    public static GameSave StoreData()
    {
        GameSave save = new GameSave();
        save.totalCoinNum = totalCoinNum;
        save.CoinNum = CoinNum;
        save.ToolLevel = ToolLevel;
        save.carColor = carColor;
        return save;
    }
    public static void RestoreData(GameSave save)
    {
        totalCoinNum =save.totalCoinNum;
        CoinNum =save.CoinNum;
        ToolLevel =save.ToolLevel;
        carColor =save.carColor;
    }
    public static void LoadDataFromFile()
    {
        BinaryFormatter bf = new BinaryFormatter();

        if (File.Exists(Application.persistentDataPath + "/GameData.dat"))
        {
            Debug.Log("file exist");
            FileStream file = File.Open(Application.persistentDataPath + "/GameData.dat", FileMode.Open);

            GameSave gameSave = (GameSave)bf.Deserialize(file);

            RestoreData(gameSave);
            
            file.Close();
        }
        
    }
    public static int GetToolLevel(Reward item)
    {
        if(item!=Reward.None)
        return ToolLevel[(int)item - 1];
        return -1;
    }
    public static int GetToolUpgradeCost(Reward item)
    {
        if (IsToolInMaxLevel((int)item-1)) return -1;
        return UpgradeCost[GetToolLevel(item)-1];
    }
    public static bool IsToolInMaxLevel(int index)
    {
        if (ToolLevel[index] < 5) return false;
        return true;
    }
}

