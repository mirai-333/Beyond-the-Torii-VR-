using UnityEngine;

public class CafeSeatTrigger : MonoBehaviour
{
    public CafeFlowManager cafeFlowManager;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (cafeFlowManager == null) return;

        cafeFlowManager.OnReachSeatArea();
    }
}