using TMPro;
using UnityEngine;

public class Util_TimeSystem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI outputText;

    [Header("Time")]
    [SerializeField] int hours = 1;   // 24-hour internal time (0-23)
    [SerializeField] int minutes = 0;

    [Header("Debug")]
    [SerializeField] float timeScale = 1f;

    void Start()
    {
        InvokeRepeating("Tick", timeScale, timeScale);
    }

    void Tick()
    {
        minutes++;
        if (minutes >= 60)
        {
            minutes = 0;
            hours++;
        }
        if (hours >= 24)
        {
            hours = 0;
        }

        // Convert internal 24h time to 12h display
        int displayHour = hours % 12;
        if (displayHour == 0) displayHour = 12;

        string period = hours < 12 ? "AM" : "PM";
        outputText.text = $"{displayHour:00}:{minutes:00} {period}";
    }

    public int GetHours() => hours;
    public int GetMinutes() => minutes;
}