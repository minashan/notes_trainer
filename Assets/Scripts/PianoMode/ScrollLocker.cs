using UnityEngine;
using UnityEngine.UI;

public class ScrollLocker : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Button lockButton;
    [SerializeField] private Sprite unlockedSprite;
    [SerializeField] private Sprite lockedSprite;
    
    private bool isLocked = false;
    
    void Start()
    {
        if (scrollRect == null)
            scrollRect = GetComponent<ScrollRect>();
        
        lockButton.onClick.AddListener(ToggleLock);
        UpdateButtonIcon();
    }
    
    void ToggleLock()
    {
        isLocked = !isLocked;
        scrollRect.enabled = !isLocked;
        UpdateButtonIcon();
    }
    
    void UpdateButtonIcon()
    {
        if (lockButton.image != null)
        {
            lockButton.image.sprite = isLocked ? lockedSprite : unlockedSprite;
        }
    }
}