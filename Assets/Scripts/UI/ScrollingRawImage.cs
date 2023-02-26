using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class ScrollingRawImage : MonoBehaviour
{
    private RawImage rawImage;
    public float xSpeed, ySpeed;
    private float xVal, yVal = -1;

    public bool isFinalReward = false;

    private void Awake()
    {
        rawImage = GetComponent<RawImage>();
    }

    private void Update()
    {
        xVal += Time.deltaTime * xSpeed;
        yVal += Time.deltaTime * ySpeed;
        if (isFinalReward&&yVal>0f)
        {
            yVal = 0f;
        }
        rawImage.uvRect = new Rect(xVal, yVal, rawImage.uvRect.width, rawImage.uvRect.height);
    }
}
