using UnityEngine;

public class FloatingGuideArrow : MonoBehaviour
{
    [Header("references")]
    public Transform playerCamera;
    public Transform goalTarget;
    public Renderer arrowRenderer;

    [Header("Position")]
    public float distanceFromPlayer = 1.5f;
    public float heightOffset = -0.05f;

    [Header("Floating")]
    public float floatSpeed = 2f;
    public float floatAmount = 0.08f;

    [Header("Distance")]
    public float hideDistance = 0.7f;

    void Update()
    {
        if (playerCamera == null || goalTarget == null) return;

        float playerToGoal = Vector3.Distance(playerCamera.position, goalTarget.position);

        if (arrowRenderer != null)
            arrowRenderer.enabled = playerToGoal > hideDistance;

        if (playerToGoal <= hideDistance)
            return;

        Vector3 basePos =
            playerCamera.position +
            playerCamera.forward * distanceFromPlayer +
            playerCamera.up * heightOffset;

        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatAmount;
        transform.position = basePos + new Vector3(0f, yOffset, 0f);

        Vector3 dir = goalTarget.position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.001f)
        {
            transform.rotation =
                Quaternion.LookRotation(dir.normalized) *
                Quaternion.Euler(0f, -90f, 0f);
        }
    }
}