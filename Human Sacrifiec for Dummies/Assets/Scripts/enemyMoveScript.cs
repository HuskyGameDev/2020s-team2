using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class enemyMoveScript : Move
{
    // using worldtile to be able to access info like is a tile occupied
    private WorldTile _tile;

    // turn system stuff
    public turnSystemScript TurnSystem;
    public TurnClass turnClass;
    public bool isTurn = false;

    // current movement stuff
    public Vector2 targetPosition;
    public Vector2 startPosition;

    public Vector3 moveTo;

    // Start is called before the first frame update
    void Start()
    {
        // make sure start position is centered
        targetPosition.x = Mathf.Round(gameObject.transform.position.x);
        targetPosition.y = Mathf.Floor(gameObject.transform.position.y) + 0.5f;
        startPosition = targetPosition;

        // load turn system
        TurnSystem = GameObject.Find("Turn-basedSystem").GetComponent<turnSystemScript>();
        //create a turn for the enemy
        foreach (TurnClass tc in TurnSystem.playersGroup)
        {
            if (tc.playerGameObject.name == gameObject.name)
            {
                turnClass = tc;
            }
        }
        //set information for starting position tile
        var tiles = GameTiles.instance.tiles;
        if (tiles.TryGetValue(new Vector3Int(Mathf.RoundToInt(startPosition.x), Mathf.FloorToInt(startPosition.y), 0), out _tile))
        {
            _tile.Occupied = true;
            _tile.Player = false;
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
            // convert start position to vector3int for breadth first search
            var bfsStart = new Vector3Int(Mathf.RoundToInt(startPosition.x), Mathf.FloorToInt(startPosition.y), 0);
            moveTo = BFS2(bfsStart);

            // if your turn go to wait and move
            StartCoroutine("WaitAndMove");
        }
    }

    IEnumerator WaitAndMove()
    {
        // wait for 1 second
        yield return new WaitForSeconds(1f);

        // movement goes here
        targetPosition.x = Mathf.Round(moveTo.x);
        targetPosition.y = Mathf.Floor(moveTo.y) + 0.5f;
        transform.position = targetPosition;

        //attack goes here

        //end turn
        EndTurn();

        StopCoroutine("WaitAndMove");
    }

    void ChangeStartPosition()
    {
        // make sure start position is centered
        targetPosition.x = Mathf.Round(gameObject.transform.position.x);
        targetPosition.y = Mathf.Floor(gameObject.transform.position.y) + 0.5f;
        startPosition = targetPosition;
    }

    // call this directly before the end turn stuff so that the BFS works corectly
    void PositionUpdate()
    {
        //updates info for where enemy began turn in tiles
        var tiles = GameTiles.instance.tiles;
        if (tiles.TryGetValue(new Vector3Int(Mathf.RoundToInt(startPosition.x), Mathf.FloorToInt(startPosition.y), 0), out _tile))
        {
            _tile.Occupied = false;
        }
        ChangeStartPosition();
        //updates info for new position
        if (tiles.TryGetValue(new Vector3Int(Mathf.RoundToInt(startPosition.x), Mathf.FloorToInt(startPosition.y), 0), out _tile))
        {
            _tile.Occupied = true;
            _tile.Player = false;
        }
    }

    // go through all processes needed for ending turn
    void EndTurn()
    {
        // update start position and occupied flags on the grid
        PositionUpdate();

        // reset grid to normal colors
        var tiles = GameTiles.instance.tiles;
        foreach (KeyValuePair<Vector3, WorldTile> t1 in tiles)
        {
            if (tiles.TryGetValue(t1.Key, out _tile))
            {
                _tile.TilemapMember.SetTileFlags(_tile.LocalPlace, TileFlags.None);
                _tile.TilemapMember.SetColor(_tile.LocalPlace, Color.white);
            }
        }

        // turn ends
        isTurn = false;
        turnClass.isTurn = isTurn;
        turnClass.wasTurnPrev = true;

    }
}
