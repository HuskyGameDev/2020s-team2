using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class YouLoseScript : MonoBehaviour
{

    public Button MainMenu;
    public Button Restart;
    public Button RageQuit;

    // Start is called before the first frame update
    void Start()
    {
        //get all of the buttons from the scene and add the appropriate listener
        Button mainMenu = MainMenu.GetComponent<Button>();
        Button restart = Restart.GetComponent<Button>();
        Button rageQuit = RageQuit.GetComponent<Button>();

        mainMenu.onClick.AddListener(MainMenuOnClick);
        restart.onClick.AddListener(PlayOnClick);
        rageQuit.onClick.AddListener(ExitOnClick);
    }

    void PlayOnClick()
    {
        //start game scene
//        FindObjectOfType<AudioManager>().PlaySound("Menu Button Forwards");
        SceneManager.LoadScene("ChooseAbility");
    }
    void ExitOnClick()
    {
        //quit the game
//        FindObjectOfType<AudioManager>().PlaySound("Menu Button Backwards");
        Application.Quit();
    }

    void MainMenuOnClick()
    {
//        FindObjectOfType<AudioManager>().PlaySound("Menu Button Backwards");
        SceneManager.LoadScene("MainMenu");
    }
}
