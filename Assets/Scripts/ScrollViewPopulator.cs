using UnityEngine;
using UnityEngine.UI;

public class ScrollViewPopulator : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform content;
    public GameObject fishItemPrefab;

    void Start()
    {
        // 设置滚动方向为垂直
        scrollRect.horizontal = true;
        scrollRect.vertical = false;
        if (scrollRect.horizontalScrollbar != null)
        {
            scrollRect.horizontalScrollbar.gameObject.SetActive(false);
        }
        if (scrollRect.verticalScrollbar != null)
        {
            scrollRect.verticalScrollbar.gameObject.SetActive(false);
        }
        // 添加三个 Fish Item
        for (int i = 0; i < 3; i++)
        {
            GameObject fishItem = Instantiate(fishItemPrefab, content);
            // 可以在这里设置每个 Fish Item 的具体信息，例如名称
            Text itemText = fishItem.GetComponentInChildren<Text>();
            if (itemText != null)
            {
                itemText.text = "Fish " + (i + 1);
            }
        }
    }
}
