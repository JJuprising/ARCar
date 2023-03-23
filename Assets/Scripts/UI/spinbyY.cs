using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinbyY : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ÈÆYÖáÐý×ª
        this.transform.Rotate(0, 0.6f, 0, Space.World);
    }
}
