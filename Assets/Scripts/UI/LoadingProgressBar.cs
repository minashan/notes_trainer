using UnityEngine;
using UnityEngine.UI;

public class LoadingProgressBar : MonoBehaviour
{
    [SerializeField] private RectTransform progressBarFill;
    [SerializeField] private float loadingTime = 2f;
    
    private float timer = 0f;
    
    void Update()
    {
        if (timer >= loadingTime) return;
        
        timer += Time.deltaTime;
        float progress = Mathf.Clamp01(timer / loadingTime);
        
        // Меняем anchorMax по X
        Vector2 anchorMin = progressBarFill.anchorMin;
        progressBarFill.anchorMax = new Vector2(anchorMin.x + progress, anchorMin.y);
        
        if (progress >= 1f)
        {
            CompleteLoading();
        }
    }
    
    void CompleteLoading()
    {
        Debug.Log("Загрузка завершена!");
        SceneNavigator.Instance.LoadKeySelection();
    }
}