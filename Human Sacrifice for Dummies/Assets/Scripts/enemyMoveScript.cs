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
    public bool isCursed = false;

    // current movement stuff
    public Vector2 targetPosition;
    public Vector2 startPosition;

    public Vector3 moveTo;

    public int damage;

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
        // if enemy drops to 0 hp or below destroy enemy
        if (health <= 0)
        {
            // any effects that happen on enemy death goes here.
            var tiles = GameTiles.instance.tiles;
            if (tiles.TryGetValue(new Vector3Int(Mathf.RoundToInt(startPosition.x), Mathf.FloorToInt(startPosition.y), 0), out _tile))
            {
                _tile.Occupied = false;
            }
            Destroy(gameObject);
        }

        //turn system stuff
        isTurn = turnClass.isTurn;

        // check if its your turn
        if(isTurn)
        {
            // convert start position to vector3int for breadth first search
            var bfsStart = new Vector3Int(Mathf.RoundToInt(startPosition.x), Mathf.FloorToInt(startPosition.y), 0);
            //find where to move to
            moveTo = BFS2(bfsStart);

            // if your turn go to wait and move
            StartCoroutine("WaitAndMove");
        }
    }

    IEnumerator WaitAndMove()
    {
        // wait
        yield return new WaitForSeconds(0.5f);

        // if cursed skip movement
        if (!isCursed)
        {
            // movement goes here
            targetPosition.x = Mathf.Round(moveTo.x);
            targetPosition.y = Mathf.Floor(moveTo.y) + 0.5f;
            transform.position = targetPosition;
        }

        yield return new WaitForSeconds(0.5f);

        //attack goes here
        var attackFrom = new Vector3Int(Mathf.RoundToInt(targetPosition.x), Mathf.FloorToInt(targetPosition.y), 0);
        EnemyMelee(attackFrom);

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
        // update start position and occupied flags on the grid before turn ends so that attack goes from current position
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
        isCursed = false;
    }

    // looks up down left and right to check if it can attack
    void EnemyMelee(Vector3Int currentPos)
    {
        // get position where neightbors would be
        Vector3Int up = new Vector3Int(currentPos.x, currentPos.y + 1, 0);
        Vector3Int down = new Vector3Int(currentPos.x, currentPos.y - 1, 0);
        Vector3Int left = new Vector3Int(currentPos.x - 1, currentPos.y, 0);
        Vector3Int right = new Vector3Int(currentPos.x + 1, currentPos.y, 0);

        var tiles = GameTiles.instance.tiles;

        bool hasAttacked = false;
        // if neighbor exists check if space is occupied
        // if not occupied add to neighbors list
        if (tiles.TryGetValue(up, out _tile))
        {
            if (_tile.Occupied)
            {
                if (_tile.Player)
                {
                    //attack up
                    hasAttacked = true;
                    EnemyDoDamage(up, damage);
                }
            }

        }

        if (hasAttacked)
        { }
        else if (tiles.TryGetValue(left, out _tile))
        {
            if (_tile.Occupied)
            {
                if (_tile.Player)
                {
                    //attack left
                    hasAttacked = true;
                    EnemyDoDamage(left, damage);
                }
            }
        }

        if (hasAttacked)
        { }
        else if (tiles.TryGetValue(down, out _tile))
        {
            if (_tile.Occupied)
            {
                if (_tile.Player)
                {
                    // attack down
                    hasAttacked = true;
                    EnemyDoDamage(down, damage);
                }
            }
        }

        if (hasAttacked)
        { }
        else if (tiles.TryGetValue(right, out _tile))
        {
            if (_tile.Occupied)
            {
                if (_tile.Player)
                {
                    // attack right
                    hasAttacked = true;
                    EnemyDoDamage(right, damage);
                }
            }
        }
    }

    void EnemyDoDamage(Vector3Int position, int damage)
    {
        Vector2 tPos;
        tPos.x = Mathf.Round(position.x);
        tPos.y = Mathf.Floor(position.y) + 0.5f;

        // damage target at position
        var objects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject go in objects)
        {
            //check if object is in player layer
            if (go.layer == 10)
            {
                if (tPos.Equals(go.transform.position))
                {
                    if (go.GetComponent<PlayerMovement>().bubble)
                    {
                        go.GetComponent<PlayerMovement>().bubble = false;
                    }
                    else
                    {
                        go.GetComponent<Move>().health -= damage;
                    }
                }
            }
        }
    }
}
