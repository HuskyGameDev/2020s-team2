using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusic : MonoBehaviour
{
    private AudioSource audioSource;
    private static GameMusic instance;
    private void Awake()
    {
        if(!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(transform.gameObject);
        audioSource = GetComponent<AudioSource>();
        //audioSource.loop = true;
    }

    public void PlayMusic()
    {
        if (audioSource.isPlaying) return;
        else
        {
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}
