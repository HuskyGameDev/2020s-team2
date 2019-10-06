using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public Button Play;
    public Button Exit;

    void Start()
    {
        Button exit = Exit.GetComponent<Button>();
        Button play = Play.GetComponent<Button>();
        play.onClick.AddListener(PlayOnClick);
        exit.onClick.AddListener(ExitOnClick);
    }
   
    void PlayOnClick()
    {
        SceneManager.LoadScene("InGame");
    }

    void ExitOnClick()
    {
        Application.Quit();
    }
}
