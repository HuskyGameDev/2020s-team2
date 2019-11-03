using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnSystemScript : MonoBehaviour
{
    public List<TurnClass> playersGroup = null;

    // Start is called before the first frame update
    void Start()
    {
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
    }

    //allows the next entity in the turn order to go
    void UpdateTurns()
    {
        for(int i = 0;i<playersGroup.Count;i++)
        {
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
