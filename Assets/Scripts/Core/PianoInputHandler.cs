using UnityEngine;
using UnityEngine.UI;
using System.Collections;



    public class PianoInputHandler : MonoBehaviour
    {
        [Header("Настройки звука")]
        public bool enableSound = true;
        [Range(0f, 1f)] public float volume = 0.8f;
        
        [Header("Цвета клавиш")]
        public Color pressedKeyColor = Color.yellow;
        public Color correctKeyColor = Color.green;
        public Color incorrectKeyColor = Color.red;
        
        [Header("Временные настройки")]
        [SerializeField] private float keyFlashDuration = 0.2f;
        [SerializeField] private float resultDisplayDuration = 1f;
        
        /// <summary>
        /// Обработка нажатия клавиши (звук + временная подсветка)
        /// </summary>
        public void ProcessKeyPress(string pressedNote, GameObject pressedKey)
        {
            Debug.Log($"[InputHandler] Processing key: {pressedNote}");
            
            // Воспроизводим звук
            PlayKeySound(pressedKey);
            
            // Запускаем мигание клавиши
            StartCoroutine(FlashKey(pressedKey, pressedKeyColor));
        }
        
        /// <summary>
        /// Установка финального цвета после проверки
        /// </summary>
        public void SetKeyFinalColor(GameObject keyObject, bool isCorrect)
        {
            StartCoroutine(ShowResultAndReset(keyObject, isCorrect));
        }
        
        private IEnumerator FlashKey(GameObject keyObject, Color flashColor)
        {
            Image keyImage = keyObject.GetComponent<Image>();
            if (keyImage == null) yield break;
            
            Color originalColor = keyImage.color;
            keyImage.color = flashColor;
            
            yield return new WaitForSeconds(keyFlashDuration);
            
            keyImage.color = originalColor;
        }
        
        private IEnumerator ShowResultAndReset(GameObject keyObject, bool isCorrect)
        {
            // Ждем окончания первого мигания
            yield return new WaitForSeconds(keyFlashDuration + 0.05f);
            
            Image keyImage = keyObject.GetComponent<Image>();
            if (keyImage == null) yield break;
            
            // Устанавливаем цвет результата
            Color resultColor = isCorrect ? correctKeyColor : incorrectKeyColor;
            keyImage.color = resultColor;
            
            // Ждем перед сбросом
            yield return new WaitForSeconds(resultDisplayDuration);
            
            // Возвращаем исходный цвет
            ResetKeyColor(keyObject);
        }
        
        private void ResetKeyColor(GameObject keyObject)
        {
            Image keyImage = keyObject.GetComponent<Image>();
            if (keyImage == null) return;
            
            // Определяем исходный цвет по имени клавиши
            bool isBlackKey = keyObject.name.Contains("#") || 
                              keyObject.name.Contains("sharp") || 
                              keyObject.name.Contains("flat") ||
                              keyObject.name.Contains("b");
            
            keyImage.color = isBlackKey ? Color.black : Color.white;
        }
        
        private void PlayKeySound(GameObject pressedKey)
        {
            if (!enableSound) return;
            
            AudioSource audioSource = pressedKey.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.volume = volume;
                audioSource.Play();
                Debug.Log($"[InputHandler] Played sound for {pressedKey.name}");
            }
            else
            {
                Debug.LogWarning($"No AudioSource found on {pressedKey.name}");
            }
        }
        
        /// <summary>
        /// Вспомогательный метод для сброса всех клавиш
        /// </summary>
        public void ResetAllKeyColors(Color resetColor)
        {
            GameObject[] keys = GameObject.FindGameObjectsWithTag("PianoKey");
            foreach (GameObject key in keys)
            {
                Image keyImage = key.GetComponent<Image>();
                if (keyImage != null)
                {
                    keyImage.color = resetColor;
                }
            }
        }
    }
