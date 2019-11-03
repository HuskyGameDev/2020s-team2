using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectAbility : MonoBehaviour
{
    //List of all available abilities.
    string[] abilities = {"Staff Whack", "Fiendish Wisp", "Vicious Slap", 
        "Forgotten Curse", "Infernal Bubble", "Shady Switcheroo", "Shadow Step" }; 
    //This string store all of the names of the abilities the user has chosen.
    string[] chosen = new string[3];
    //Initialize all of the buttons in the scene.
    public Button Ability1;
    public Button Ability2;
    public Button Ability3;
    public Button Ability4;
    public Button Ability5;
    public Button Ability6;
    public Button Ability7;
    public Button Reset;
    public Button Confirm;
    public Text Chosen1;
    public Text Chosen2;
    public Text Chosen3;

    //Initialize a list for button objects needed to reset them when reset button is pressed.
    public List<Button> buttons;

    private void Start()
    {
        //populate button list with the buttons
        Button[] btns = this.GetComponentsInChildren<Button>(true);
        for(int i = 0; i < btns.Length; i++)
        {
            buttons.Add(btns[i]);
        }

        //get all button components
        Button ability1 = Ability1.GetComponent<Button>();
        Button ability2 = Ability2.GetComponent<Button>();
        Button ability3 = Ability3.GetComponent<Button>();
        Button ability4 = Ability4.GetComponent<Button>();
        Button ability5 = Ability5.GetComponent<Button>();
        Button ability6 = Ability6.GetComponent<Button>();
        Button ability7 = Ability7.GetComponent<Button>();
        Button reset = Reset.GetComponent<Button>();
        Button confirm = Confirm.GetComponent<Button>();
        Text chosen1 = Chosen1.GetComponentInChildren<Text>();
        Text chosen2 = Chosen2.GetComponentInChildren<Text>();
        Text chosen3 = Chosen3.GetComponentInChildren<Text>();

        //add appropriate listeners to the buttons
        ability1.onClick.AddListener(AddOneToBar);
        ability2.onClick.AddListener(AddTwoToBar);
        ability3.onClick.AddListener(AddThreeToBar);
        ability4.onClick.AddListener(AddFourToBar);
        ability5.onClick.AddListener(AddFiveToBar);
        ability6.onClick.AddListener(AddSixToBar);
        ability7.onClick.AddListener(AddSevenToBar);
        reset.onClick.AddListener(resetButton);
        confirm.onClick.AddListener(confirmButton);
    }

    //Check if the chosen ability array is filled at the given index
    bool IsOpen(int index)
    {
        return (chosen[index] == null);
    }

    //reset all butons to be interactable again and remove choices from chosen array
    void resetButton()
    {
        for(int i = 0; i < chosen.Length; i++)
        {
            chosen[i] = null;
        }
        for(int i = 0; i < buttons.Count ; i++)
        {
            buttons[i].interactable = true;
        }
        updateChosenList();
    }


    //confirm choices and advance to the next scene
    void confirmButton()
    {
        if (chosen[2] == null)
        {

        }
        else
        {
            SceneManager.LoadScene("Level1");
        }
    }

    //Add the first ability to whichever slot is available in the chosen array
    void AddOneToBar()
    {
        if (IsOpen(0))
        {
            chosen[0] = abilities[0];
        }else if (IsOpen(1))
        {
            chosen[1] = abilities[0];
        }else if (IsOpen(2))
        {
            chosen[2] = abilities[0];
        }
        updateChosenList();
        Ability1.GetComponent<Button>().interactable = false;
        Debug.Log(chosen[0]);
        Debug.Log(chosen[1]);
        Debug.Log(chosen[2]);


    }

    //Add the second ability to whichever slot is available in the chosen array
    void AddTwoToBar()
    {
        if (IsOpen(0))
        {
            chosen[0] = abilities[1];
        }
        else if (IsOpen(1))
        {
            chosen[1] = abilities[1];
        }
        else if (IsOpen(2))
        {
            chosen[2] = abilities[1];
        }
        updateChosenList();
        Ability2.GetComponent<Button>().interactable = false;
        Debug.Log(chosen[0]);
        Debug.Log(chosen[1]);
        Debug.Log(chosen[2]);

    }

    //Add the third ability to whichever slot is available in the chosen array
    void AddThreeToBar()
    {
        if (IsOpen(0))
        {
            chosen[0] = abilities[2];
        }
        else if (IsOpen(1))
        {
            chosen[1] = abilities[2];
        }
        else if (IsOpen(2))
        {
            chosen[2] = abilities[2];
        }
        updateChosenList();
        Ability3.GetComponent<Button>().interactable = false;
        Debug.Log(chosen[0]);
        Debug.Log(chosen[1]);
        Debug.Log(chosen[2]);
    }

    //Add the fourth ability to whichever slot is available in the chosen array
    void AddFourToBar()
    {
        if (IsOpen(0))
        {
            chosen[0] = abilities[3];
        }
        else if (IsOpen(1))
        {
            chosen[1] = abilities[3];
        }
        else if (IsOpen(2))
        {
            chosen[2] = abilities[3];
        }
        updateChosenList();
        Ability4.GetComponent<Button>().interactable = false;
        Debug.Log(chosen[0]);
        Debug.Log(chosen[1]);
        Debug.Log(chosen[2]);
    }

    //Add the fifth ability to whichever slot is available in the chosen array
    void AddFiveToBar()
    {
        if (IsOpen(0))
        {
            chosen[0] = abilities[4];
        }
        else if (IsOpen(1))
        {
            chosen[1] = abilities[4];
        }
        else if (IsOpen(2))
        {
            chosen[2] = abilities[4];
        }
        updateChosenList();
        Ability5.GetComponent<Button>().interactable = false;
        Debug.Log(chosen[0]);
        Debug.Log(chosen[1]);
        Debug.Log(chosen[2]);
    }

    //Add the sixth ability to whichever slot is available in the chosen array
    void AddSixToBar()
    {
        if (IsOpen(0))
        {
            chosen[0] = abilities[5];
        }
        else if (IsOpen(1))
        {
            chosen[1] = abilities[5];
        }
        else if (IsOpen(2))
        {
            chosen[2] = abilities[5];
        }
        updateChosenList();
        Ability6.GetComponent<Button>().interactable = false;
        Debug.Log(chosen[0]);
        Debug.Log(chosen[1]);
        Debug.Log(chosen[2]);
    }

    //Add the seventh ability to whichever slot is available in the chosen array
    void AddSevenToBar()
    {
        if (IsOpen(0))
        {
            chosen[0] = abilities[6];
        }
        else if (IsOpen(1))
        {
            chosen[1] = abilities[6];
        }
        else if (IsOpen(2))
        {
            chosen[2] = abilities[6];
        }
        updateChosenList();
        Ability7.GetComponent<Button>().interactable = false;
        Debug.Log(chosen[0]);
        Debug.Log(chosen[1]);
        Debug.Log(chosen[2]);
    }

    //adds the chosen ability to the list
    void updateChosenList()
    {
        if (chosen[0] != null)
        {
            Chosen1.text = chosen[0];
        }
        else
        {
            Chosen1.text = "Ability One";
        }

        if (chosen[1] != null)
        {
            Chosen2.text = chosen[1];
        }
        else
        {
            Chosen2.text = "Ability Two";
        }

        if (chosen[2] != null)
        {
            Chosen3.text = chosen[2];
        }
        else
        {
            Chosen3.text = "Ability Three";
        }
    }
}