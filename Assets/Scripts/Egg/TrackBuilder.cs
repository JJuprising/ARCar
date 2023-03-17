using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackBuilder : Singleton<TrackBuilder>
{
    public GameObject[] doors;
    private void Start()
    {
        doors = new GameObject[4];
    }

    private void BuildTrack()
    {

    }
    private bool CheckValidity()
    {
        foreach(GameObject go in doors)
        {
            if(go == null) return false;
        }
        return true;
    }
    public Vector3 GetDoorPos(int index)
    {
        if(index<0|| index >= doors.Length) return Vector3.zero;

        return doors[index].transform.position;
    }
    public Vector3 quardaticBezier(int from,int to,float t)
    {
        if(from<0|| from >= doors.Length|| to < 0 || to >= doors.Length) return Vector3.zero;
        Vector3 a = doors[from].transform.position;

        Vector3 c = doors[to].transform.position;

        Vector3 b = (c - a)/2 + GetVerticalDir(c - a);

        Vector3 aa = a + (b - a) * t;
        Vector3 bb = b + (c - b) * t;
        return aa + (bb - aa) * t;
    }

    /// <summary>
    /// 获取某向量的垂直向量
    /// </summary>
    public Vector3 GetVerticalDir(Vector3 _dir)
    {
        
        if (_dir.x == 0)
        {
            return new Vector3(1, 0, 0);
        }
        else
        {
            return new Vector3(-_dir.z / _dir.x, 0, 1).normalized;
        }
    }

}
