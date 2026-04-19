using UnityEngine;

public class SimpleMirrorReflection : MonoBehaviour
{
    public Transform playerCamera;
    public Transform mirrorCamera;

    void LateUpdate()
    {
        if (playerCamera == null || mirrorCamera == null) return;

        Transform mirror = transform;

   
        Vector3 normal = mirror.forward;
        Vector3 mirrorPos = mirror.position;

        Vector3 toCam = playerCamera.position - mirrorPos;
        Vector3 reflectedPos = playerCamera.position - 2f * Vector3.Dot(toCam, normal) * normal;
        mirrorCamera.position = reflectedPos;

        Vector3 reflectedForward = Vector3.Reflect(playerCamera.forward, normal);

        mirrorCamera.rotation = Quaternion.LookRotation(reflectedForward, Vector3.up);
    }
}