using UnityEngine;

public class WaterDetectPoint : MonoBehaviour
{
    public LadleWaterController ladle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            ladle.FillWater();
        }
    }
}