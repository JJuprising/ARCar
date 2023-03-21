using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class StaticData
{
    public static int CoinNum = 100;//金币数量

    public static int totalCoinNum = 200;

    public static bool isObservedFinshed = false;//if all 4 gates were observed

    public static int[] GateObserved = { 0, 0, 0, 0 };//mark 4 gates observed status

    public static bool EndTimeTrial = false;//三圈是否结束

    public static string[] TimeRecord = new string[4];//默认只存储四次游戏记录

    public static CarColor carColor = CarColor.RED;

    public static int[] ToolLevel = { 1, 1, 1 ,1};

    public static bool UseItemSign = false;//按钮触发，按下变为true触发getthing的useitem事件

    public static int racemode = 0;

    
    public static int[] UpgradeCost = {50, 75, 100, 150 };//升级费用，暂时写在这里

    public static int[] targetCoin = { 0, 100, 300 };//车辆解锁费用，暂时写在这里

    public static bool isUseTool = false;//眼电触发SocketClient.cs,当为true使用道具
    /// <summary>
    /// 检查是否在未完成上一圈的情况下重复通过同一个门
    /// </summary>
    /// <param name="pos">数组下标，0~3</param>
    /// <returns>本次计数器+1是否合法</returns>
    public static bool CheckGatePassingValidity(int pos)
    {
        int max = GateObserved.Max();
        int min = GateObserved.Min();

        if (max == min && pos == 0)//上一圈跑完，再一次经过一号门
        {
            return true;
        }
        if (GateObserved[pos] + 1 - min > 1)//重复经过某个门导致比完整圈数大2圈以上
        {
            return false;
        }
        if (max - min == 1)//在某一圈途中
        {
            for (int i = 0; i < GateObserved.Length; i++)
            {
                if (GateObserved[i] != max)//预计需要经过的门
                {
                    if (i == pos)//当前通过的门
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
        Debug.Log("score：" + GateObserved[0] + "|" + GateObserved[1] + "|" + GateObserved[2] + "|" + GateObserved[3]);
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

