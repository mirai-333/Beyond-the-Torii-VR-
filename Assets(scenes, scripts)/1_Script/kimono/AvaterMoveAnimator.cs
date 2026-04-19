using UnityEngine;

public class AvatarMoveAnimator : MonoBehaviour
{
    public Animator animator;
    public Transform xrOrigin;

    [Header("Settings")]
    public float speedMultiplier = 10f;
    public float smoothSpeed = 8f;

    private Vector3 lastPosition;
    private float currentSpeed;

    void Start()
    {
        if (xrOrigin != null)
            lastPosition = xrOrigin.position;
    }

    void Update()
    {
        if (animator == null || xrOrigin == null) return;

        Vector3 currentPosition = xrOrigin.position;
        float distanceMoved = Vector3.Distance(currentPosition, lastPosition);

        float targetSpeed = distanceMoved * speedMultiplier;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * smoothSpeed);

        animator.SetFloat("speed", currentSpeed);

        lastPosition = currentPosition;
    }
}