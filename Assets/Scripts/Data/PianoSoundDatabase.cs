using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PianoSounds", menuName = "Piano/Sound Database")]
public class PianoSoundDatabase : ScriptableObject
{
    [System.Serializable]
    public class NoteSound
    {
        public string noteName;  // "C0", "C0sharp", "D0" и т.д.
        public AudioClip clip;
    }
    
    public List<NoteSound> noteSounds = new List<NoteSound>();
    
    public AudioClip GetClip(string noteName)
    {
        foreach (var ns in noteSounds)
        {
            if (ns.noteName == noteName)
                return ns.clip;
        }
        return null;
    }
}