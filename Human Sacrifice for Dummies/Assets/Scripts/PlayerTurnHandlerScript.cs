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
    public Button viciousSlap;

    public bool wizardHasMoved = false;
    public bool sacrificeHasMoved = false;
    public bool wizardHasAttacked = false;

    // turn system stuff
    public turnSystemScript TurnSystem;
    public TurnClass turnClass;
    public HUD hud;
    public bool isTurn = false;

    List<Button> boundAbilities = new List<Button>();

    // Start is called before the first frame update
    void Start()
    {
        Button endturn = endTurn.GetComponent<Button>();
        endturn.onClick.AddListener(TurnEnder);

        Button wizardmove = wizardMove.GetComponent<Button>();
        wizardmove.onClick.AddListener(MoveWizard);

        Button sacrificemove = sacrificeMove.GetComponent<Button>();
        sacrificemove.onClick.AddListener(MoveSacrifice);

        boundAbilities.Add(hud.Ability1);
        boundAbilities.Add(hud.Ability2);
        boundAbilities.Add(hud.Ability3);

        for (int i = 0; i < 3; i++)
        {
            Debug.Log(boundAbilities[i]);
            if (PersistentData.abilities[i] == "Staff Whack")
            {
                staffWhack = boundAbilities[i];
            }
            else if (PersistentData.abilities[i] == "Fiendish Wisp")
            {
                fiendishWisp = boundAbilities[i];
            }
            else if (PersistentData.abilities[i] == "Vicious Slap")
            {
                viciousSlap = boundAbilities[i];
            }
            else if (PersistentData.abilities[i] == "Forgotten Curse")
            {
                forgottenCurse = boundAbilities[i];
            }
            else if (PersistentData.abilities[i] == "Infernal Bubble")
            {
                infernalBubble = boundAbilities[i];
            }
            else if (PersistentData.abilities[i] == "Shady Switcheroo")
            {
                shadyswitcheroo = boundAbilities[i];
            }
            else if (PersistentData.abilities[i] == "Shadow Step")
            {
                shadowstep = boundAbilities[i];
            }
        }

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

        GameObject.FindGameObjectWithTag("Music").GetComponent<GameMusic>().PlayBattleMusic();

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
        FindObjectOfType<AudioManager>().PlaySound("Enemy Turn"); //When end turn button is hit, enemy turn sound will play
        wizardHasMoved = false;
        sacrificeHasMoved = false;
        wizardHasAttacked = false;
        isTurn = false;
        turnClass.isTurn = isTurn;
        turnClass.wasTurnPrev = true;
        hud.resetButtons();

        //Resets move range
        wizard.GetComponent<PlayerMovement>().moveRange = 3;
        sacrifice.GetComponent<PlayerMovement>().moveRange = 3;
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
