using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class ShadowStep : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public ToolTip toolTip;
    public GameObject playerTurnHandler;
    [SerializeField] GameObject animationObject;

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
            //Add one block of movement to wizard and sarifice for the turn
            GameObject sac = GameObject.FindGameObjectWithTag("Sacrifice");
            gameObject.GetComponent<PlayerMovement>().moveRange += 1;
            sac.GetComponent<PlayerMovement>().moveRange += 1;
            FindObjectOfType<AudioManager>().PlaySound("Shadow Step"); // Play Shadow Step Sound
            Instantiate(animationObject, sac.transform.position + new Vector3(0, -.1f, -1), sac.transform.rotation, sac.transform);
            Instantiate(animationObject, gameObject.transform.position + new Vector3(0, -.1f, -1), gameObject.transform.rotation, gameObject.transform);
            EndAttack();
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
    public void OnPointerEnter(PointerEventData eventData)
    {
        toolTip.DisplayText("ShadowStep");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTip.HideText();
    }
}
