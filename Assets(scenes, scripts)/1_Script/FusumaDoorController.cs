using System.Collections;
using UnityEngine;

public class FusumaDoorController : MonoBehaviour
{
    [Header("Doors")]
    public Transform leftDoor;
    public Transform rightDoor;

    [Header("Slide Settings")]
    public float slideDistance = 0.8f;
    public float slideDuration = 1.0f;

    private Vector3 leftClosedPos;
    private Vector3 rightClosedPos;
    private Vector3 leftOpenPos;
    private Vector3 rightOpenPos;

    private bool isOpen = false;
    private bool isMoving = false;

    void Start()
    {
        if (leftDoor != null)
            leftClosedPos = leftDoor.localPosition;

        if (rightDoor != null)
            rightClosedPos = rightDoor.localPosition;

        leftOpenPos = leftClosedPos + Vector3.back * slideDistance;
        rightOpenPos = rightClosedPos + Vector3.forward * slideDistance;
    }

    public void OpenDoors()
    {
        if (isMoving || isOpen) return;
        StartCoroutine(SlideDoors(leftClosedPos, leftOpenPos, rightClosedPos, rightOpenPos));
    }

    public void CloseDoors()
    {
        if (isMoving || !isOpen) return;
        StartCoroutine(SlideDoors(leftOpenPos, leftClosedPos, rightOpenPos, rightClosedPos));
    }

    private IEnumerator SlideDoors(
        Vector3 leftFrom, Vector3 leftTo,
        Vector3 rightFrom, Vector3 rightTo)
    {
        isMoving = true;

        float t = 0f;
        while (t < slideDuration)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / slideDuration);

            if (leftDoor != null)
                leftDoor.localPosition = Vector3.Lerp(leftFrom, leftTo, p);

            if (rightDoor != null)
                rightDoor.localPosition = Vector3.Lerp(rightFrom, rightTo, p);

            yield return null;
        }

        if (leftDoor != null)
            leftDoor.localPosition = leftTo;

        if (rightDoor != null)
            rightDoor.localPosition = rightTo;

        isOpen = !isOpen;
        isMoving = false;
    }
}