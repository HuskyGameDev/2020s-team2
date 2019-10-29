using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alter : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if the thing colliding with the alter has tag sacrifice
        if (collision.CompareTag("Sacrifice"))
        {
            // you win code goes here.
            print("You won by getting the Sacrifice to the alter");
        }
    }
}
