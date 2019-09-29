using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 targetPosition;

    private void Start()
    {

        targetPosition.x = Mathf.Floor(gameObject.transform.position.x) + 0.5f;
        targetPosition.y = Mathf.Floor(gameObject.transform.position.y) + 0.5f;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.x = Mathf.Floor(targetPosition.x) + 0.5f;
            targetPosition.y = Mathf.Floor(targetPosition.y) + 0.5f;
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * 5);
    }
}
