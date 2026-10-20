
using DG.Tweening;
using UnityEngine;

public class UtilPhysPositionReset3D : MonoBehaviour
{
    [SerializeField] private Vector3 resetPosition;
    [SerializeField] private bool triggerResetTransform;

    private void Update()
    {
        if (!triggerResetTransform) return;
        ResetTransform(); triggerResetTransform=false;
    }

    private void ResetTransform()
    {
        gameObject.TryGetComponent(out Rigidbody rb);
        gameObject.TryGetComponent(out Transform pos);
        if (rb) rb.isKinematic = true;
        if (pos) pos.position = resetPosition;
        if (pos) pos.rotation = Quaternion.identity;
        DOVirtual.DelayedCall(0.1f, () =>
        {
            if (rb) rb.isKinematic = false;
            if (rb) rb.linearVelocity = Vector3.zero; 
        });


    }
}
