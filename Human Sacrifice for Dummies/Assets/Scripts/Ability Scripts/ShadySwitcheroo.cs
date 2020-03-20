using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShadySwitcheroo : MonoBehaviour
{
    public GameObject playerTurnHandler;

    public bool isTurn = false;
    public bool hasSearched = false;
    public Vector2 startPosition;
    public Vector2 targetPosition;
    public Vector2 tempPosition;
    public HUD hud;
    private WorldTile _tile;

    // Update is called once per frame
    void Update()
    {
        if (isTurn)
        {
            UpdatePosition();

            var tiles = GameTiles.instance.tiles;
            var worldPoint1 = new Vector3Int(Mathf.RoundToInt(startPosition.x), Mathf.FloorToInt(startPosition.y), 0);
            var worldPoint2 = new Vector3Int(Mathf.RoundToInt(targetPosition.x), Mathf.FloorToInt(targetPosition.y), 0);

            if (!hasSearched)
            {
                // color wizard tile
                if (tiles.TryGetValue(worldPoint1, out _tile))
                {
                    _tile.TilemapMember.SetTileFlags(_tile.LocalPlace, TileFlags.None);
                    _tile.TilemapMember.SetColor(_tile.LocalPlace, Color.magenta);
                }
                // color sac tile
                if (tiles.TryGetValue(worldPoint2, out _tile))
                {
                    _tile.TilemapMember.SetTileFlags(_tile.LocalPlace, TileFlags.None);
                    _tile.TilemapMember.SetColor(_tile.LocalPlace, Color.magenta);
                }

                hasSearched = true;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                // used for checking if your clicking on the grid
                Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var mousePoint = new Vector3Int(Mathf.RoundToInt(point.x), Mathf.FloorToInt(point.y), 0);

                // position centered on grid
                tempPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                tempPosition.x = Mathf.Round(tempPosition.x);
                tempPosition.y = Mathf.Floor(tempPosition.y) + 0.5f;

                // if you click the sacrifice, switch the positions
                if (worldPoint2 == mousePoint)
                {
                    GameObject sac = GameObject.FindGameObjectWithTag("Sacrifice");
                    sac.transform.position = startPosition;
                    gameObject.transform.position = targetPosition;
                    FindObjectOfType<AudioManager>().PlaySound("Shady Switcheroo"); // Play Shady Switcheroo Sound
                    EndAttack();
                }
            }
        }
    }

    void UpdatePosition()
    {
        // startPosition = wizard, targetPosition = Sacrifice
        startPosition.x = Mathf.Round(gameObject.transform.position.x);
        startPosition.y = Mathf.Floor(gameObject.transform.position.y) + 0.5f;

        GameObject sac = GameObject.FindGameObjectWithTag("Sacrifice");
        targetPosition.x = Mathf.Round(sac.transform.position.x);
        targetPosition.y = Mathf.Floor(sac.transform.position.y) + 0.5f;
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
        var worldPoint1 = new Vector3Int(Mathf.RoundToInt(startPosition.x), Mathf.FloorToInt(startPosition.y), 0);
        var worldPoint2 = new Vector3Int(Mathf.RoundToInt(targetPosition.x), Mathf.FloorToInt(targetPosition.y), 0);

        // clear wizard tile
        if (tiles.TryGetValue(worldPoint1, out _tile))
        {
            _tile.TilemapMember.SetTileFlags(_tile.LocalPlace, TileFlags.None);
            _tile.TilemapMember.SetColor(_tile.LocalPlace, Color.white);
        }

        // clear sac tile
        if (tiles.TryGetValue(worldPoint2, out _tile))
        {
            _tile.TilemapMember.SetTileFlags(_tile.LocalPlace, TileFlags.None);
            _tile.TilemapMember.SetColor(_tile.LocalPlace, Color.white);
        }

    }

}
