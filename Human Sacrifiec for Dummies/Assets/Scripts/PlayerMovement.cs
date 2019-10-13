using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 targetPosition;
    public Vector2 startPosition;

    // turn system stuff
    public turnSystemScript TurnSystem;
    public TurnClass turnClass;
    public bool isTurn = false;
    public KeyCode moveKey;

    private void Start()
    {
        targetPosition.x = Mathf.Round(gameObject.transform.position.x);
        targetPosition.y = Mathf.Floor(gameObject.transform.position.y) + 0.5f;
        startPosition = targetPosition;

        // turn system stuff
        TurnSystem = GameObject.Find("Turn-basedSystem").GetComponent<turnSystemScript>();

        foreach(TurnClass tc in TurnSystem.playersGroup)
        {
            if(tc.playerGameObject.name == gameObject.name)
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

        if (isTurn)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                // handle movement
                targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPosition.x = Mathf.Round(targetPosition.x);
                targetPosition.y = Mathf.Floor(targetPosition.y) + 0.5f;
                transform.position = targetPosition;

                // turn based stuff
                isTurn = false;
                turnClass.isTurn = isTurn;
                turnClass.wasTurnPrev = true;
            }
        }
    }
}
