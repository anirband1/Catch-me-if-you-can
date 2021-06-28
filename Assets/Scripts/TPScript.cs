using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPScript : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    Vector2 playerBounds;
    Vector2 boxPos;

    float timer, timeBeforeChange;

    float objectWidth, objectHeight;

    float minX, minY, maxX, maxY;

    void Start()
    {
        objectWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        objectHeight = GetComponent<SpriteRenderer>().bounds.size.y;
        playerBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width - objectWidth / 2, Screen.height - objectHeight / 2, 0));
        timeBeforeChange = Random.Range(0.5f, 2f);

        ShiftPos();
    }


    void Update()
    {
        if (timer >= timeBeforeChange)
        {
            ShiftPos();
            timer = 0f;
        }

        timer += Time.deltaTime;
    }

    void ShiftPos()
    {
        minX = -playerBounds.x;
        maxX = playerBounds.x;
        minY = -playerBounds.y;
        maxY = playerBounds.y;

        boxPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

        transform.position = boxPos;

        timeBeforeChange = Random.Range(0.5f, 2f);
    }
}
