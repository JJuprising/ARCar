using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticData 
{
    public static int CoinNum = 0;//�������

    public static bool isObservedFinshed = false;//if all 4 gates were observed

    public static int[] GateObserved = {0,0,0,0};//mark 4 gates observed status


    public static bool EndTimeTrial = false;//��Ȧ�Ƿ����

    public static int[] TimeRecord = new int[4];//Ĭ��ֻ�洢�Ĵ���Ϸ��¼
}
