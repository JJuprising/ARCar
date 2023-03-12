using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 陀螺仪控制像机旋转
/// </summary>
public class CamMove : MonoBehaviour
{
    private Transform cacheTransform;
    [Tooltip("旋转偏移")]
    [SerializeField]
    private Vector3 originAxisRotation = Vector3.zero;
    [Tooltip("旋转速度")]
    [SerializeField]
    private float speed = 0.5f;
    [Tooltip("固定X轴")]
    [SerializeField]
    private bool freezeX = false;
    [Tooltip("固定Y轴")]
    [SerializeField]
    private bool freezeY = false;
    [Tooltip("固定Z轴")]
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
            //设置设备陀螺仪的开启/关闭状态，使用陀螺仪功能必须设置为 true  
            Input.gyro.enabled = true;
            //设置陀螺仪的更新检索时间，即隔 0.1秒更新一次  
            Input.gyro.updateInterval = 0.1f;
            //获取设备重力加速度向量  
            //Vector3 deviceGravity = Input.gyro.gravity;
            //设备的旋转速度，返回结果为x，y，z轴的旋转速度，单位为（弧度/秒）  
            //Vector3 rotationVelocity = Input.gyro.rotationRate;
            //获取更加精确的旋转  
            //Vector3 rotationVelocity2 = Input.gyro.rotationRateUnbiased;
            //获取移除重力加速度后设备的加速度  
            
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

    // Unity:  左手坐标系
    // 陀螺仪: 右手坐标系
    // 将旋转从右手坐标系转换到左手坐标系
    private Quaternion ConvertRotation(Quaternion q)
    {
        // 右手坐标系转左手坐标系只需将z轴反向即可
        // q.w: 像机旋转方向与手机旋转方向相反
        //-q.w: 像机旋转方向与手机旋转方向相同
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }

    // 手机水平放置时陀螺仪态势为0
    // 获取陀螺仪态势
    private Quaternion GyroAttitude()
    {
        Quaternion q = ConvertRotation(Input.gyro.attitude);
        //if (Screen.orientation == ScreenOrientation.LandscapeLeft)
            q = Quaternion.Euler(90, 0, 0) * q;//竖屏校正
        q = q * Quaternion.Euler(originAxisRotation);
        return q;
    }
}