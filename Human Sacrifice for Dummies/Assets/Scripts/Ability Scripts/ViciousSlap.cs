using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class ViciousSlap : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public ToolTip toolTip;
    public GameObject playerTurnHandler;

    public bool isTurn = false;
    public bool hasSearched = false;
    public Vector2 startPosition;
    public Vector2 targetPosition;
    public HUD hud;
    private WorldTile _tile;

    List<Vector3Int> neighbors = new List<Vector3Int>();

    // Update is called once per frame
    void Update()
    {
        if (isTurn)
        {
            UpdatePosition();

            var tiles = GameTiles.instance.tiles;
            var worldPoint = new Vector3Int(Mathf.RoundToInt(startPosition.x), Mathf.FloorToInt(startPosition.y), 0);

            if (!hasSearched)
            {
                var attackStart = new Vector3Int(Mathf.RoundToInt(startPosition.x), Mathf.FloorToInt(startPosition.y), 0);
                FindMeleeTargets(attackStart);

                // color start tile
                if (tiles.TryGetValue(worldPoint, out _tile))
                {
                    _tile.TilemapMember.SetTileFlags(_tile.LocalPlace, TileFlags.None);
                    _tile.TilemapMember.SetColor(_tile.LocalPlace, Color.magenta);
                }

                // color target tiles
                foreach (Vector3Int pos in neighbors)
                {
                    if (tiles.TryGetValue(pos, out _tile))
                    {
                        _tile.TilemapMember.SetTileFlags(_tile.LocalPlace, TileFlags.None);
                        _tile.TilemapMember.SetColor(_tile.LocalPlace, Color.blue);
                    }
                }

                hasSearched = true;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                // used for checking if your clicking on the grid
                Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var mousePoint = new Vector3Int(Mathf.RoundToInt(point.x), Mathf.FloorToInt(point.y), 0);

                // position centered on grid
                targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPosition.x = Mathf.Round(targetPosition.x);
                targetPosition.y = Mathf.Floor(targetPosition.y) + 0.5f;

                // if you click an attackable target, attack that target
                foreach (Vector3Int pos in neighbors)
                {
                    if (pos == mousePoint)
                    {
                        // damage target
                        FindObjectOfType<AudioManager>().PlaySound("Staff Whack");
                        pushEnemy(pos, pos - worldPoint);
                        EndAttack();
                    }
                }

                // if you click yourself, skip attack
                if (worldPoint == mousePoint)
                {
                    FindObjectOfType<AudioManager>().PlaySound("Staff Whack");
                    EndAttack();
                    playerTurnHandler.GetComponent<PlayerTurnHandlerScript>().wizardHasAttacked = false;
                }
            }
        }
    }

    private void pushEnemy(Vector3Int enemyPos, Vector3Int direction)
    {
        Vector2 tPos;
        tPos.x = Mathf.Round(enemyPos.x);
        tPos.y = Mathf.Floor(enemyPos.y) + 0.5f;

        // damage target at position
        var objects = FindObjectsOfType<GameObject>();
        var tiles = GameTiles.instance.obstacleTiles;

        foreach (GameObject go in objects)
        {
            //check if object is in enemy layer
            if (go.layer == 11)
            {
                if (tPos.Equals(go.transform.position))
                {
                    Vector3Int pushPosition = enemyPos + new Vector3Int(direction.x, direction.y, 0);
                    go.transform.position = enemyPos + new Vector3(direction.x, direction.y + .5f, 0);
                    if (tiles.TryGetValue(pushPosition, out _tile)) {
                        if (_tile.Name.Equals("water"))
                        {
                            DestroyEnemy(pushPosition);
                        }
                        if (_tile.Name.Equals("rock"))
                        {
                            DoDamage(pushPosition);
                            DoDamage(pushPosition);
                            go.transform.position = enemyPos;
                        }
                    } else
                    {
                        DoDamage(pushPosition);
                    }
                }
            }
        }
    }

    void UpdatePosition()
    {
        // make sure start position is centered
        targetPosition.x = Mathf.Round(gameObject.transform.position.x);
        targetPosition.y = Mathf.Floor(gameObject.transform.position.y) + 0.5f;
        startPosition = targetPosition;
    }

    void FindMeleeTargets(Vector3Int currentPos)
    {
        // make list to be returned
        neighbors.Clear();

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
            if (_tile.Occupied)
            {
                if (!_tile.Player)
                {
                    neighbors.Add(up);
                }
            }

        }

        if (tiles.TryGetValue(left, out _tile))
        {
            if (_tile.Occupied)
            {
                if (!_tile.Player)
                {
                    neighbors.Add(left);
                }
            }
        }

        if (tiles.TryGetValue(down, out _tile))
        {
            if (_tile.Occupied)
            {
                if (!_tile.Player)
                {
                    neighbors.Add(down);
                }
            }
        }

        if (tiles.TryGetValue(right, out _tile))
        {
            if (_tile.Occupied)
            {
                if (!_tile.Player)
                {
                    neighbors.Add(right);
                }
            }
        }
    }

    void EndAttack()
    {
        hud.unlockButtons();
        ClearColors();
        hasSearched = false;
        isTurn = false;
        playerTurnHandler.GetComponent<PlayerTurnHandlerScript>().wizardHasAttacked = true;
    }

    void ClearColors()
    {
        var tiles = GameTiles.instance.tiles;
        var worldPoint = new Vector3Int(Mathf.RoundToInt(startPosition.x), Mathf.FloorToInt(startPosition.y), 0);

        // clear start tile
        if (tiles.TryGetValue(worldPoint, out _tile))
        {
            _tile.TilemapMember.SetTileFlags(_tile.LocalPlace, TileFlags.None);
            _tile.TilemapMember.SetColor(_tile.LocalPlace, Color.white);
        }

        // clear target tiles
        foreach (Vector3Int pos in neighbors)
        {
            if (tiles.TryGetValue(pos, out _tile))
            {
                _tile.TilemapMember.SetTileFlags(_tile.LocalPlace, TileFlags.None);
                _tile.TilemapMember.SetColor(_tile.LocalPlace, Color.white);
            }
        }
    }

    void DoDamage(Vector3Int position)
    {
        Vector2 tPos;
        tPos.x = Mathf.Round(position.x);
        tPos.y = Mathf.Floor(position.y) + 0.5f;

        // damage target at position
        var objects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject go in objects)
        {
            //check if object is in enemy layer
            if (go.layer == 11)
            {
                if (tPos.Equals(go.transform.position))
                {
                    go.GetComponent<Move>().health -= 1;
                }
            }
        }
    }

    void DestroyEnemy(Vector3Int position)
    {
        Vector2 tPos;
        tPos.x = Mathf.Round(position.x);
        tPos.y = Mathf.Floor(position.y) + 0.5f;

        // damage target at position
        var objects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject go in objects)
        {
            //check if object is in enemy layer
            if (go.layer == 11)
            {
                if (tPos.Equals(go.transform.position))
                {
                    go.GetComponent<Move>().health = 0;
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        toolTip.DisplayText("ViciousSlap");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTip.HideText();
    }

}
