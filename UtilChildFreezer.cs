using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util__ChildFreezer : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector3 initialWorldPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initialWorldPosition = rectTransform.position; // world position
    }

    void LateUpdate()
    {
        // Lock it back to its original world position every frame
        rectTransform.position = initialWorldPosition;
    }
}
