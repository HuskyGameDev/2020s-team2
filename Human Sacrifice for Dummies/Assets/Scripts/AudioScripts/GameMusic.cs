using System;
using UnityEngine;

public class GameMusic : MonoBehaviour
{
    public AudioSource MenuMusic;
    public AudioSource BattleMusic;
    public AudioSource DefeatMusic;
    public AudioSource VictoryMusic;

    private static GameMusic instance;

    static public Boolean isMenuMusic;

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
    }

    public void PlayMenuMusic()
    {
        MenuMusic.loop = true;
        isMenuMusic = true;
        StopMusic();
        MenuMusic.Play();
    }
    public void PlayBattleMusic()
    {
        BattleMusic.loop = true;
        isMenuMusic = false;
        StopMusic();
        BattleMusic.Play();
    }

    public void PlayDefeatMusic()
    {
        DefeatMusic.loop = true;
        isMenuMusic = false;
        StopMusic();
        DefeatMusic.Play();
    }

    public void PlayVictoryMusic()
    {
        VictoryMusic.loop = true;
        isMenuMusic = false;
        StopMusic();
        VictoryMusic.Play();
    }

    public void StopMusic()
    {
        MenuMusic.Stop();
        BattleMusic.Stop();
        DefeatMusic.Stop();
        VictoryMusic.Stop();
    }
}
