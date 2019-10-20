using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    private WorldTile _tile;
    // current movement stuff
    public Vector2 targetPosition;
    public Vector2 startPosition;

    // turn system stuff
    public turnSystemScript TurnSystem;
    public TurnClass turnClass;
    public bool isTurn = false;
    public KeyCode moveKey;

    private void Start()
    {
        // start position centered
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

        // check if it is your turn
        if (isTurn)
        {
            // trigger for movement
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                // used for checking if your clicking on the grid
                Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var worldPoint = new Vector3Int((int)Mathf.Round(point.x), Mathf.FloorToInt(point.y), 0);
                // position so you are centered on grid
                targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPosition.x = Mathf.Round(targetPosition.x);
                targetPosition.y = Mathf.Floor(targetPosition.y) + 0.5f;

                var tiles = GameTiles.instance.tiles;
                // if colliding dont move
                if (tiles.TryGetValue(worldPoint, out _tile))
                {
                    transform.position = targetPosition;

                    // end turn
                    isTurn = false;
                    turnClass.isTurn = isTurn;
                    turnClass.wasTurnPrev = true;

                }
            }
        }
    }
}
