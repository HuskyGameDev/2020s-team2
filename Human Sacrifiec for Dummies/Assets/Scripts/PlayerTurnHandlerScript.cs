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

    public bool wizardHasMoved = false;
    public bool sacrificeHasMoved = false;

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

        endturn.onClick.AddListener(TurnEnder);
        wizardmove.onClick.AddListener(MoveWizard);
        sacrificemove.onClick.AddListener(MoveSacrifice);

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

    // Update is called once per frame
    void TurnEnder()
    {
        wizardHasMoved = false;
        sacrificeHasMoved = false;
        isTurn = false;
        turnClass.isTurn = isTurn;
        turnClass.wasTurnPrev = true;
    }
}
