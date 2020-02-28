using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
    public GameObject textBox; // The box the text is in
    public Text theText; // The text itself
    public GameObject stopPlayerFromPlayingBox; //Invisible panel that stops user from pressing buttons while dialog is going

    public TextAsset textFile; // The file the text is coming from
    public string[] textLines; // Hold the lines gotten from the text file

    public int currentLineNumber;
    public int endAtLine;

    // Start is called before the first frame update
    void Start()
    {
        if (textFile != null)
        {
            textLines = textFile.text.Split('\n'); // Split the textfile into lines in an array
        }

        if (endAtLine == 0)
        {
            endAtLine = textLines.Length - 1;
        }
    }

    void Update()
    {
        if (currentLineNumber < endAtLine) {

            theText.text = textLines[currentLineNumber]; // Display current line

            if (Input.GetKeyDown(KeyCode.Return))
            {
                currentLineNumber++;
            }

            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                currentLineNumber++;
            }

            if (Input.GetMouseButtonDown(0))
            {
                currentLineNumber++;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentLineNumber++;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow)) // Move Backwards through dialog
            {
                if (currentLineNumber > 0)
                {
                    currentLineNumber--;
                }
            }
        }
        if (currentLineNumber >= endAtLine)
        {
            textBox.SetActive(false); // Hides the text box panel
            stopPlayerFromPlayingBox.SetActive(false); // Hides invisible panel from blocking the user from the buttons
        }
    }
}
