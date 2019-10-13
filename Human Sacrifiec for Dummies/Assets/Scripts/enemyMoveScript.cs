using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMoveScript : MonoBehaviour
{
    // turn system stuff
    public turnSystemScript TurnSystem;
    public TurnClass turnClass;
    public bool isTurn = false;

    // Start is called before the first frame update
    void Start()
    {
        // turn system stuff
        TurnSystem = GameObject.Find("Turn-basedSystem").GetComponent<turnSystemScript>();

        foreach (TurnClass tc in TurnSystem.playersGroup)
        {
            if (tc.playerGameObject.name == gameObject.name)
            {
                turnClass = tc;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //turn system stuff
        isTurn = turnClass.isTurn;

        // check if its your turn
        if(isTurn)
        {
            // if your turn go to wait and move
            StartCoroutine("WaitAndMove");
        }
    }

    IEnumerator WaitAndMove()
    {
        // wait for 1 second
        yield return new WaitForSeconds(1f);

        // movement goes here

        // end turn
        isTurn = false;
        turnClass.isTurn = isTurn;
        turnClass.wasTurnPrev = true;

        StopCoroutine("WaitAndMove");
    }
}
