using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViciousSlapAnimation : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
