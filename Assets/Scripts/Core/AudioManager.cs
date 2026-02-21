using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [SerializeField] private AudioSource pianoSource;
    
    private bool _isMuted = false;
    public bool IsMuted => _isMuted;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;
            ApplyMute();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void ToggleMute()
    {
        _isMuted = !_isMuted;
        PlayerPrefs.SetInt("Muted", _isMuted ? 1 : 0);
        PlayerPrefs.Save();
        ApplyMute();
    }
    
    private void ApplyMute()
    {
        if (pianoSource != null)
            pianoSource.mute = _isMuted;
    }
    
    public void PlayPianoNote(AudioClip clip)
    {
        if (!_isMuted && pianoSource != null && clip != null)
        {
            pianoSource.PlayOneShot(clip);
        }
    }
}