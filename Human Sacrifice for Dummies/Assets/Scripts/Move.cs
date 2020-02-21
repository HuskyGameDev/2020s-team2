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

    public int health;
    public int moveRange;

    //used for player movement
    public void BFS(Vector3Int startPos)
    {
        
        Vector3Int currentPos = startPos;
        Queue<Vector3Int> frontier = new Queue<Vector3Int>();

        distanceChart.Clear(); //this chart shows distance from startPos
        pathChart.Clear(); //this chart shows the path to travel from startPos to a specific tile

        frontier.Enqueue(currentPos);
        distanceChart.Add(currentPos, 0);
        pathChart.Add(currentPos, startPos);

        //intialization
        var tiles = GameTiles.instance.tiles;
        var worldPoint = new Vector3Int(Mathf.RoundToInt(currentPos.x), Mathf.FloorToInt(currentPos.y), 0);

        // colors the starting tile
        if (tiles.TryGetValue(worldPoint, out _tile))
        {
            _tile.TilemapMember.SetTileFlags(_tile.LocalPlace, TileFlags.None);
            _tile.TilemapMember.SetColor(_tile.LocalPlace, Color.red);
        }

        // if I stil have places to visit keep going
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
                    if (1 + distanceChart[currentPos] <= moveRange)
                    {
                        if (tiles.TryGetValue(nextPos, out _tile))
                        {
                            _tile.TilemapMember.SetTileFlags(_tile.LocalPlace, TileFlags.None);
                            _tile.TilemapMember.SetColor(_tile.LocalPlace, Color.red);
                        }
                    }
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

    //used for enemy movement
    public Vector3 BFS2(Vector3Int startPos)
    {
        Vector3Int currentPos = startPos;
        Queue<Vector3Int> frontier = new Queue<Vector3Int>();

        distanceChart.Clear(); //this chart shows distance from startPos
        pathChart.Clear(); //this chart shows the path to travel from startPos to a specific tile

        frontier.Enqueue(currentPos);
        distanceChart.Add(currentPos, 0);
        pathChart.Add(currentPos, startPos);
        // intialization
        var tiles = GameTiles.instance.tiles;
        var worldPoint = new Vector3Int(Mathf.RoundToInt(currentPos.x), Mathf.FloorToInt(currentPos.y), 0);

        // if I stil have places to visit keep going
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

                    //Searching for the player
                    if (tiles.TryGetValue(nextPos, out _tile))
                    {
                        if (_tile.Occupied)
                        {
                            if(_tile.Player)
                            { //this finds the spot that the enemy wants to move to and returns that
                                Vector3Int tempPos = nextPos;
                                while (distanceChart[pathChart[tempPos]] > moveRange)
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
        return startPos; //if a path to the player is not found, enemy will not move
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

    //Breadth First Seacrh for ranged units.
    public Vector3 BFS3(Vector3Int startPos)
    {
        Vector3Int currentPos = startPos;
        Queue<Vector3Int> frontier = new Queue<Vector3Int>();

        distanceChart.Clear(); //this chart shows distance from startPos
        pathChart.Clear(); //this chart shows the path to travel from startPos to a specific tile

        frontier.Enqueue(currentPos);
        distanceChart.Add(currentPos, 0);
        pathChart.Add(currentPos, startPos);
        // intialization
        var tiles = GameTiles.instance.tiles;
        var worldPoint = new Vector3Int(Mathf.RoundToInt(currentPos.x), Mathf.FloorToInt(currentPos.y), 0);

        // if I stil have places to visit keep going
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

                    //Searching for the player
                    if (tiles.TryGetValue(nextPos, out _tile))
                    {
                        if (_tile.Occupied)
                        {
                            if (_tile.Player)
                            { //this finds the spot that the enemy wants to move to and returns that
                                Vector3Int tempPos = nextPos;
                                while (distanceChart[pathChart[tempPos]] > moveRange)
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
        return startPos; //if a path to the player is not found, enemy will not move
    }
}
