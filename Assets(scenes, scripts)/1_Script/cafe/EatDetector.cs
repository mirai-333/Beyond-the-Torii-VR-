using UnityEngine;

public class FoodEatDetector : MonoBehaviour
{
    public CafeFlowManager cafeFlowManager;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("food")) return;
        if (cafeFlowManager == null) return;

        cafeFlowManager.OnFoodEaten(other.gameObject);
    }
}