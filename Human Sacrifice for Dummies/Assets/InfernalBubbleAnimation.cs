using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfernalBubbleAnimation : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
