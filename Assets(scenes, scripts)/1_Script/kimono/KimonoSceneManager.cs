using UnityEngine;
using UnityEngine.SceneManagement;

public class KimonoSceneManager : MonoBehaviour
{
    public string returnSceneName = "Main";

    public void OnClickExitinKimono()
    {
        SoundManager.Instance.PlayKoto1();
        GameProgress.ReturnedFromKimono = true;
        SceneManager.LoadScene(returnSceneName);
    }
}