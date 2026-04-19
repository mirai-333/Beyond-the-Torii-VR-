using UnityEngine;

public class MatchaProgressManager : MonoBehaviour
{
    [Header("reference")]
    public GameObject level1;
    public GameObject level2;
    public GameObject level3;

    [Header("Settings")]
    public string chasenTag = "Chasen";
    public float level1Threshold = 0.5f;
    public float level2Threshold = 1.2f;
    public float level3Threshold = 2.0f;
    public float minMovePerFrame = 0.002f;

    [Header("Sound")]
    public AudioSource matchaAudioSource;
    public AudioClip matchaSound;

    public TutorialUIManager tutorialUIManager;

    private Transform currentChasen;
    private Vector3 lastTipPosition;
    private bool isStirring = false;
    private float stirAmount = 0f;
    private bool hasPlayedLevel3Sound = false;

    void Start()
    {
        SetOnlyLevel(1);
    }

    void Update()
    {
        if (!isStirring || currentChasen == null)
        {
            StopMatchaSound();
            return;
        }

        Vector3 currentPos = currentChasen.position;
        float move = Vector3.Distance(currentPos, lastTipPosition);

        if (move > minMovePerFrame)
        {
            stirAmount += move;
            UpdateMatchaLevel();

            PlayMatchaSound();
        }
        else
        {
            StopMatchaSound();
        }

        lastTipPosition = currentPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(chasenTag)) return;

        currentChasen = other.transform;
        lastTipPosition = currentChasen.position;
        isStirring = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(chasenTag)) return;

        if (currentChasen == other.transform)
        {
            currentChasen = null;
            isStirring = false;
            StopMatchaSound();
        }
    }

    private void UpdateMatchaLevel()
    {
        if (stirAmount >= level3Threshold)
        {
            SetOnlyLevel(3);

            if (!hasPlayedLevel3Sound)
            {
                SoundManager.Instance.PlayKoto1();
                hasPlayedLevel3Sound = true;
                tutorialUIManager.ShowEvent4Guide();
            }
        }
        else if (stirAmount >= level2Threshold)
        {
            SetOnlyLevel(2);
        }
        else
        {
            SetOnlyLevel(1);
        }
    }

    private void SetOnlyLevel(int level)
    {
        if (level1 != null) level1.SetActive(level == 1);
        if (level2 != null) level2.SetActive(level == 2);
        if (level3 != null) level3.SetActive(level == 3);
    }

    private void PlayMatchaSound()
    {
        if (matchaAudioSource != null && matchaSound != null)
        {
            if (!matchaAudioSource.isPlaying)
            {
                matchaAudioSource.clip = matchaSound;
                matchaAudioSource.loop = true;
                matchaAudioSource.Play();
            }
        }
    }

    private void StopMatchaSound()
    {
        if (matchaAudioSource != null && matchaAudioSource.isPlaying)
        {
            matchaAudioSource.Stop();
        }
    }
}