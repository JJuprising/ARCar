using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    float speed = 5f;
    Vector3 direction = new Vector3();


    void Update()
    {
        direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            direction.z++;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction.z--;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction.x--;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction.x++;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            direction.y++;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            direction.y--;
        }

        Vector3 currentpos = Camera.main.transform.position;
        currentpos += direction.normalized*Time.deltaTime*speed;
        Camera.main.transform.position = currentpos;
    }
}
