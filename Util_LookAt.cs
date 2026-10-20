
using UnityEngine;

public class Util_LookAt : MonoBehaviour
{
    public Transform target;
    [SerializeField] float rotationSpeed=10;
    [SerializeField] Vector3 rotationOffset;
    [SerializeField] bool disableX;
    [SerializeField] bool disableY;
    [SerializeField] bool disableZ;
    [SerializeField] bool doLook; 
    float ogspeed;
    
    void Start()
    {
        ogspeed = rotationSpeed;
    }

    void Update()
    {
        if (doLook)
        {
            if (target!=null)
            {
                var direction = target.position - transform.position;
                if (disableX) direction.x = 0;
                if (disableY) direction.y = 0;
                if (disableZ) direction.z = 0;
                if (direction != Vector3.zero)
                {
                    var lookRotation = Quaternion.LookRotation(direction);
                    var finalRotation = lookRotation * Quaternion.Euler(rotationOffset);

                    transform.rotation = Quaternion.Lerp(
                        transform.rotation,
                        finalRotation,
                        Time.deltaTime * rotationSpeed
                    );
                }
            }
            
        }
    }

    public void SetSpeed(float speed)
    {
        rotationSpeed = speed;
    }

    public void ResetSpeed()
    {
        rotationSpeed = ogspeed;
    }

    public void SetLook(Transform pos)
    {
        target = pos;
    }

    public void SetRotationOffset(Vector3 offset)
    {
        rotationOffset = offset;
    }
}
