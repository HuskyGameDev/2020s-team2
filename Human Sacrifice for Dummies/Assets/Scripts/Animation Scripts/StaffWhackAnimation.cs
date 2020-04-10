using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffWhackAnimation : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
