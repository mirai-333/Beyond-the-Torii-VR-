using UnityEngine;

public class HandWashTarget : MonoBehaviour
{
    public ChozuyaTutorialManager tutorialManager;
    public bool isLeftHand = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("WaterDrop")) return;
        if (tutorialManager == null) return;

        if (isLeftHand)
            tutorialManager.OnLeftHandWashed();
        else
            tutorialManager.OnRightHandWashed();
    }
}