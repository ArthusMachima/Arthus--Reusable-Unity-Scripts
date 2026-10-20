using UnityEngine;

public class Util_RotationWithMouse : MonoBehaviour
{
    [SerializeField] Transform AppliedObject;
    [SerializeField] [Range(1,20)] float RotationSpeed = 5f;
    [SerializeField] bool doRotate = true;
    [SerializeField] bool lockCursor = true;

    float xRotation;
    float yRotation;

    public static Util_RotationWithMouse inst { get; internal set; }

    void Awake()
    {
        inst = this;
    }

    void Start()
    {
        if (AppliedObject == null)
        {
            AppliedObject = transform;
        }
    }

    void OnEnable()
    {
        if (!lockCursor) return;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        if (!lockCursor) return;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (!doRotate) return;

        var mouseX = Input.GetAxis("Mouse X") * RotationSpeed;
        var mouseY = Input.GetAxis("Mouse Y") * RotationSpeed;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        yRotation += mouseX;

        AppliedObject.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
