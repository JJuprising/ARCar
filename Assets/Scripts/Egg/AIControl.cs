using UnityEngine;

public class AIControl : Singleton<AIControl>
{
    [SerializeField] GameObject car1;
    [SerializeField] GameObject car2;
    [SerializeField] float speed1 = 0.6f;
    [SerializeField] float speed2 = 0.8f;
    [SerializeField] bool canMove = false;
    [SerializeField] bool isHit = false;
    [SerializeField] GameObject effect;
    private void Start()
    {
        car1.SetActive(false);
        car2.SetActive(false);
    }
    public void SetCarActive()
    {
        car1.SetActive(true);
        car2.SetActive(true);
    }
    public void StartMove()
    {
        canMove = true;
    }
    public void HitLeftCar()
    {
        isHit = true;
        GameObject go =  Instantiate(effect,car1.transform);
        go.transform.localPosition = Vector3.zero;

    }
    private void Update()
    {
        if (canMove)
        {
            
            if (isHit)
            {
                Vector3 pos = car1.transform.position;
                pos.z -= Time.deltaTime * speed1;
                car1.transform.position = pos;

                pos = car2.transform.position;
                pos.z += Time.deltaTime * speed2;
                car2.transform.position = pos;
            }
            else
            {
                Vector3 pos = car1.transform.position;
                pos.z += Time.deltaTime * speed1;
                car1.transform.position = pos;

                pos = car2.transform.position;
                pos.z += Time.deltaTime * speed2;
                car2.transform.position = pos;
            }
            
        }
    }

}
