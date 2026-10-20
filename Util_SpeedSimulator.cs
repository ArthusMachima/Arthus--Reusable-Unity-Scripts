
using UnityEngine;

public class Util_SpeedSimulator : MonoBehaviour
{
    [SerializeField] Vector3 direction;
    [SerializeField] float perSecond = 1;
    
    float time;

    void FixedUpdate()
    {
        time  += Time.fixedDeltaTime;
        if (!(time >= perSecond)) return;
        transform.position += direction;
        time = 0;
    }
}
