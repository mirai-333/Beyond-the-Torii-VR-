using UnityEngine;

public class SimpleFloat : MonoBehaviour
{
    public float floatSpeed = 2f;
    public float floatAmount = 0.1f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float y = Mathf.Sin(Time.time * floatSpeed) * floatAmount;

        transform.position = startPos + new Vector3(0, y, 0);
    }
}