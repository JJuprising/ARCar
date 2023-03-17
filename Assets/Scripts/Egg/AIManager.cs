using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : Singleton<AIManager>
{
    [SerializeField] private GameObject[] AIModels;

    private bool isInit = false;

    public bool isStart = false;

    private void Init()
    {

    }
    
}
