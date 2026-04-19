using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class LadleWaterController : MonoBehaviour
{
    [Header("reference")]
    public GameObject waterVisual;
    public Transform pourPoint;
    public GameObject waterDropPrefab;
    public ChozuyaTutorialManager tutorialManager;
    public XRGrabInteractable grabInteractable;

    [Header("Audio Sources")]
    public AudioSource seAudioSource;
    public AudioSource pourLoopAudioSource;

    [Header("Audios")]
    public AudioClip gripSound;
    public AudioClip fillWaterSound;
    public AudioClip waterSound;

    [Header("State")]
    public bool hasWater = false;

    [Header("Pour Settings")]
    public float pourDuration = 1.0f;
    public float dropInterval = 0.05f;
    public int dropsPerBurst = 3;

    private bool isPouring = false;

    void Awake()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrabbed);
        }
    }

    void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        }
    }

    void Start()
    {
        if (waterVisual != null)
            waterVisual.SetActive(false);

        if (pourLoopAudioSource != null)
        {
            pourLoopAudioSource.loop = true;
            pourLoopAudioSource.playOnAwake = false;

            if (waterSound != null)
                pourLoopAudioSource.clip = waterSound;
        }
    }

    void Update()
    {
        CheckPour();
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        if (seAudioSource != null && gripSound != null)
        {
            seAudioSource.PlayOneShot(gripSound);
        }
    }

    private void CheckPour()
    {
        if (!hasWater) return;
        if (isPouring) return;

        float sidewaysTilt = Mathf.Abs(Vector3.Dot(transform.forward, Vector3.up));

        if (sidewaysTilt > 0.7f)
        {
            StartCoroutine(PourRoutine());
        }
    }

    public void FillWater()
    {
        if (hasWater) return;

        hasWater = true;

        if (waterVisual != null)
            waterVisual.SetActive(true);

        if (seAudioSource != null && fillWaterSound != null)
        {
            seAudioSource.PlayOneShot(fillWaterSound);
        }

        if (tutorialManager != null)
            tutorialManager.OnWaterScooped();
    }

    private IEnumerator PourRoutine()
    {
        isPouring = true;

        if (pourLoopAudioSource != null && waterSound != null)
        {
            if (!pourLoopAudioSource.isPlaying)
                pourLoopAudioSource.Play();
        }

        float timer = 0f;

        while (timer < pourDuration)
        {
            SpawnDrops();
            yield return new WaitForSeconds(dropInterval);
            timer += dropInterval;
        }

        hasWater = false;

        if (waterVisual != null)
            waterVisual.SetActive(false);

        if (pourLoopAudioSource != null && pourLoopAudioSource.isPlaying)
        {
            pourLoopAudioSource.Stop();
        }

        isPouring = false;
    }

    private void SpawnDrops()
    {
        if (waterDropPrefab == null || pourPoint == null) return;

        for (int i = 0; i < dropsPerBurst; i++)
        {
            Vector3 spread =
                pourPoint.right * Random.Range(-0.03f, 0.03f) +
                pourPoint.forward * Random.Range(-0.01f, 0.01f);

            GameObject drop = Instantiate(
                waterDropPrefab,
                pourPoint.position + spread,
                Quaternion.identity
            );

            Rigidbody rb = drop.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.down * 2.0f;
            }
        }
    }
}