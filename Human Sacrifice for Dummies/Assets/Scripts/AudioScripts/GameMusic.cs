using UnityEngine;

public class GameMusic : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource BattleMusic;
    public AudioSource DefeatMusic;

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
        audioSource.loop = true;
    }

    public void PlayMainMusic()
    {
        audioSource.Play();
    }
    public void PlayBattleMusic()
    {
        BattleMusic.Play();
    }

    public void PlayDefeatMusic()
    {
        DefeatMusic.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void StopBattleMusic()
    {
        BattleMusic.Stop();
    }
}
