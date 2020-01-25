using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsScript : MonoBehaviour
{
    public Button Exit;

    void Start()
    {
        GameObject.FindGameObjectWithTag("Music").GetComponent<GameMusic>().PlayMusic();
        Button exit = Exit.GetComponent<Button>();
        exit.onClick.AddListener(ExitOnClick);
    }

   void ExitOnClick()
    {
        FindObjectOfType<AudioManager>().PlaySound("Menu Button Backward");
        SceneManager.LoadScene("MainMenu");
    }
}
