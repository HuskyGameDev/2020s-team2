using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class HUD : MonoBehaviour
{
    public Text PlayerName;

    public Button Ability1;
    public Button Ability2;
    public Button Ability3;
    public Button Move;
    public Button EndTurn;
    public Button MoveSac;

    public Text Turn;

    public List<Button> buttons;
    
    bool[] buttonUsed = {false,false,false,false,false,false};


    // Start is called before the first frame update
    void Start()
    {
        Button[] btns = this.GetComponentsInChildren<Button>(true);
        for (int i = 0; i < btns.Length; i++)
        {
            buttons.Add(btns[i]);
            buttonUsed[i] = false;
        }

        Button ability1 = Ability1.GetComponent<Button>();
        Button ability2 = Ability2.GetComponent<Button>();
        Button ability3 = Ability3.GetComponent<Button>();
        Button move = Move.GetComponent<Button>();
        Button endturn = EndTurn.GetComponent<Button>();
        Button moveSac = MoveSac.GetComponent<Button>();
        Text name = PlayerName.GetComponent<Text>();
        Text turn = Turn.GetComponent<Text>();

        ability1.onClick.AddListener(AbilityOneClick);
        ability2.onClick.AddListener(AbilityTwoClick);
        ability3.onClick.AddListener(AbilityThreeClick);
        move.onClick.AddListener(MovePlayerOnClick);
        moveSac.onClick.AddListener(MoveSacOnClick);
        endturn.onClick.AddListener(EndOnClick);

    }

    void MovePlayerOnClick()
    {
        StartCoroutine(waitMove(0));       
    }

    void MoveSacOnClick()
    {
        StartCoroutine(waitMove(1));
    }

    void AbilityOneClick()
    {
        StartCoroutine(waitAction());
        Ability1.GetComponent<Button>().interactable = false;
        buttonUsed[0] = true;
        buttonUsed[1] = true;
        buttonUsed[2] = true;
    }

    void AbilityTwoClick()
    {
        StartCoroutine(waitAction());
        Ability2.GetComponent<Button>().interactable = false;
        buttonUsed[0] = true;
        buttonUsed[1] = true;
        buttonUsed[2] = true;
    }

    void AbilityThreeClick()
    {
        StartCoroutine(waitAction());
        Ability3.GetComponent<Button>().interactable = false;
        buttonUsed[0] = true;
        buttonUsed[1] = true;
        buttonUsed[2] = true;
    }

    void EndOnClick()
    {

    }

    //index is the index of the button that was pressed, used to keep it disabled after running 
    //the coroutine
    IEnumerator waitAction()
    {
        for(int i = 0; i < buttons.Count; i++)
        {
                buttons[i].GetComponent<Button>().interactable = false;
        }
        
        //Replace with wait until <Abilityscript finishes>
        yield return new WaitForSeconds(1f);


        for (int i = 0; i < 6; i++)
        {
            if (!buttonUsed[i])
            {
                buttons[i].GetComponent<Button>().interactable = true;
            }
        }


    }

    //which is which character is moving, 0 for player, 1 for sacrifice.
    IEnumerator waitMove(int which)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].GetComponent<Button>().interactable = false;
        }

        if(which == 0)
        {
            buttonUsed[3] = true;
        }
        else if(which == 1)
        {
            buttonUsed[4] = true;
        }


        //Replace this with wait until <movescript finishes>
        yield return new WaitForSeconds(1f);


        

        for (int i = 0; i < 6; i++)
        {
            if (!buttonUsed[i])
            {
                buttons[i].GetComponent<Button>().interactable = true;
            }
        }
       
    }

    public void resetButtons()
    {
        Button[] btns = this.GetComponentsInChildren<Button>(true);
        for (int i = 0; i < btns.Length; i++)
        {
            //buttons.Add(btns[i]);
            buttonUsed[i] = false;
        }
    }

}
