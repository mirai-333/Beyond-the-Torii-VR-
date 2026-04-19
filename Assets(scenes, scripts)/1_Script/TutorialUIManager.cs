using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialUIManager : MonoBehaviour
{

    public Transform xrOrigin;

    public GameObject startButtonObject;

    [Header("Sound")]
    public AudioSource introAudioSource;
    public AudioClip taikoClip1;
    public AudioClip taikoClip2;

    [Header("Intro Board")]
    public RectTransform introBoard;
    public float introDropDuration = 0.5f;
    public float introStayDuration = 5f;
    public float introHideDuration = 0.4f;

    public Vector3 introHiddenLocalPos = new Vector3(0f, 3f, 0f);
    public Vector3 introShownLocalPos = new Vector3(0f, 0f, 0f);

    public GameObject moveObject;

    [Header("Event 1")]
    public GameObject arrowObjectEvent1;

    [Header("Event 2")]
    public GameObject arrowObjectEvent2;

    [Header("Event 3")]
    public GameObject arrowObjectEvent3;

    [Header("Cafe Enter Button")]
    public GameObject btnEnter;
    public string nextSceneName = "cafe";
    public float enterButtonDelay = 0.5f;

    [Header("Fusuma Enter Button")]
    public GameObject doorEnterButton;
    public FusumaDoorController fusumaDoorController;

    [Header("How To Make")]
    public GameObject howToMakeButton;
    public GameObject howToMakeBoard;
    public Image howToMakeButtonImage;
    public Sprite howToMakeNormalSprite;
    public Sprite howToMakeActiveSprite;

    private bool isHowToMakeOpen = false;

    [Header("Event 4 Arrow")]
    public GameObject arrowObjectEvent4;

    [Header("Kimono Enter Button")]
    public GameObject kimonoEnterButton;
    public string kimonoSceneName = "kimono";

    [Header("Return Position")]
    public Transform goal2Point;
    public Transform goal4Point;

    private bool isPlaying = false;

   void Start()
    {
        if (introBoard != null)
        {
            introBoard.gameObject.SetActive(false);
            introBoard.localPosition = introHiddenLocalPos;
        }

        if (btnEnter != null) btnEnter.SetActive(false);
        if (doorEnterButton != null) doorEnterButton.SetActive(false);
        if (kimonoEnterButton != null) kimonoEnterButton.SetActive(false);

        if (howToMakeBoard != null) howToMakeBoard.SetActive(false);

        if (howToMakeButtonImage != null && howToMakeNormalSprite != null)
            howToMakeButtonImage.sprite = howToMakeNormalSprite;

        SetArrowUI(false, arrowObjectEvent1);
        SetArrowUI(false, arrowObjectEvent2);
        SetArrowUI(false, arrowObjectEvent3);
        SetArrowUI(false, arrowObjectEvent4);

        if (GameProgress.ReturnedFromKimono)
        {
            GameProgress.ReturnedFromKimono = false;

            if (startButtonObject != null)
                startButtonObject.SetActive(false);

            if (moveObject != null)
                moveObject.SetActive(true);

            isPlaying = true;

            if (goal4Point != null)
            {
                xrOrigin.position = goal4Point.position;
                xrOrigin.rotation = goal4Point.rotation;
            }

            return;
        }

        if (GameProgress.ReturnedFromCafe)
        {
            GameProgress.ReturnedFromCafe = false;

            if (startButtonObject != null)
                startButtonObject.SetActive(false);

            if (moveObject != null)
                moveObject.SetActive(true);

            isPlaying = true;

            if (goal2Point != null)
            {
                xrOrigin.position = goal2Point.position;
                xrOrigin.rotation = goal2Point.rotation;
            }

            StartNextFlowAfterCafe();
            return;
        }

        if (startButtonObject != null)
            startButtonObject.SetActive(true);

        if (moveObject != null)
            moveObject.SetActive(false);
    }

    private void StartNextFlowAfterCafe()
    {
        ShowEvent3Guide();
    }

    public void OnClickStart()
    {
        if (isPlaying) return;
        isPlaying = true;

        if (startButtonObject != null)
            startButtonObject.SetActive(false);

        if (moveObject != null)
            moveObject.SetActive(true);

         SoundManager.Instance.PlayKoto1();

        StartCoroutine(IntroSequence());
    }

    private IEnumerator IntroSequence()
    {
        introBoard.gameObject.SetActive(true);
        introBoard.localPosition = introHiddenLocalPos;

        if (introAudioSource != null && taikoClip1 != null)
        {
            introAudioSource.PlayOneShot(taikoClip1);
        }

        yield return StartCoroutine(BounceMove(
            introBoard,
            introHiddenLocalPos,
            introShownLocalPos,
            introDropDuration
        ));

        yield return new WaitForSeconds(introStayDuration);

        yield return StartCoroutine(SmoothMove(
            introBoard,
            introShownLocalPos,
            introHiddenLocalPos,
            introHideDuration
        ));

        introBoard.gameObject.SetActive(false);

        ShowEvent1Guide();
    }

    public void ShowEvent1Guide()
    {
        SetArrowUI(true, arrowObjectEvent1);
    }

    public void CompleteEvent1()
    {
        SetArrowUI(false, arrowObjectEvent1);
    }

    public void ShowEvent2Guide()
    {
        SetArrowUI(true, arrowObjectEvent2);
    }

    public void CompleteEvent2()
    {
        SetArrowUI(false, arrowObjectEvent2);
        StartCoroutine(ShowEnterButtonDelayed());
    }

    public void ShowEvent3Guide()
    {
        SetArrowUI(true, arrowObjectEvent3);
    }

    public void CompleteEvent3()
    {
        SetArrowUI(false, arrowObjectEvent3);
        if (doorEnterButton != null)
            doorEnterButton.SetActive(true);
        
    }

    public void ShowEvent4Guide()
    {
        SetArrowUI(true, arrowObjectEvent4);
    }

    public void CompleteEvent4()
    {
        SetArrowUI(false, arrowObjectEvent4);

        if (kimonoEnterButton != null)
            kimonoEnterButton.SetActive(true);
    }
    private IEnumerator ShowEnterButtonDelayed()
    {
        yield return new WaitForSeconds(enterButtonDelay);

        if (btnEnter != null)
            btnEnter.SetActive(true);
    }

    public void OnClickEnter()
    {
        SoundManager.Instance.PlayKoto1(); 
        SceneManager.LoadScene(nextSceneName);
    }

    public void OnClickDoorEnter()
    {
        SoundManager.Instance.PlayKoto1();

        if (doorEnterButton != null)
            doorEnterButton.SetActive(false);

        if (fusumaDoorController != null)
            fusumaDoorController.OpenDoors();
    }

    public void OnClickHowToMake()
    {
        isHowToMakeOpen = !isHowToMakeOpen;
        SoundManager.Instance.PlayTaiko1();

        if (howToMakeBoard != null)
            howToMakeBoard.SetActive(isHowToMakeOpen);

        if (howToMakeButtonImage != null)
        {
            if (isHowToMakeOpen && howToMakeActiveSprite != null)
                howToMakeButtonImage.sprite = howToMakeActiveSprite;
            else if (!isHowToMakeOpen && howToMakeNormalSprite != null)
                howToMakeButtonImage.sprite = howToMakeNormalSprite;
        }
    }

    public void OnClickKimonoEnter()
    {
        SoundManager.Instance.PlayKoto1();

        if (kimonoEnterButton != null)
            kimonoEnterButton.SetActive(false);

        SceneManager.LoadScene("kimono");
    }


    private void SetArrowUI(bool active, GameObject arrowObject)
    {
        if (arrowObject != null) arrowObject.SetActive(active);
    }

    private IEnumerator SmoothMove(RectTransform target, Vector3 from, Vector3 to, float duration)
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / duration);
            target.localPosition = Vector3.Lerp(from, to, p);
            yield return null;
        }

        target.localPosition = to;
    }

    private IEnumerator BounceMove(RectTransform target, Vector3 from, Vector3 to, float duration)
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / duration);

            float s = 1.70158f;
            float x = p - 1f;
            float eased = 1f + (s + 1f) * x * x * x + s * x * x;

            target.localPosition = Vector3.LerpUnclamped(from, to, eased);
            yield return null;
        }

        target.localPosition = to;
    }
}