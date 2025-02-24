using UnityEngine;

public class TestFishingBag : MonoBehaviour
{
    public FishingBagManager bagManager;

    public void OnTestButtonClick()
    {
        bagManager.PurchaseAndOpenBag();
    }
}
