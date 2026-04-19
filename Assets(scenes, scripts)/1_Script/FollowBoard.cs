using UnityEngine;

public class FollowBoardVR : MonoBehaviour
{
    #使ってない

    public Transform targetCamera;

    [Header("position setting")]
    public float forwardDistance = 3.6f;
    public float upwardOffset = 0.45f;
    public float leftOffset = -0.45f;

    void LateUpdate()
    {
        if (targetCamera == null) return;

        Vector3 targetPos =
            targetCamera.position
            + targetCamera.forward * forwardDistance
            + targetCamera.up * upwardOffset
            + targetCamera.right * leftOffset;

        transform.position = targetPos;

        transform.rotation = Quaternion.LookRotation(transform.position - targetCamera.position);    }
}