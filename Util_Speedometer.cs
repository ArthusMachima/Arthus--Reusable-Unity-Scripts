using TMPro;
using UnityEngine;

public class Util_Speedometer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI speedText;
    Vector3 coord;
    float timer;
    const float perSecond = 0.01f;

    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (!(timer >= perSecond)) return;
        var speed = (transform.position - coord).magnitude;
        //Debug.Log(speed);
        speedText.text = $"{speed/perSecond:F2}m/s";
        coord = transform.position;
        timer = 0;
    }
}
