using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Move : MonoBehaviour
{
    // using worldtile to be able to access info like is a tile occupied
    private WorldTile _tile;

    // this will record the distances from start to all positions on map
    public Dictionary<Vector3Int, int> distanceChart = new Dictionary<Vector3Int, int>();

    // this will record the / a shortest path from start to all positions on map
    public Dictionary<Vector3Int, Vector3Int> pathChart = new Dictionary<Vector3Int, Vector3Int>();

    public void BFS(Vector3Int startPos)
    {
        Vector3Int currentPos = startPos;
        Queue<Vector3Int> frontier = new Queue<Vector3Int>();

        distanceChart.Clear();
        pathChart.Clear();

        frontier.Enqueue(currentPos);
        distanceChart.Add(currentPos, 0);
        pathChart.Add(currentPos, startPos);

        var tiles = GameTiles.instance.tiles;
        var worldPoint = new Vector3Int(Mathf.RoundToInt(currentPos.x), Mathf.FloorToInt(currentPos.y), 0);

        // colors the starting tile
        if (tiles.TryGetValue(worldPoint, out _tile))
        {
            _tile.TilemapMember.SetTileFlags(_tile.LocalPlace, TileFlags.None);
            _tile.TilemapMember.SetColor(_tile.LocalPlace, Color.red);
        }

        // if i stil have places to visit keep going
        while (frontier.Count > 0)
        {
            // get current position
            currentPos = frontier.Dequeue();

            // iterate through list of all neighbors
            foreach (Vector3Int nextPos in GetNeighbors(currentPos))
            {
                // has the tile been processed yet
                if (distanceChart.ContainsKey(nextPos) == false)
                {
                    frontier.Enqueue(nextPos);
                    distanceChart.Add(nextPos, 1 + distanceChart[currentPos]);
                    pathChart.Add(nextPos, currentPos);

                    // colors tiles that are within 3 blocks
                    if (1 + distanceChart[currentPos] <= 3)
                    {
                        if (tiles.TryGetValue(nextPos, out _tile))
                        {
                            _tile.TilemapMember.SetTileFlags(_tile.LocalPlace, TileFlags.None);
                            _tile.TilemapMember.SetColor(_tile.LocalPlace, Color.red);
                        }
                    }

                    // more logic can go here
                    // example: ai stops searching when player is found
                }
            }
        }
    }

    // looks up down left and right and checks if there is a grass tile there.
    public List<Vector3Int> GetNeighbors(Vector3Int currentPos)
    {
        // make list to be returned
        List<Vector3Int> neighbors = new List<Vector3Int>();

        // get position where neightbors would be
        Vector3Int up = new Vector3Int(currentPos.x, currentPos.y + 1, 0);
        Vector3Int down = new Vector3Int(currentPos.x, currentPos.y - 1, 0);
        Vector3Int left = new Vector3Int(currentPos.x - 1, currentPos.y, 0);
        Vector3Int right = new Vector3Int(currentPos.x + 1, currentPos.y, 0);

        var tiles = GameTiles.instance.tiles;

        // if neighbor exists check if space is occupied
        // if not occupied add to neighbors list
        if (tiles.TryGetValue(up, out _tile))
        {
            if (!_tile.Occupied) { neighbors.Add(up); }
        }

        if (tiles.TryGetValue(down, out _tile))
        {
            if (!_tile.Occupied) { neighbors.Add(down); }
        }

        if (tiles.TryGetValue(left, out _tile))
        {
            if (!_tile.Occupied) { neighbors.Add(left); }
        }

        if (tiles.TryGetValue(right, out _tile))
        {
            if (!_tile.Occupied) { neighbors.Add(right); }
        }

        // return list
        return neighbors;
    }

    public Vector3 BFS2(Vector3Int startPos)
    {
        Vector3Int currentPos = startPos;
        Queue<Vector3Int> frontier = new Queue<Vector3Int>();

        distanceChart.Clear();
        pathChart.Clear();

        frontier.Enqueue(currentPos);
        distanceChart.Add(currentPos, 0);
        pathChart.Add(currentPos, startPos);

        var tiles = GameTiles.instance.tiles;
        var worldPoint = new Vector3Int(Mathf.RoundToInt(currentPos.x), Mathf.FloorToInt(currentPos.y), 0);

        // if i stil have places to visit keep going
        while (frontier.Count > 0)
        {
            // get current position
            currentPos = frontier.Dequeue();

            // iterate through list of all neighbors
            foreach (Vector3Int nextPos in GetNeighbors2(currentPos))
            {
                // has the tile been processed yet
                if (distanceChart.ContainsKey(nextPos) == false)
                {
                    frontier.Enqueue(nextPos);
                    distanceChart.Add(nextPos, 1 + distanceChart[currentPos]);
                    pathChart.Add(nextPos, currentPos);

                    if (tiles.TryGetValue(nextPos, out _tile))
                    {
                        if (_tile.Occupied)
                        {
                            if(_tile.Player)
                            {
                                Vector3Int tempPos = nextPos;
                                while (distanceChart[pathChart[tempPos]] > 3)
                                {
                                    tempPos = pathChart[tempPos];
                                }
                                return pathChart[tempPos];
                            }
                        }
                    }
                }
            }
        }
        return startPos;
    }

    

    // looks up down left and right and checks if there is a grass tile there.
    public List<Vector3Int> GetNeighbors2(Vector3Int currentPos)
    {
        // make list to be returned
        List<Vector3Int> neighbors = new List<Vector3Int>();

        // get position where neightbors would be
        Vector3Int up = new Vector3Int(currentPos.x, currentPos.y + 1, 0);
        Vector3Int down = new Vector3Int(currentPos.x, currentPos.y - 1, 0);
        Vector3Int left = new Vector3Int(currentPos.x - 1, currentPos.y, 0);
        Vector3Int right = new Vector3Int(currentPos.x + 1, currentPos.y, 0);

        var tiles = GameTiles.instance.tiles;

        // if neighbor exists check if space is occupied
        // if not occupied add to neighbors list
        if (tiles.TryGetValue(up, out _tile))
        {
            if (!_tile.Occupied)
            {
                neighbors.Add(up);
            }
            else if (_tile.Player)
            {
                neighbors.Add(up);
                return neighbors;
            }
        }

        if (tiles.TryGetValue(down, out _tile))
        {
            if (!_tile.Occupied)
            {
                neighbors.Add(down);
            }
            else if (_tile.Player)
            {
                neighbors.Add(down);
                return neighbors;
            }
        }

        if (tiles.TryGetValue(left, out _tile))
        {
            if (!_tile.Occupied)
            {
                neighbors.Add(left);
            }
            else if (_tile.Player)
            {
                neighbors.Add(left);
                return neighbors;
            }
        }

        if (tiles.TryGetValue(right, out _tile))
        {
            if (!_tile.Occupied)
            {
                neighbors.Add(right);
            }
            else if (_tile.Player)
            {
                neighbors.Add(right);
                return neighbors;
            }
        }

        // return list
        return neighbors;
    }
}
