using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private AudioSource pianoSource;
    
    [Header("SETTINGS")]
    private const string MUTED_PREF_KEY = "Muted";
    
    public static AudioManager Instance { get; private set; }
    public bool IsMuted { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Load mute state
            IsMuted = PlayerPrefs.GetInt(MUTED_PREF_KEY, 0) == 1;
            ApplyMute();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void ToggleMute()
    {
        IsMuted = !IsMuted;
        PlayerPrefs.SetInt(MUTED_PREF_KEY, IsMuted ? 1 : 0);
        PlayerPrefs.Save();
        
        ApplyMute();
    }
    
    private void ApplyMute()
    {
        if (pianoSource != null)
        {
            pianoSource.mute = IsMuted;
        }
    }
    
    public void PlayPianoNote(AudioClip clip)
    {
        if (IsMuted || pianoSource == null || clip == null) return;
        
        pianoSource.PlayOneShot(clip);
    }
}