using UnityEngine;
using UnityEngine.UI;

public class KeyboardZoom : MonoBehaviour
{
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 1.5f;
    [SerializeField] private float zoomStep = 0.1f;
    
    private RectTransform rectTransform;
    private float currentScale = 1f;
    private float originalWidth;
    
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        currentScale = rectTransform.localScale.x;
        originalWidth = rectTransform.rect.width;
    }
    
    public void ZoomIn()
    {
        currentScale = Mathf.Clamp(currentScale + zoomStep, minScale, maxScale);
        rectTransform.localScale = new Vector3(currentScale, 1f, 1f);
    }
    
    public void ZoomOut()
    {
        float newScale = currentScale - zoomStep;
        float newWidth = originalWidth * newScale;
        
        // Не даём стать меньше родителя (например, Viewport)
        if (newWidth >= rectTransform.parent.GetComponent<RectTransform>().rect.width)
        {
            currentScale = newScale;
            rectTransform.localScale = new Vector3(currentScale, 1f, 1f);
        }
    }
}