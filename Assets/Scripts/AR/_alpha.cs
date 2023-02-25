using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _alpha : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Color co = GetComponent<MeshRenderer>().material.color;
        co.a = 0;
        GetComponent<MeshRenderer>().material.color = co;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
