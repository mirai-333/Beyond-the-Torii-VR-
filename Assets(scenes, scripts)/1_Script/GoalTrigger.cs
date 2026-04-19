using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public TutorialUIManager uiManager;
    public int eventIndex = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (uiManager == null) return;

        if (eventIndex == 1)
        {
            uiManager.CompleteEvent1();
        }
        else if (eventIndex == 2)
        {
            uiManager.CompleteEvent2();
        }

        else if (eventIndex == 3)
        {
            uiManager.CompleteEvent3();
        }

        else if (eventIndex == 4)
        {
            uiManager.CompleteEvent4();
        }
    }
}