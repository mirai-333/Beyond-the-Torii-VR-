using UnityEngine;

public class FoodButtonHandler : MonoBehaviour
{
    public CafeFlowManager cafeFlowManager;
    public string foodId = "Tuna";

    public void OnClickFoodButton()
    {
        if (cafeFlowManager != null)
            cafeFlowManager.OnChooseFood(foodId);
    }
}