
using UnityEngine;
using UnityEngine.EventSystems;

public class Util__Follow : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] float followSpeed = 10;
    [SerializeField] float followDistance;
    public bool doFollow;
    [SerializeField] bool disableZ;
    [SerializeField] bool controlDistanceWithMouse;
    [SerializeField] bool followRotationY;
    private float _ogspeed;

    private void Start()
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
        if (followRotationY)
        {
            transform.rotation = Quaternion.Euler(
                transform.rotation.x, 
                target.rotation.eulerAngles.y, 
                transform.rotation.z);
        }
                
            
        transform.position = Vector3.Lerp(transform.position, targetPosition+offset, Time.deltaTime * followSpeed);

        if (controlDistanceWithMouse) followDistance -= Input.GetAxis("Mouse ScrollWheel") * 5;
    }

    // Drag Initialization
    public void OnBeginDrag(PointerEventData eventData)
    {
        doFollow = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        doFollow = true;
    }
}