using UnityEngine;

public class Util_Follow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] float followSpeed = 10;
    [SerializeField] float followDistance;
    public bool doFollow;
    [SerializeField] bool disableZ;
    [SerializeField] bool followRotationX;
    [SerializeField] bool followRotationY;
    [SerializeField] bool followRotationZ;
    float _ogspeed;

    void Start()
    {
        _ogspeed = followSpeed;
    }

    public void SetPos(Transform pos)
    {
        target = pos;
    }

    public void SetSpeed(float speed)
    {
        followSpeed = speed;
    }

    public void ResetSpeed()
    {
        followSpeed = _ogspeed;
    }

    void Update()
    {
        if (!doFollow || !target) return;
        var direction = (transform.position - target.position).normalized;
        var targetPosition = target.position + direction * followDistance;
        if (disableZ) targetPosition.z = transform.position.z;
        
        if (followRotationX || followRotationY || followRotationZ)
        {
            var rotx = transform.rotation.x;
            var roty = transform.rotation.y;
            var rotz = transform.rotation.z;
            
            if (followRotationX) rotx = target.rotation.eulerAngles.x;
            if (followRotationX) roty = target.rotation.eulerAngles.y;
            if (followRotationX) rotz = target.rotation.eulerAngles.z;
            
            transform.rotation = Quaternion.Euler(rotx, roty, rotz);
        }
                
            
        transform.position = Vector3.Lerp(transform.position, targetPosition+offset, Time.deltaTime * followSpeed);

    }
}