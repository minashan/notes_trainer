using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class NoteEvent
{
    public string noteName;
    public float startTime;   // момент нажатия
    public float endTime;     // момент отпускания
}

public class PianoRecorder : MonoBehaviour
{
    [Header("Кнопки")]
    [SerializeField] private Button recordButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button playlistButton;
    
    [Header("Sprites для REC")]
    [SerializeField] private Sprite recordSprite;
    [SerializeField] private Sprite stopSprite;
    
    [Header("Sprites для PLAY")]
    [SerializeField] private Sprite playSprite;
    [SerializeField] private Sprite pauseSprite;
    
    private Image recordButtonImage;
    private Image playButtonImage;
    private bool isRecording = false;
    private bool isPlaying = false;
    private List<NoteEvent> currentRecording = new List<NoteEvent>();
    private float recordStartTime;
    private Coroutine playbackCoroutine;
    
    void Start()
    {
        recordButtonImage = recordButton.GetComponent<Image>();
        playButtonImage = playButton.GetComponent<Image>();
        
        recordButton.onClick.AddListener(ToggleRecording);
        playButton.onClick.AddListener(TogglePlayback);
        playlistButton.onClick.AddListener(OpenPlaylist);
    }
    
    void ToggleRecording()
    {
        isRecording = !isRecording;
        
        if (isRecording)
        {
            currentRecording.Clear();
            recordStartTime = Time.time;
            recordButtonImage.sprite = stopSprite;
            Debug.Log("Запись начата");
        }
        else
        {
            recordButtonImage.sprite = recordSprite;
            Debug.Log($"Запись остановлена. Записано {currentRecording.Count} нот");
        }
    }
    
    void TogglePlayback()
    {
        if (isPlaying)
        {
            // Останавливаем воспроизведение
            if (playbackCoroutine != null)
                StopCoroutine(playbackCoroutine);
            isPlaying = false;
            playButtonImage.sprite = playSprite;
            Debug.Log("Воспроизведение остановлено");
        }
        else
        {
            // Начинаем воспроизведение
            if (currentRecording.Count == 0)
            {
                Debug.Log("Нет записанных нот");
                return;
            }
            isPlaying = true;
            playButtonImage.sprite = pauseSprite;
            playbackCoroutine = StartCoroutine(PlaybackCoroutine(currentRecording));
        }
    }
    
    public void NotePlayed(string noteName)
{
    if (isRecording)
    {
        currentRecording.Add(new NoteEvent
        {
            noteName = noteName,
            startTime = Time.time - recordStartTime,
            endTime = -1 // пока не отпущена
        });
    }
}


public void NoteReleased(string noteName)
{
    if (isRecording && currentRecording.Count > 0)
    {
        var lastEvent = currentRecording[currentRecording.Count - 1];
        if (lastEvent.noteName == noteName && lastEvent.endTime < 0)
        {
            lastEvent.endTime = Time.time - recordStartTime;
            currentRecording[currentRecording.Count - 1] = lastEvent;
        }
    }
}


    
   IEnumerator PlaybackCoroutine(List<NoteEvent> recording)
{
    Debug.Log("Воспроизведение начато");
    float startTime = Time.time;
    PianoInputHandler inputHandler = FindFirstObjectByType<PianoInputHandler>();
    
    foreach (var noteEvent in recording)
    {
        if (!isPlaying) break;
        
        float waitTime = noteEvent.startTime - (Time.time - startTime);
        if (waitTime > 0) yield return new WaitForSeconds(waitTime);
        
        PianoKey[] keys = FindObjectsByType<PianoKey>(FindObjectsSortMode.None);
        foreach (var key in keys)
        {
            if (key.GetNoteName() == noteEvent.noteName)
            {
                AudioSource source = key.GetComponent<AudioSource>();
                if (source != null && source.clip != null)
                {
                    inputHandler.NoteOn(source.clip);
                    
                    // Ждём, пока не наступит время отпускания
                    float duration = noteEvent.endTime - noteEvent.startTime;
                    if (duration > 0)
                    {
                        yield return new WaitForSeconds(duration);
                        inputHandler.NoteOff(source.clip);
                    }
                }
                break;
            }
        }
    }
    
    isPlaying = false;
    playButtonImage.sprite = playSprite;
    Debug.Log("Воспроизведение завершено");
}

IEnumerator ReleaseAfterDelay(PianoInputHandler handler, AudioClip clip, float delay)
{
    yield return new WaitForSeconds(delay);
    handler.NoteOff(clip);
}
    
    void OpenPlaylist()
    {
        Debug.Log("Открыть список сохранённых записей");
        // TODO: создать окно со списком файлов
    }
}