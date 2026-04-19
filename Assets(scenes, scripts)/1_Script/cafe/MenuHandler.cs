using UnityEngine;

public class MenuInteractHandler : MonoBehaviour
{
    public CafeFlowManager cafeFlowManager;

    public void OnMenuSelected()
    {
        if (cafeFlowManager != null)
            cafeFlowManager.OnMenuPressed();
    }
}