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
        Button exit = Exit.GetComponent<Button>();
        exit.onClick.AddListener(ExitOnClick);
    }

   void ExitOnClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
