
using UnityEngine;
using UnityEngine.Events;

enum States
{
    Box,
    Sphere
}

public class Util_TriggerInvoker : MonoBehaviour
{
    [SerializeField] UnityEvent onEnter;
    [SerializeField] UnityEvent onStay;
    [SerializeField] UnityEvent onExit;
    [SerializeField] int maxColDetection=3;
    [SerializeField] States collisionType = States.Box;
    int _maxColDetection;
    Collider selfCol;
    Collider[] hitColliders;
    //bool[] hitColliderEnterStates;
    bool hitColliderEnterStates;

    void Start()
    {
        selfCol = GetComponent<Collider>();
    }

    void Update()
    {
        
        hitColliders = new Collider[maxColDetection];
        //
        if (maxColDetection!=_maxColDetection)
        {
            //hitColliderEnterStates = new bool[maxColDetection];
            _maxColDetection = maxColDetection;
        }

        var count = collisionType switch
        {
            States.Box => Physics.OverlapBoxNonAlloc(transform.position, transform.localScale / 2, hitColliders, transform.rotation),
            States.Sphere => Physics.OverlapSphereNonAlloc(transform.position, transform.localScale.magnitude / 2, hitColliders),
            _ => 0
        };

        for (var i = 0; i < count; i++)
        {
            var t = hitColliders[i];
            //if (!t && hitColliderEnterStates[i]) { onExit?.Invoke(); hitColliderEnterStates[i] = false; }
            //if (hitColliderEnterStates) { onExit?.Invoke(); hitColliderEnterStates = false; }
            if (t == selfCol || !t || !t.CompareTag("Player")) continue;
            //if (!hitColliderEnterStates[i]) { onEnter?.Invoke(); hitColliderEnterStates[i] = true; }
            //if (!hitColliderEnterStates) { onEnter?.Invoke(); hitColliderEnterStates = true; }
            onStay?.Invoke();
        }
    }
}
