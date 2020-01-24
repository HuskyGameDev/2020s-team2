using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnHandlerScript : MonoBehaviour
{
    public GameObject wizard;
    public GameObject sacrifice;

    public Button endTurn;
    public Button wizardMove;
    public Button sacrificeMove;
    public Button staffWhack;
    public Button fiendishWisp;
    public Button forgottenCurse;
    public Button infernalBubble;
    public Button shadyswitcheroo;

    public bool wizardHasMoved = false;
    public bool sacrificeHasMoved = false;
    public bool wizardHasAttacked = false;

    // turn system stuff
    public turnSystemScript TurnSystem;
    public TurnClass turnClass;
    public bool isTurn = false;

    // Start is called before the first frame update
    void Start()
    {
        Button endturn = endTurn.GetComponent<Button>();
        Button wizardmove = wizardMove.GetComponent<Button>();
        Button sacrificemove = sacrificeMove.GetComponent<Button>();
        Button staffwhack = staffWhack.GetComponent<Button>();
        Button shadySwitcheroo = shadyswitcheroo.GetComponent<Button>();
        //Button fiendishwisp = fiendishWisp.GetComponent<Button>();
        //Button forgottencurse = forgottenCurse.GetComponent<Button>();
        //Button infernalbubble = infernalBubble.GetComponent<Button>();


        endturn.onClick.AddListener(TurnEnder);
        wizardmove.onClick.AddListener(MoveWizard);
        sacrificemove.onClick.AddListener(MoveSacrifice);
        staffwhack.onClick.AddListener(UseStaffWhack);
        shadySwitcheroo.onClick.AddListener(UseShadySwitcheroo);
        //fiendishwisp.onClick.AddListener(UseFiendishWisp);
        //forgottencurse.onClick.AddListener(UseForgottenCurse);
        //infernalbubble.onClick.AddListener(UseInfernalBubble);


        // load turn system
        TurnSystem = GameObject.Find("Turn-basedSystem").GetComponent<turnSystemScript>();

        foreach (TurnClass tc in TurnSystem.playersGroup)
        {
            if (tc.playerGameObject.name == gameObject.name)
            {
                turnClass = tc;
            }
        }
    }

    void MoveWizard()
    {
        if (!wizardHasMoved)
        {
            wizard.GetComponent<PlayerMovement>().isTurn = true;
        }
    }

    void MoveSacrifice()
    {
        if (!sacrificeHasMoved)
        {
            sacrifice.GetComponent<PlayerMovement>().isTurn = true;
        }
    }

    void TurnEnder()
    {
        wizardHasMoved = false;
        sacrificeHasMoved = false;
        wizardHasAttacked = false;
        isTurn = false;
        turnClass.isTurn = isTurn;
        turnClass.wasTurnPrev = true;
    }

    void UseStaffWhack()
    {
        if (!wizardHasAttacked)
        {
            wizard.GetComponent<StaffWhack>().isTurn = true;
        }
    }

    void UseFiendishWisp()
    {
        if (!wizardHasAttacked)
        {
            wizard.GetComponent<FiendishWisp>().isTurn = true;
        }
    }

    void UseForgottenCurse()
    {
        if (!wizardHasAttacked)
        {
            wizard.GetComponent<ForgottenCurse>().isTurn = true;
        }
    }

    void UseInfernalBubble()
    {
        if (!wizardHasAttacked)
        {
            wizard.GetComponent<InfernalBubble>().isTurn = true;
        }
    }

    private void UseShadySwitcheroo()
    {
        if (!wizardHasAttacked)
        {
            wizard.GetComponent<ShadySwitcheroo>().isTurn = true;
        }
    }
}
