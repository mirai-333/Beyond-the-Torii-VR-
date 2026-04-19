using UnityEngine;

public class CafeExitTrigger : MonoBehaviour
{
    public CafeFlowManager cafeFlowManager;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (cafeFlowManager == null) return;

        cafeFlowManager.OnReachExitArea();
    }
}