using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIControl : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float speed = 5f;
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (AIManager.Instance.isStart)
        {
            Move();
        }
    }
    private void Move()
    {
            Vector3 delta = Camera.main.transform.position - transform.position;
            float distance = delta.magnitude;
            distance = distance > 1f ? distance : 0f;
            rb.MovePosition(Vector3.MoveTowards(transform.position, Camera.main.transform.position + delta.normalized, distance * Time.deltaTime*speed));//
    }
}
