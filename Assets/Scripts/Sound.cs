using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable] public class Sound
{
    public string name;
    public AudioSource source;
    public AudioClip clip;
    public bool loop = false;
    [Range(0, 3)] public float pitch = 1;
    [Range(0, 1)] public float volume = 0.3f;
}
