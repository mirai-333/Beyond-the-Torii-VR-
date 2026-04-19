using UnityEngine;


public class KimonoWearTrigger : MonoBehaviour
{
    [Header("Reference")]
    public GameObject wornKimonoObject;
    public GameObject grabbedKimonoObject;
    public Animator avatarAnimator; 
    public RuntimeAnimatorController defaultController; 
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    
    public GameObject[] objectsToHide; 

    [Header("UI")]
    public GameObject grabLabel;

    public AvatarMoveAnimator moveAnimator;
    private bool hasWorn = false;

    void Start()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrabbed);
        }
    }

    private void OnGrabbed(UnityEngine.XR.Interaction.Toolkit.SelectEnterEventArgs args)
    {
        if (grabLabel != null)
            grabLabel.SetActive(false);

        SoundManager.Instance.PlayKimonoGrab();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasWorn) return;

        if (!other.CompareTag("WearZone")) return;

        WearKimono();
    }

    public void WearKimono()
    {
        if (hasWorn) return;
        hasWorn = true;

        if (grabInteractable != null && grabInteractable.isSelected)
        {
            grabInteractable.interactionManager.SelectExit(
                grabInteractable.firstInteractorSelecting,
                grabInteractable
            );
        }

        SoundManager.Instance.PlayKimonoWear();

        if (moveAnimator != null)
        {
            moveAnimator.enabled = false;
        }

        if (avatarAnimator != null)
        {
            avatarAnimator.SetFloat("speed", 0f);
        }

        if (objectsToHide != null)
        {
            foreach (GameObject obj in objectsToHide)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
        }

        if (wornKimonoObject != null)
        {
            wornKimonoObject.SetActive(true);
        }

        if (grabbedKimonoObject != null)
        {
            grabbedKimonoObject.SetActive(false);
        }
    }
}