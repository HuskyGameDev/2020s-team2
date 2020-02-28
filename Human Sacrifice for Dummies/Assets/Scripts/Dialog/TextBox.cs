using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
    public GameObject textBox;
    public Text theText;

    public TextAsset textFile;
    public string[] textLines;

    public int currentLineNumber;
    public int endAtLine;

 //   public PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
       // player = FindObjectOfType<PlayerMovement>();

        if (textFile != null)
        {
            textLines = textFile.text.Split('\n');
        }

        if (endAtLine == 0)
        {
            endAtLine = textLines.Length - 1;
        }
    }

    void Update()
    {
        if (currentLineNumber < endAtLine) {

            theText.text = textLines[currentLineNumber];

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
        }
        if (currentLineNumber >= endAtLine)
        {
            textBox.SetActive(false);
        }
    }
}
