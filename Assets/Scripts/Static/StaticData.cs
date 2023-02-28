using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class StaticData 
{
    public static int CoinNum = 100;//金币数量

    public static bool isObservedFinshed = false;//if all 4 gates were observed

    public static int[] GateObserved = {0,0,0,0};//mark 4 gates observed status

    public static bool EndTimeTrial = false;//三圈是否结束

    public static string[] TimeRecord = new string[4];//默认只存储四次游戏记录

    public static CarColor carColor = CarColor.RED;

    public static int[] ToolLevel = {1,1,1};

    /// <summary>
    /// 检查是否在未完成上一圈的情况下重复通过同一个门
    /// </summary>
    /// <param name="pos">数组下标，0~3</param>
    /// <returns>本次计数器+1是否合法</returns>
    public static bool CheckGatePassingValidity(int pos)
    {
        int max = GateObserved.Max();
        int min = GateObserved.Min();
        
        if (max == min&&pos==0)//上一圈跑完，再一次经过一号门
        {
            return true;
        }
        if (GateObserved[pos] + 1 - min > 1)//重复经过某个门导致比完整圈数大2圈以上
        {
            return false;
        }
        if (max - min == 1)//在某一圈途中
        {
            for(int i = 0; i < GateObserved.Length; i++)
            {
                if(GateObserved[i] != max)//预计需要经过的门
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
    


    
}
