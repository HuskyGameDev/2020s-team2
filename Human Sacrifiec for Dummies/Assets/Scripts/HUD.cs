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

    // Start is called before the first frame update
    void Start()
    {
        //populate button list with the buttons
        Button[] btns = this.GetComponentsInChildren<Button>(true);
        for (int i = 0; i < btns.Length; i++)
        {
            buttons.Add(btns[i]);
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
        Move.GetComponent<Button>().interactable = false;
    }

    void MoveSacOnClick()
    {
        MoveSac.GetComponent<Button>().interactable = false;
    }

    void AbilityOneClick()
    {
        Ability1.GetComponent<Button>().interactable = false;
    }

    void AbilityTwoClick()
    {
        Ability2.GetComponent<Button>().interactable = false;
    }

    void AbilityThreeClick()
    {
        Ability3.GetComponent<Button>().interactable = false;
    }

    void EndOnClick()
    {

    }



}
