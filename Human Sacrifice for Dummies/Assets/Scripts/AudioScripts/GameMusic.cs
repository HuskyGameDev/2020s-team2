using UnityEngine;

public class GameMusic : MonoBehaviour
{
    public AudioSource MenuMusic;
    public AudioSource BattleMusic;
    public AudioSource DefeatMusic;
    public AudioSource VictoryMusic;

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

        MenuMusic.loop = true;
        BattleMusic.loop = true;
        DefeatMusic.loop = true;
        VictoryMusic.loop = true; 
    }

    public void PlayMenuMusic()
    {
        StopMusic();
        MenuMusic.Play();
    }
    public void PlayBattleMusic()
    {
        StopMusic();
        BattleMusic.Play();
    }

    public void PlayDefeatMusic()
    {
        StopMusic();
        DefeatMusic.Play();
    }

    public void PlayVictoryMusic()
    {
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
