using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
// add list
using System.Collections.Generic;
public class FishingBagButtonHandler : MonoBehaviour
{
    public Button fishingBagButton;
    public GameObject fishingBagListScrollView;
    public Button closeButton;

    void Start()
    {
        // 初始时隐藏 Fishing Bag 列表的滚动视图
        fishingBagListScrollView.SetActive(false);

        // 为按钮添加点击事件监听器
        fishingBagButton.onClick.AddListener(ShowFishingBagList);
        // closeButton.onClick.AddListener(HideFishingBagList);
    }

    void Update()
    {
        if (fishingBagListScrollView.activeSelf && Input.GetMouseButtonDown(0))
        {
            // 检查点击是否在 Scroll View 之外
            if (!IsPointerOverUIObject())
            {
                HideFishingBagList();
            }
        }
    }

    void ShowFishingBagList()
    {
        // 显示 Fishing Bag 列表的滚动视图
        fishingBagListScrollView.SetActive(true);
    }

    void HideFishingBagList()
    {
        // 隐藏 Fishing Bag 列表的滚动视图
        fishingBagListScrollView.SetActive(false);
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
