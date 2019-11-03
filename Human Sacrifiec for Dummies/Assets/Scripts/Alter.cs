using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Alter : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if the thing colliding with the alter has tag sacrifice
        if (collision.CompareTag("Sacrifice"))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
