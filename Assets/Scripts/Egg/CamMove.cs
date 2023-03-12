using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// �����ǿ��������ת
/// </summary>
public class CamMove : MonoBehaviour
{
    private Transform cacheTransform;
    [Tooltip("��תƫ��")]
    [SerializeField]
    private Vector3 originAxisRotation = Vector3.zero;
    [Tooltip("��ת�ٶ�")]
    [SerializeField]
    private float speed = 0.5f;
    [Tooltip("�̶�X��")]
    [SerializeField]
    private bool freezeX = false;
    [Tooltip("�̶�Y��")]
    [SerializeField]
    private bool freezeY = false;
    [Tooltip("�̶�Z��")]
    [SerializeField]
    private bool freezeZ = false;

    public Text debugText;

    Vector3 acceleration;

    Rigidbody rb;

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        cacheTransform = transform;
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (SystemInfo.supportsGyroscope)
        {
            //�����豸�����ǵĿ���/�ر�״̬��ʹ�������ǹ��ܱ�������Ϊ true  
            Input.gyro.enabled = true;
            //���������ǵĸ��¼���ʱ�䣬���� 0.1�����һ��  
            Input.gyro.updateInterval = 0.1f;
            //��ȡ�豸�������ٶ�����  
            //Vector3 deviceGravity = Input.gyro.gravity;
            //�豸����ת�ٶȣ����ؽ��Ϊx��y��z�����ת�ٶȣ���λΪ������/�룩  
            //Vector3 rotationVelocity = Input.gyro.rotationRate;
            //��ȡ���Ӿ�ȷ����ת  
            //Vector3 rotationVelocity2 = Input.gyro.rotationRateUnbiased;
            //��ȡ�Ƴ��������ٶȺ��豸�ļ��ٶ�  
            
        }
    }

    private void Update()
    {
        if (SystemInfo.supportsGyroscope)
        {
            Quaternion attitude = GyroAttitude();
            Quaternion rotation = Quaternion.Slerp(cacheTransform.rotation, attitude, speed);
            Vector3 euler = rotation.eulerAngles;
            euler.x = freezeX ? 0 : euler.x;
            euler.y = freezeY ? 0 : euler.y;
            euler.z = freezeZ ? 0 : euler.z;
            cacheTransform.rotation = Quaternion.Euler(euler);

            if (debugText != null)
            {
                string debug = string.Format("Gyro: {0}\nCamera: {1}\nAcc:{2}",
                    Input.gyro.attitude, cacheTransform.rotation.eulerAngles, Input.gyro.userAcceleration);
                debugText.text = debug;
            }

            acceleration = Input.gyro.userAcceleration;
            rb.AddForce(acceleration*10f);
        }
    }

    // Unity:  ��������ϵ
    // ������: ��������ϵ
    // ����ת����������ϵת������������ϵ
    private Quaternion ConvertRotation(Quaternion q)
    {
        // ��������ϵת��������ϵֻ�轫z�ᷴ�򼴿�
        // q.w: �����ת�������ֻ���ת�����෴
        //-q.w: �����ת�������ֻ���ת������ͬ
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }

    // �ֻ�ˮƽ����ʱ������̬��Ϊ0
    // ��ȡ������̬��
    private Quaternion GyroAttitude()
    {
        Quaternion q = ConvertRotation(Input.gyro.attitude);
        //if (Screen.orientation == ScreenOrientation.LandscapeLeft)
            q = Quaternion.Euler(90, 0, 0) * q;//����У��
        q = q * Quaternion.Euler(originAxisRotation);
        return q;
    }
}