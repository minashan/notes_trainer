using UnityEngine;

public class LoadingProgressBar : MonoBehaviour
{
    [Header("Progress Bar")]
    [SerializeField] private RectTransform progressBarFill;
    [SerializeField] private float loadingTime = 2f;
    
    private float _timer;
    
    private void Update()
    {
        if (_timer >= loadingTime) return;
        
        _timer += Time.deltaTime;
        float progress = Mathf.Clamp01(_timer / loadingTime);
        
        Vector2 anchorMin = progressBarFill.anchorMin;
        progressBarFill.anchorMax = new Vector2(anchorMin.x + progress, anchorMin.y);
        
        if (progress >= 1f)
        {
            CompleteLoading();
        }
    }
    
    private void CompleteLoading()
    {
        SceneNavigator.Instance?.LoadKeySelection();
    }
}