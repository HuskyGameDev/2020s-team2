using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShadowStep : MonoBehaviour
{
    public GameObject playerTurnHandler;

    public bool isTurn = false;
    public bool hasSearched = false;
    public Vector2 startPosition;
    public Vector2 targetPosition;
    public Vector2 tempPosition;

    private WorldTile _tile;

    // Update is called once per frame
    void Update()
    {
        if (isTurn)
        {
            gameObject.GetComponent<PlayerMovement>().moveRange += 1;
            EndAttack();
        }
    }
    void EndAttack()
    {
        hasSearched = false;
        isTurn = false;
        playerTurnHandler.GetComponent<PlayerTurnHandlerScript>().wizardHasAttacked = true;
    }
}
