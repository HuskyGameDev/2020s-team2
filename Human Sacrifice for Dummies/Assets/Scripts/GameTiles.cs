﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameTiles : MonoBehaviour
{
    public static GameTiles instance;
    public Tilemap Tilemap;
    public Tilemap waterTiles;
    public Tilemap rockTiles;

    public Dictionary<Vector3, WorldTile> tiles;
    public Dictionary<Vector3, WorldTile> obstacleTiles;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        GetWorldTiles();
    }

    // Use this for initialization
    private void GetWorldTiles()
    {
        tiles = new Dictionary<Vector3, WorldTile>();
        foreach (Vector3Int pos in Tilemap.cellBounds.allPositionsWithin)
        {
            var localPlace = new Vector3Int(pos.x, pos.y, pos.z);

            if (!Tilemap.HasTile(localPlace)) continue;
            var tile = new WorldTile
            {
                LocalPlace = localPlace,
                WorldLocation = Tilemap.CellToWorld(localPlace),
                TileBase = Tilemap.GetTile(localPlace),
                TilemapMember = Tilemap,
                Name = localPlace.x + "," + localPlace.y,
                Occupied = false,
                Cost = 1 // TODO: Change this with the proper cost from ruletile
            };

            tiles.Add(tile.LocalPlace, tile);
        }

        obstacleTiles = new Dictionary<Vector3, WorldTile>();
        foreach (Vector3Int pos in waterTiles.cellBounds.allPositionsWithin)
        {
            var localPlace = new Vector3Int(pos.x, pos.y, pos.z);

            if (!waterTiles.HasTile(localPlace)) continue;
            var tile = new WorldTile
            {
                LocalPlace = localPlace,
                WorldLocation = Tilemap.CellToWorld(localPlace),
                TileBase = Tilemap.GetTile(localPlace),
                TilemapMember = Tilemap,
                Name = "water",
                Occupied = false,
                Cost = 1 // TODO: Change this with the proper cost from ruletile
            };
            
            obstacleTiles.Add(tile.LocalPlace, tile);
        }

        foreach (Vector3Int pos in rockTiles.cellBounds.allPositionsWithin)
        {
            var localPlace = new Vector3Int(pos.x, pos.y, pos.z);

            if (!rockTiles.HasTile(localPlace)) continue;
            var tile = new WorldTile
            {
                LocalPlace = localPlace,
                WorldLocation = Tilemap.CellToWorld(localPlace),
                TileBase = Tilemap.GetTile(localPlace),
                TilemapMember = Tilemap,
                Name = "rock",
                Occupied = false,
                Cost = 1 // TODO: Change this with the proper cost from ruletile
            };

            obstacleTiles.Add(tile.LocalPlace, tile);
        }
    }
}