using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public Button Play; 
    public Button Options;
    public Button Exit;
   
    void Start()
    {
        Button play = Play.GetComponent<Button>();
        Button options = Options.GetComponent<Button>();
        Button exit = Exit.GetComponent<Button>();
        play.onClick.AddListener(PlayOnClick);
        options.onClick.AddListener(OptionsOnClick);
        exit.onClick.AddListener(ExitOnClick);
    }
   
    void PlayOnClick()
    {
        SceneManager.LoadScene("InGame");
    }

    void OptionsOnClick()
    {
        SceneManager.LoadScene("Options");
    }

    void ExitOnClick()
    {
        Application.Quit();
    }
}
