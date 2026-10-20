using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Util_MouseLog : MonoBehaviour
{
    private EventSystem _eventSystem;
    private PointerEventData _uiEventData;

    private void Awake()
    {
        _eventSystem = Object.FindFirstObjectByType<EventSystem>();
        if (_eventSystem == null)
        {
            Debug.LogError("EventSystem not found in the scene.");
            return;
        }

        _uiEventData = new PointerEventData(_eventSystem);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick("Left");
        }

        if (Input.GetMouseButtonDown(1))
        {
            HandleClick("Right");
        }
    }

    private void HandleClick(string buttonName)
    {
        // Try UI first
        if (RaycastUI(out GameObject uiHit))
        {
            Debug.Log($"{buttonName} clicked on UI: {uiHit.name}");
            return;
        }

        // Then try 3D world
        if (RaycastWorld(out RaycastHit worldHit))
        {
            Debug.Log($"{buttonName} clicked on 3D object: {worldHit.collider.gameObject.name}");
        }
    }

    private bool RaycastUI(out GameObject hit)
    {
        _uiEventData.position = Input.mousePosition;
        List<RaycastResult> results = new();
        _eventSystem.RaycastAll(_uiEventData, results);

        if (results.Count > 0)
        {
            hit = results[0].gameObject;
            return true;
        }

        hit = null;
        return false;
    }

    private bool RaycastWorld(out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            return true;
        }

        hit = default;
        return false;
    }
}