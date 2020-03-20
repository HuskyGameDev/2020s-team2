using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public Button Play;
    public Button Tutorial;
    public Button Options;
    public Button Exit;
    static public Boolean isTutorial;

    void Start()
    {
        //get all of the buttons from the scene and add the appropriate listener
        Button play = Play.GetComponent<Button>();
        Button tutorial = Tutorial.GetComponent<Button>();
        Button options = Options.GetComponent<Button>();
        Button exit = Exit.GetComponent<Button>();
        play.onClick.AddListener(PlayOnClick);
        tutorial.onClick.AddListener(TutorialOnClick);
        options.onClick.AddListener(OptionsOnClick);
        exit.onClick.AddListener(ExitOnClick);

        GameObject.FindGameObjectWithTag("Music").GetComponent<GameMusic>().PlayMenuMusic();
        isTutorial = false;
    }
   
    void PlayOnClick()
    {
        //start game scene
        FindObjectOfType<AudioManager>().PlaySound("Evil Laugh");
        SceneManager.LoadScene("ChooseAbility");
    }

    void TutorialOnClick()
    {
        FindObjectOfType<AudioManager>().PlaySound("Evil Laugh");
        isTutorial = true;
        SceneManager.LoadScene("ChooseAbility");
    }

    void OptionsOnClick()
    {
        FindObjectOfType<AudioManager>().PlaySound("Menu Button Forwards");
        SceneManager.LoadScene("Options");
    }

    void ExitOnClick()
    {
        //quit the game
        FindObjectOfType<AudioManager>().PlaySound("Menu Button Backwards");
        Application.Quit();
    }
}
