using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 targetPosition;
    public Vector2 startPosition;

    private void Start()
    {

        targetPosition.x = Mathf.Round(gameObject.transform.position.x);
        targetPosition.y = Mathf.Floor(gameObject.transform.position.y) + 0.5f;
        startPosition = targetPosition;

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.x = Mathf.Round(targetPosition.x);
            targetPosition.y = Mathf.Floor(targetPosition.y) + 0.5f;
        }
        float magnitude = Vector2.Distance(startPosition, targetPosition);
        if (magnitude <= 3)
        {

        }
        else
        {
            targetPosition = startPosition;
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * 5);
    }
}
