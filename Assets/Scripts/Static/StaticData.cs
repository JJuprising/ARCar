using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class StaticData 
{
    public static int CoinNum = 100;//�������

    public static bool isObservedFinshed = false;//if all 4 gates were observed

    public static int[] GateObserved = {0,0,0,0};//mark 4 gates observed status

    public static bool EndTimeTrial = false;//��Ȧ�Ƿ����

    public static string[] TimeRecord = new string[4];//Ĭ��ֻ�洢�Ĵ���Ϸ��¼

    public static CarColor carColor = CarColor.RED;

    public static int[] ToolLevel = {1,1,1};

    /// <summary>
    /// ����Ƿ���δ�����һȦ��������ظ�ͨ��ͬһ����
    /// </summary>
    /// <param name="pos">�����±꣬0~3</param>
    /// <returns>���μ�����+1�Ƿ�Ϸ�</returns>
    public static bool CheckGatePassingValidity(int pos)
    {
        int max = GateObserved.Max();
        int min = GateObserved.Min();
        
        if (max == min&&pos==0)//��һȦ���꣬��һ�ξ���һ����
        {
            return true;
        }
        if (GateObserved[pos] + 1 - min > 1)//�ظ�����ĳ���ŵ��±�����Ȧ����2Ȧ����
        {
            return false;
        }
        if (max - min == 1)//��ĳһȦ;��
        {
            for(int i = 0; i < GateObserved.Length; i++)
            {
                if(GateObserved[i] != max)//Ԥ����Ҫ��������
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
    


    
}
