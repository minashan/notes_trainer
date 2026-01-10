using UnityEngine;
using TMPro;
using System.Collections;


    public class UIManager : MonoBehaviour
    {
        [Header("Текстовые элементы")]
        [SerializeField] private TextMeshProUGUI feedbackText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI streakText;
        
        [Header("Настройки анимаций")]
        [SerializeField] private float feedbackFadeDuration = 0.5f;
        [SerializeField] private float feedbackDisplayDuration = 1.5f;
        
        [Header("Цвета")]
        [SerializeField] private Color correctColor = new Color(0f, 0.5f, 0f, 1f); // темно-зеленый
        [SerializeField] private Color incorrectColor = Color.red;
        [SerializeField] private Color defaultTextColor = Color.white;
        
        private Coroutine _currentFeedbackCoroutine;
        
        /// <summary>
        /// Показать обратную связь (правильно/неправильно)
        /// </summary>
        public void ShowFeedback(string message, bool isCorrect)
{
    Debug.Log($"[UIManager] ShowFeedback: {message}, correct: {isCorrect}");
    
    if (_currentFeedbackCoroutine != null)
    {
        StopCoroutine(_currentFeedbackCoroutine);
    }
    
    // ПРОСТО передаем сообщение как есть (это название ноты)
    _currentFeedbackCoroutine = StartCoroutine(ShowFeedbackCoroutine(message, isCorrect));
}
        
        /// <summary>
        /// Показать название ноты (без оценки правильности)
        /// </summary>
        public void ShowNoteName(string noteName, Color color)
        {
            if (feedbackText == null) return;
            
            feedbackText.text = noteName;
            feedbackText.color = color;
            feedbackText.gameObject.SetActive(true);
        }
        
        /// <summary>
        /// Очистить обратную связь
        /// </summary>
        public void ClearFeedback()
        {
            if (feedbackText == null) return;
            
            feedbackText.text = "";
            feedbackText.gameObject.SetActive(false);
        }
        
        /// <summary>
        /// Обновить счет
        /// </summary>
        public void UpdateScore(int score, int streak)
        {
            if (scoreText != null)
                scoreText.text = $"Счет: {score}";
            
            if (streakText != null)
                streakText.text = $"Серия: {streak}";
        }
        
        /// <summary>
        /// Корутина для показа обратной связи с анимацией
        /// </summary>
        private IEnumerator ShowFeedbackCoroutine(string message, bool isCorrect)
{
    if (feedbackText == null) yield break;
    
    // Устанавливаем текст и цвет
    feedbackText.text = message;
    feedbackText.color = isCorrect ? correctColor : incorrectColor;
    feedbackText.gameObject.SetActive(true);
    
    // Ждем указанное время (ИСПОЛЬЗУЕМ feedbackDisplayDuration)
    yield return new WaitForSeconds(feedbackDisplayDuration);
    
    // Плавно исчезаем (ИСПОЛЬЗУЕМ feedbackFadeDuration)
    float elapsedTime = 0f;
    Color startColor = feedbackText.color;
    Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
    
    while (elapsedTime < feedbackFadeDuration)
    {
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / feedbackFadeDuration;
        feedbackText.color = Color.Lerp(startColor, targetColor, t);
        yield return null;
    }
    
    // Очищаем
    ClearFeedback();
    _currentFeedbackCoroutine = null;
}
        
        /// <summary>
        /// Инициализация (можно вызывать из GameManager)
        /// </summary>
        public void Initialize(TextMeshProUGUI feedback, TextMeshProUGUI score, TextMeshProUGUI streak)
        {
            feedbackText = feedback;
            scoreText = score;
            streakText = streak;
            
            ClearFeedback(); // Начинаем с чистого состояния
            
            Debug.Log("[UIManager] Initialized");
        }
    }
