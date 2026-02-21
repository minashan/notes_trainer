using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    [SerializeField] private Sprite unmutedSprite;
    [SerializeField] private Sprite mutedSprite;
    
    private Image buttonImage;
    private Button button;
    
    void Start()
    {
        buttonImage = GetComponent<Image>();
        button = GetComponent<Button>();
        
        UpdateVisual();
        button.onClick.AddListener(ToggleMute);
    }
    
    void ToggleMute()
    {
        AudioManager.Instance.ToggleMute();
        UpdateVisual();
    }
    
    void UpdateVisual()
    {
        if (AudioManager.Instance.IsMuted)
        {
            buttonImage.sprite = mutedSprite;
            buttonImage.color = new Color(0.7f, 0.7f, 0.7f); // тусклее
        }
        else
        {
            buttonImage.sprite = unmutedSprite;
            buttonImage.color = Color.white; // яркий
        }
    }
}