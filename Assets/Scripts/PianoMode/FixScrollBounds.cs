using UnityEngine;
using UnityEngine.UI;

public class FixScrollBounds : MonoBehaviour
{
    private ScrollRect scrollRect;
    
    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
    }
    
    void Update()
    {
        if (scrollRect.content != null && scrollRect.content.rect.width > 0)
        {
            scrollRect.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scrollRect.content.rect.width);
        }
    }
}