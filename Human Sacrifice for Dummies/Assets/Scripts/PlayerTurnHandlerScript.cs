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
    public Button shadowstep;

    public bool wizardHasMoved = false;
    public bool sacrificeHasMoved = false;
    public bool wizardHasAttacked = false;

    // turn system stuff
    public turnSystemScript TurnSystem;
    public TurnClass turnClass;
    public HUD hud;
    public bool isTurn = false;

    // Start is called before the first frame update
    void Start()
    {
        Button endturn = endTurn.GetComponent<Button>();
        endturn.onClick.AddListener(TurnEnder);

        Button wizardmove = wizardMove.GetComponent<Button>();
        wizardmove.onClick.AddListener(MoveWizard);

        Button sacrificemove = sacrificeMove.GetComponent<Button>();
        sacrificemove.onClick.AddListener(MoveSacrifice);

        if (staffWhack != null)
        {
            Button staffwhack = staffWhack.GetComponent<Button>();
            staffwhack.onClick.AddListener(UseStaffWhack);
        }

        if (shadyswitcheroo != null)
        {
            Button shadySwitcheroo = shadyswitcheroo.GetComponent<Button>();
            shadySwitcheroo.onClick.AddListener(UseShadySwitcheroo);
        }

        if (fiendishWisp != null)
        {
            Button fiendishwisp = fiendishWisp.GetComponent<Button>();
            fiendishwisp.onClick.AddListener(UseFiendishWisp);
        }

        if (forgottenCurse != null)
        {
            Button forgottencurse = forgottenCurse.GetComponent<Button>();
            forgottencurse.onClick.AddListener(UseForgottenCurse);
        }

        if (infernalBubble != null)
        {
            Button infernalbubble = infernalBubble.GetComponent<Button>();
            infernalbubble.onClick.AddListener(UseInfernalBubble);
        }

        if (shadowstep != null)
        {
            Button shadowStep = shadowstep.GetComponent<Button>();
            shadowStep.onClick.AddListener(useshadowStep);
        }


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

    private void useshadowStep()
    {
        if (!wizardHasAttacked)
        {
            wizard.GetComponent<ShadowStep>().isTurn = true;
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
        hud.resetButtons();
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
