using UnityEngine;
using UnityEngine.UI;

public class ChozuyaTutorialManager : MonoBehaviour
{
    public enum TutorialStep
    {
        GrabLadle,
        ScoopWater,
        WashLeftHand,
        WashRightHand,
        Done
    }

    [Header("UI")]
    public GameObject ladleLabel;
    public Image wallImage;

    [Header("Sound")]
    public AudioSource washAudioSource;
    public AudioClip taikoClip2;

    [Header("Board Contents")]
    public Sprite grabLadleSprite;
    public Sprite scoopWaterSprite;
    public Sprite washLeftHandSprite;
    public Sprite washRightHandSprite;
    public Sprite doneSprite;

    [Header("Link")]
    public TutorialUIManager tutorialUIManager;

    [Header("State")]
    public TutorialStep currentStep = TutorialStep.GrabLadle;

    void Start()
    {
        if (ladleLabel != null) ladleLabel.SetActive(true);
        if (wallImage != null && grabLadleSprite != null)
            wallImage.sprite = grabLadleSprite;
    }

    public void OnLadleGrabbed()
    {
        if (currentStep != TutorialStep.GrabLadle) return;

        if (ladleLabel != null)
            ladleLabel.SetActive(false);

        currentStep = TutorialStep.ScoopWater;
        SetWallSprite(scoopWaterSprite);
    }

    public void OnWaterScooped()
    {
        if (currentStep != TutorialStep.ScoopWater) return;

        currentStep = TutorialStep.WashLeftHand;
        SetWallSprite(washLeftHandSprite);
    }

    public void OnLeftHandWashed()
    {
        if (currentStep != TutorialStep.WashLeftHand) return;

        currentStep = TutorialStep.WashRightHand;
        SetWallSprite(washRightHandSprite);
    }

    public void OnRightHandWashed()
    {
        if (currentStep != TutorialStep.WashRightHand) return;

        currentStep = TutorialStep.Done;
        SetWallSprite(doneSprite);

        if (tutorialUIManager != null)
            tutorialUIManager.ShowEvent2Guide();
    }

    private void SetWallSprite(Sprite nextSprite)
    {
        if (wallImage != null && nextSprite != null)
            wallImage.sprite = nextSprite;

        if (washAudioSource != null && taikoClip2 != null)
            washAudioSource.PlayOneShot(taikoClip2);
    }

    
}