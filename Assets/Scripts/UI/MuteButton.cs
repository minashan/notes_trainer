using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite unmutedSprite;
    [SerializeField] private Sprite mutedSprite;
    
    [Header("Visual Settings")]
    [SerializeField] private Color mutedColor = new(0.7f, 0.7f, 0.7f);
    [SerializeField] private Color unmutedColor = Color.white;
    
    private Image _buttonImage;
    private Button _button;
    
    private void Start()
    {
        _buttonImage = GetComponent<Image>();
        _button = GetComponent<Button>();
        
        UpdateVisual();
        _button.onClick.AddListener(ToggleMute);
    }
    
    private void ToggleMute()
    {
        AudioManager.Instance?.ToggleMute();
        UpdateVisual();
    }
    
    private void UpdateVisual()
    {
        if (AudioManager.Instance == null) return;
        
        bool isMuted = AudioManager.Instance.IsMuted;
        _buttonImage.sprite = isMuted ? mutedSprite : unmutedSprite;
        _buttonImage.color = isMuted ? mutedColor : unmutedColor;
    }
}