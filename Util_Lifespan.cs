using UnityEngine;

public class Util_Lifespan : MonoBehaviour
{
    [SerializeField] float time=1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, time);
    }
}
