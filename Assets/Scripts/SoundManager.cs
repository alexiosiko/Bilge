using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    public void StopAudio(string name)
    {
        // Get sound
        Sound s = GetSound(name);

        // If not found
        if (s == null)
            return;

        // Stop audio
        StartCoroutine(FadeAudio(s));
    }
    IEnumerator FadeAudio(Sound s)
    {
        // Store default volume
        float volumeDefault = s.source.volume;

        for (int i = 0; i < 10; i++)
        {
            // Lower the volume
            s.source.volume -= volumeDefault / 10;
            yield return new WaitForSeconds(0.05f);
        }
        // Disable source
        s.source.Stop();
        
        // Then set volume back to default
        s.source.volume = volumeDefault;
    }
    public void PlayAudio(string name)
    {
        // Get sound
        Sound s = GetSound(name);

        // If not found
        if (s == null)
            return;

        // Play audio
        //if (s.source.isPlaying == false)
            s.source.Play();
    }
    Sound GetSound(string name)
    {
        // Search for sound
        Sound s = null;
        foreach (Sound temp in sounds)
        {  
            if (temp.name == name)
                s = temp;
        }

        // If didn't find clip
        if (s == null) {
            Debug.Log("Couldn't find sound clip: " + name);
            return null;
        }

        // Return the sound
        return s;
    }
}
