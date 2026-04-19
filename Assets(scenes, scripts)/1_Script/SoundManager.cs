using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource audioSource;

    [Header("common sound")]
    public AudioClip taiko1;
    public AudioClip taiko2;

    [Header("Buttonsounds")]
    public AudioClip koto1;

    [Header("Others")]
    public AudioClip paper;
    public AudioClip appear;
    public AudioClip eat;
    public AudioClip matcha;
    public AudioClip kimonoGrab;
    public AudioClip kimonoWear;

    public void PlayKoto1()
    {
        audioSource.PlayOneShot(koto1);
    }

    public void PlayAppear()
    {
        audioSource.PlayOneShot(appear);
    }

    public void PlayEat()
    {
        audioSource.PlayOneShot(eat);
    }

    public void PlayMatcha()
    {
        audioSource.PlayOneShot(matcha);
    }


    public void PlayPaper()
    {
        audioSource.PlayOneShot(paper);
    }


    public void PlayKimonoGrab()
    {
        audioSource.PlayOneShot(kimonoGrab);
    }


    public void PlayKimonoWear()
    {
        audioSource.PlayOneShot(kimonoWear);
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayTaiko1()
    {
        audioSource.PlayOneShot(taiko1);
    }

    public void PlayTaiko2()
    {
        audioSource.PlayOneShot(taiko2);
    }

    public void PlayRandomTaiko()
    {
        AudioClip clip = Random.value > 0.5f ? taiko1 : taiko2;
        audioSource.PlayOneShot(clip);
    }
}