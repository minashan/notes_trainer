using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollSync : MonoBehaviour
{
    [SerializeField] private RectTransform secondContent; // Content2 (чёрные)
    [SerializeField] private Vector2 offset = new Vector2(0, 0); // ← добавили смещение
    
    private ScrollRect scrollRect;
    private RectTransform firstContent;
    
    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        firstContent = scrollRect.content;
        
        scrollRect.onValueChanged.AddListener(SyncScroll);
        SyncScroll(scrollRect.normalizedPosition);
    }
    
    void SyncScroll(Vector2 pos)
    {
        if (secondContent != null)
        {
            secondContent.anchoredPosition = firstContent.anchoredPosition + offset;
        }
    }
}