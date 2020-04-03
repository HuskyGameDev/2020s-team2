using System;
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
    public bool ranged = false;
    public bool hasMoved = false;

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

        if (gameObject.name.Equals("Ranged Enemy"))
            ranged = true;
    }

    // Update is called once per frame
    void Update()
    {
        // if enemy drops to 0 hp or below destroy enemy
        if (health <= 0)
        {
            // any effects that happen on enemy death goes here.
            FindObjectOfType<AudioManager>().PlaySound("Enemy Death");
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
            FindObjectOfType<AudioManager>().PlaySound("Player Turn");
        }
    }

    IEnumerator WaitAndMove()
    {
        // wait
        yield return new WaitForSeconds(0.5f);

        //Finds location of Wizard
        GameObject[] gameObjects = FindObjectsOfType<GameObject>();
        GameObject player = null;
        GameObject sacrifice = null;
        foreach (GameObject go in gameObjects)
        {
            if (go.name.Equals("Wizard"))
                player = go;
            else if (go.name.Equals("Sacrifice"))
                sacrifice = go;
        }

        //check whether the wizard or the sacrifice is closer
        Vector2Int wizardDistanceVector = new Vector2Int(Mathf.RoundToInt(transform.position.x - player.transform.position.x),
            Mathf.FloorToInt(transform.position.y - player.transform.position.y));

        Vector2Int sacrificeDistanceVector = new Vector2Int(Mathf.RoundToInt(transform.position.x - sacrifice.transform.position.x),
            Mathf.FloorToInt(transform.position.y - sacrifice.transform.position.y));

        float distanceFromWizard = wizardDistanceVector.magnitude;
        float distanceFromSacrifice = sacrificeDistanceVector.magnitude;

        // if cursed skip movement
        if (!isCursed)
        {
            // if an enemy is ranged, move them
            if(ranged)
            {

                if (distanceFromWizard <= distanceFromSacrifice)
                    moveRanged(player.transform.position);
                else
                    moveRanged(sacrifice.transform.position);
            }
            // if a melee enemy or if far from player, move towards player
            else {
                targetPosition.x = Mathf.Round(moveTo.x);
                targetPosition.y = Mathf.Floor(moveTo.y) + 0.5f;
                transform.position = targetPosition;
            }

        }

        yield return new WaitForSeconds(0.5f);

        //attack goes here
        var attackFrom = new Vector3Int(Mathf.RoundToInt(targetPosition.x), Mathf.FloorToInt(targetPosition.y), 0);
        if (ranged)
        {
            if (distanceFromWizard <= distanceFromSacrifice)
                rangedAttack(player);
            else
                rangedAttack(sacrifice);
        }
        else
            EnemyMelee(attackFrom);

        //end turn
        EndTurn();
        StopCoroutine("WaitAndMove");
    }

    private void rangedAttack(GameObject player)
    {
        //Finds distance in relation to player
        Vector2Int distanceFromPlayer = new Vector2Int(Mathf.RoundToInt(transform.position.x - player.transform.position.x),
            Mathf.FloorToInt(transform.position.y - player.transform.position.y));

        //Check to see if there is a rock in the way
        var tiles = GameTiles.instance.obstacleTiles;
        bool XInLine = false;
        bool YInLine = false;
        foreach(var tilePair in tiles)
        {
            Vector3 tilePosition = tilePair.Key;
            WorldTile _tile = tilePair.Value;
            if (tilePosition.x == player.transform.position.x && _tile.Name.Equals("rock") && distanceFromPlayer.y > Mathf.RoundToInt(transform.position.x - tilePosition.y))
                XInLine = true;
            else if (tilePosition.y - .5f == player.transform.position.y && _tile.Name.Equals("rock") && distanceFromPlayer.x > Mathf.RoundToInt(transform.position.x - tilePair.Key.x))
                YInLine = true;
        }

        //If there is no rock in the way, deal damage
        if ((distanceFromPlayer.x == 0 && !XInLine) || (distanceFromPlayer.y == 0 && !YInLine))
            EnemyDoDamage(new Vector3Int(Mathf.RoundToInt(player.transform.position.x), Mathf.RoundToInt(player.transform.position.y), 0), damage);
    }

    private void moveRanged(Vector3 playerPosition)
    {
        if (hasMoved)
            return;

        //Finds distance in relation to wizard
        Vector2Int distanceFromPlayer = new Vector2Int(Mathf.RoundToInt(transform.position.x - playerPosition.x),
            Mathf.FloorToInt(transform.position.y - playerPosition.y));

        //Finds the direction in relation to the wizard that the enemy is
        //If value is positive, the direction is up or right from the player. If it is positiove, the direction is down or left.
        Vector2Int direction = new Vector2Int();
        if (distanceFromPlayer.x == 0)
            direction.x = 0;
        else
            direction.x = distanceFromPlayer.x / distanceFromPlayer.x;

        if (distanceFromPlayer.y == 0)
            direction.y = 0;
        else
            direction.y = distanceFromPlayer.y / distanceFromPlayer.y;

        //If the distance to the player is close, and the enemy has a shot on the player, move away from the player
        //First try x
        if (Math.Abs(distanceFromPlayer.x) < (moveRange + 1) && distanceFromPlayer.y == 0)
        {
            targetPosition.x = playerPosition.x + direction.x * (moveRange + 1);
            targetPosition.y = transform.position.y;
        } 
        //Next try y
        else if (distanceFromPlayer.x == 0 && Math.Abs(distanceFromPlayer.y) < (moveRange + 1))
        {
            targetPosition.x = transform.position.x;
            targetPosition.y = playerPosition.y + direction.y * (moveRange + 1);
        }
        //If neither of those are true, move towards the closest "0" to the player (so the enemy is in a straight line sight to the player)
        else
        {
            if(Math.Abs(distanceFromPlayer.x) < Math.Abs(distanceFromPlayer.y))
            {
                if (Math.Abs(distanceFromPlayer.x) <= moveRange)
                {
                    targetPosition.x = playerPosition.x;
                    targetPosition.y = transform.position.y;
                } 
                else
                {
                    targetPosition.x = transform.position.x + moveRange * -direction.x;
                    targetPosition.y = transform.position.y;
                }
            } 
            else
            {
                if (Math.Abs(distanceFromPlayer.y) <= moveRange)
                {
                    targetPosition.x = transform.position.x;
                    targetPosition.y = playerPosition.y;
                }
                else
                {
                    targetPosition.x = transform.position.x;
                    targetPosition.y = transform.position.y + moveRange * -direction.y;
                }
            }
        }

        //Check to make sure the target position is not outside the game boundaries
        if (targetPosition.x > 4)
            targetPosition.x = 4;
        else if (targetPosition.x < -4)
            targetPosition.x = -4;
        if (targetPosition.y > 4.5)
            targetPosition.y = 4.5f;
        else if (targetPosition.y < -3.5)
            targetPosition.y = -3.5f;

        // Make sure enemy's do not hit rocks or water
        var tiles = GameTiles.instance.obstacleTiles;
        if (tiles.TryGetValue(new Vector3(Mathf.Round(targetPosition.x), Mathf.Floor(targetPosition.y), 0), out _tile))
        {
            //The enemy doesn't move so it doesn't stay on top of the rock/water
            targetPosition = transform.position;
        }

        transform.position = targetPosition;
        hasMoved = true;
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
        hasMoved = false;
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
