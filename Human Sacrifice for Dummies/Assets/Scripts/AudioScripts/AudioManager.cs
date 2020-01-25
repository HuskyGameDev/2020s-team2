using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sounds[] sounds;

    // Initialization, basically Start, but it goes before Start
    void Awake()
    {
        // Loop through sounds and add an audio source component
        // on the current object for each one
        foreach (Sounds s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.audioClip; //Clip of the audio source set to s.audioClip (Sounds)
            s.source.volume = s.volume;
        }
    }

    public void PlaySound(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.name == name); //Look through sounds and find element with the same name
        s.source.Play();
    }
}

[System.Serializable]
//Nested Class created to be a small sound object, mostly so that the sounds can be given names and assigned volume levels more easily
public class Sounds
{
    public string name;
    public AudioClip audioClip;

    [Range(0f, 1f)]
    public float volume;

    [HideInInspector]
    public AudioSource source;
}

