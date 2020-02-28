using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class turnSystemScript : MonoBehaviour
{
    public List<TurnClass> playersGroup = null;

    public GameObject camera;
    public List<Button> buttons;

    // Start is called before the first frame update
    void Start()
    {

        Camera cam = camera.GetComponent<Camera>();
        Button[] btns = cam.GetComponentsInChildren<Button>(true);
        for (int i = 0; i < btns.Length; i++)
        {
            buttons.Add(btns[i]);
        }
        ResetTurns();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTurns();
    }

    //cleans up variables to prepare for next turn
    void ResetTurns() 
    {
        for (int i = 0; i < playersGroup.Count; i++)
        {
            if (i == 0)
            {
                playersGroup[i].isTurn = true;
                playersGroup[i].wasTurnPrev = false;
            }
            else
            {
                playersGroup[i].isTurn = false;
                playersGroup[i].wasTurnPrev = false;
            }
        }
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].interactable = true;
        }

        // clear infernal bubble
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().bubble = false;
        GameObject.FindGameObjectWithTag("Sacrifice").GetComponent<PlayerMovement>().bubble = false;
    }

    //allows the next entity in the turn order to go
    void UpdateTurns()
    {
        for(int i = 0;i<playersGroup.Count;i++)
        {
            if (playersGroup[i].playerGameObject == null)
            {
                // game object was destroyed
                playersGroup[i].wasTurnPrev = true;
            }

            if (!playersGroup[i].wasTurnPrev)
            {
                playersGroup[i].isTurn = true;
                break;
            }
            else if (i == playersGroup.Count -1 && playersGroup[i].wasTurnPrev)
            {
                ResetTurns();
            }
        }
    }
}

[System.Serializable]
public class TurnClass
{
    public GameObject playerGameObject;
    public bool isTurn = false;
    public bool wasTurnPrev = false;


}
