using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{

    Transform target;
    private void Start()
    {

        target = GameObject.Find("Mate1").transform;

    }
    private void Update()
    {

        transform.LookAt(target.position);
        transform.position = Vector3.MoveTowards(transform.position, target.position, 1f * Time.deltaTime);
        Vector3 delta = target.position - transform.position;
        if (delta.sqrMagnitude < 0.00001f)
        {
            TriggerBomb();
        }

    }
    void TriggerBomb()
    {
        AIControl.Instance.HitLeftCar();

        Destroy(this.gameObject);
    }
    
}
