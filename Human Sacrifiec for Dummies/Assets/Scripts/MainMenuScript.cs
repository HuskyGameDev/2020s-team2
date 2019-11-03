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

        //get all of the buttons from the scene and add the appropriate listener
        Button play = Play.GetComponent<Button>();
        Button options = Options.GetComponent<Button>();
        Button exit = Exit.GetComponent<Button>();
        play.onClick.AddListener(PlayOnClick);
        options.onClick.AddListener(OptionsOnClick);
        exit.onClick.AddListener(ExitOnClick);
    }
   
    void PlayOnClick()
    {
        //start game scene
        SceneManager.LoadScene("InGame");
    }

    void OptionsOnClick()
    {
        //start options screen
        SceneManager.LoadScene("Options");
    }

    void ExitOnClick()
    {
        //quit the game
        Application.Quit();
    }
}
