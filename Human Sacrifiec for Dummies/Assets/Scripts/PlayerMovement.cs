﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : Move
{
    public bool hasSearched = false;

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
        // make sure start position is centered
        targetPosition.x = Mathf.Round(gameObject.transform.position.x);
        targetPosition.y = Mathf.Floor(gameObject.transform.position.y) + 0.5f;
        startPosition = targetPosition;

        // load turn system
        TurnSystem = GameObject.Find("Turn-basedSystem").GetComponent<turnSystemScript>();

        foreach(TurnClass tc in TurnSystem.playersGroup)
        {
            if(tc.playerGameObject.name == gameObject.name)
            {
                turnClass = tc;
            }
        }
        var tiles = GameTiles.instance.tiles;
        if (tiles.TryGetValue(new Vector3Int(Mathf.RoundToInt(startPosition.x), Mathf.FloorToInt(startPosition.y), 0), out _tile))
        {
            _tile.Occupied = true;
            _tile.Player = true;
        }
    }

    void Update()
    {
        // is it my turn
        isTurn = turnClass.isTurn;

        // check if it is your turn
        if (isTurn)
        {
            // make sure the BFS des not execute every frame
            if (!hasSearched)
            {
                // before starting make sure the center is the current position not the starting position from turn 1
                ChangeStartPosition();

                // convert start position to vector3int for breadth first search
                var bfsStart = new Vector3Int(Mathf.RoundToInt(startPosition.x), Mathf.FloorToInt(startPosition.y), 0);
                BFS(bfsStart);
                hasSearched = true;
            }

            // trigger for movement
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                // used for checking if your clicking on the grid
                Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var worldPoint = new Vector3Int(Mathf.RoundToInt(point.x), Mathf.FloorToInt(point.y), 0);

                // position for character to be centered on grid
                targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPosition.x = Mathf.Round(targetPosition.x);
                targetPosition.y = Mathf.Floor(targetPosition.y) + 0.5f;

                // go through the BFS distance chart
                foreach (KeyValuePair<Vector3Int, int> t in distanceChart)
                {
                    // if the clicked tile is within 3 spaces move there
                    if (t.Key == worldPoint && t.Value <= 3)
                    {

                        transform.position = targetPosition;

                        EndTurn();
                    }
                }

            }
        }
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
        var tiles = GameTiles.instance.tiles;
        if (tiles.TryGetValue(new Vector3Int(Mathf.RoundToInt(startPosition.x), Mathf.FloorToInt(startPosition.y), 0), out _tile))
        {
            _tile.Occupied = false;
            _tile.Player = false;
        }
        ChangeStartPosition();
        if (tiles.TryGetValue(new Vector3Int(Mathf.RoundToInt(startPosition.x), Mathf.FloorToInt(startPosition.y), 0), out _tile))
        {
            _tile.Occupied = true;
            _tile.Player = true;
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

        // BFS will be called again next turn
        hasSearched = false;

        // turn ends
        isTurn = false;
        turnClass.isTurn = isTurn;
        turnClass.wasTurnPrev = true;

    }
}
