using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CafeFlowManager : MonoBehaviour
{
    public enum CafeStep
    {
        GoToSeat,
        SitDown,
        Seated,
        WaitStaff,
        ChooseFood,
        WaitFood,
        FoodServed,
        GoToExit,
        ExitReady,
        Done
    }

    [Header("State")]
    public CafeStep currentStep = CafeStep.GoToSeat;

    [Header("References")]
    public Transform xrOrigin;
    public Transform seatPoint;
    public GameObject seatArrow;
    public GameObject sitButton;
    public GameObject moveObject;

    [Header("Staff")]
    public GameObject staffNPC;
    public Transform staffStopPoint;
    public float staffMoveSpeed = 1.2f;
    public GameObject staffCanvas;

    [Header("Menu")]
    public GameObject tableMenu;
    public GameObject orderButtonsRoot;

    [Header("Food")]
    public Transform foodSpawnPoint;
    public GameObject tunaPrefab;
    public GameObject omeletPrefab;
    public GameObject ikuraPrefab;
    public float foodServeDelay = 2f;

    [Header("Result")]
    public GameObject currentFoodInstance;

    [Header("Exit")]
    public GameObject exitArrow;
    public GameObject exitButton;
    public string returnSceneName = "Main";

    public void Start()
    {
        if (seatArrow != null) seatArrow.SetActive(true);
        if (sitButton != null) sitButton.SetActive(false);

        if (staffCanvas != null) staffCanvas.SetActive(false);
        if (tableMenu != null) tableMenu.SetActive(false);
        if (orderButtonsRoot != null) orderButtonsRoot.SetActive(false);

        if (exitArrow != null) exitArrow.SetActive(false);
        if (exitButton != null) exitButton.SetActive(false);
    }

    public void OnReachSeatArea()
    {
        if (currentStep != CafeStep.GoToSeat) return;

        currentStep = CafeStep.SitDown;

        if (seatArrow != null) seatArrow.SetActive(false);
        if (sitButton != null) sitButton.SetActive(true);
    }

    private Vector3 savedStandPosition;
    private Quaternion savedStandRotation;

    public void OnClickSit()
    {
        if (currentStep != CafeStep.SitDown) return;
        if (xrOrigin == null || seatPoint == null) return;

        savedStandPosition = xrOrigin.position;
        savedStandRotation = xrOrigin.rotation;

        xrOrigin.position = seatPoint.position;
        xrOrigin.rotation = seatPoint.rotation;

        SoundManager.Instance.PlayTaiko1();

        if (sitButton != null) sitButton.SetActive(false);
        if (moveObject != null) moveObject.SetActive(false);

        currentStep = CafeStep.Seated;

        StartCoroutine(StaffSequence());
    }

    private IEnumerator StaffSequence()
    {
        currentStep = CafeStep.WaitStaff;

        if (staffNPC != null && staffStopPoint != null)
        {
            while (Vector3.Distance(staffNPC.transform.position, staffStopPoint.position) > 0.05f)
            {
                staffNPC.transform.position = Vector3.MoveTowards(
                    staffNPC.transform.position,
                    staffStopPoint.position,
                    staffMoveSpeed * Time.deltaTime
                );

                yield return null;
            }

            staffNPC.transform.position = staffStopPoint.position;
        }

        if (staffCanvas != null)
            staffCanvas.SetActive(true);
            SoundManager.Instance.PlayAppear();

        if (tableMenu != null)
            tableMenu.SetActive(true);

        currentStep = CafeStep.ChooseFood;
    }

    public void OnMenuPressed()
    {
        if (currentStep != CafeStep.ChooseFood) return;

        SoundManager.Instance.PlayPaper();

        if (orderButtonsRoot != null)
            orderButtonsRoot.SetActive(true);
    }

    public void OnChooseFood(string foodId)
    {
        if (currentStep != CafeStep.ChooseFood) return;

        SoundManager.Instance.PlayTaiko1();

        if (orderButtonsRoot != null)
            orderButtonsRoot.SetActive(false);

        if (staffCanvas != null)
            staffCanvas.SetActive(false);

        if (tableMenu != null)
            tableMenu.SetActive(false);

        currentStep = CafeStep.WaitFood;
        StartCoroutine(ServeFoodRoutine(foodId));
    }

    private IEnumerator ServeFoodRoutine(string foodId)
    {
        yield return new WaitForSeconds(foodServeDelay);

        GameObject prefabToSpawn = null;

        if (foodId == "Tuna") prefabToSpawn = tunaPrefab;
        else if (foodId == "Omelet") prefabToSpawn = omeletPrefab;
        else if (foodId == "Ikura") prefabToSpawn = ikuraPrefab;

        if (prefabToSpawn != null && foodSpawnPoint != null)
        {
            currentFoodInstance = Instantiate(
                prefabToSpawn,
                foodSpawnPoint.position,
                foodSpawnPoint.rotation
            );
            SoundManager.Instance.PlayTaiko2();
        }

        currentStep = CafeStep.FoodServed;
    }

    public void OnFoodEaten(GameObject eatenObject)
    {
        if (currentStep != CafeStep.FoodServed) return;

        StartCoroutine(FoodEatenSequence(eatenObject));
    }

    private IEnumerator FoodEatenSequence(GameObject eatenObject)
    {
        SoundManager.Instance.PlayEat();

        if (eatenObject != null)
        {
            Destroy(eatenObject);
        }
        else if (currentFoodInstance != null)
        {
            Destroy(currentFoodInstance);
        }

        currentFoodInstance = null;

        yield return new WaitForSeconds(1f);

        xrOrigin.position = savedStandPosition;
        xrOrigin.rotation = savedStandRotation;

        currentStep = CafeStep.GoToExit;

        if (moveObject != null) moveObject.SetActive(true);

        if (exitArrow != null)
            exitArrow.SetActive(true);
    }

    public void OnReachExitArea()
    {
        if (currentStep != CafeStep.GoToExit) return;

        currentStep = CafeStep.ExitReady;

        if (exitArrow != null)
            exitArrow.SetActive(false);

        if (exitButton != null)
            exitButton.SetActive(true);
    }

    public void OnClickExit()
    {
        if (currentStep != CafeStep.ExitReady) return;

        SoundManager.Instance.PlayKoto1();

        currentStep = CafeStep.Done;

        GameProgress.ReturnedFromCafe = true;

        SceneManager.LoadScene(returnSceneName);
    }
}