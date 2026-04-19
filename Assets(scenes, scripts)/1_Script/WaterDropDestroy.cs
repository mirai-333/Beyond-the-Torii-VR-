using UnityEngine;

public class WaterDropAutoDestroy : MonoBehaviour
{
    public float lifeTime = 1.5f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}