using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticData 
{
    public static int CoinNum = 0;//金币数量

    public static bool isObservedFinshed = false;//if all 4 gates were observed

    public static int[] GateObserved = {0,0,0,0};//mark 4 gates observed status


    public static bool EndTimeTrial = false;//三圈是否结束

    public static int[] TimeRecord = new int[4];//默认只存储四次游戏记录
}
