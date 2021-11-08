using UnityEngine;

// Spawn the enemy and player after launching python script

public class PlayerScript : MonoBehaviour
{
    Vector2 playerPos;
    Vector2 playerBounds;

    float objectWidth, objectHeight, maxX, maxY;

    NetworkManager networkManager;
    void Start()
    {
        networkManager = FindObjectOfType<NetworkManager>();

        objectWidth = GetComponent<SpriteRenderer>().size.x;
        objectHeight = GetComponent<SpriteRenderer>().size.y;

        playerBounds = FindObjectOfType<GameManager>().screenBounds;

        maxX = playerBounds.x - objectWidth;
        maxY = playerBounds.y - objectHeight;

        playerPos = Vector2.zero;
    }

    void Update()
    {
        Vector2 receivedPos = networkManager.receivedPos;

        float tempX = Mathf.Clamp(-maxX, maxX * receivedPos.x, maxX);
        float tempY = Mathf.Clamp(-maxY, -(maxY * receivedPos.y), maxY);

        playerPos = new Vector2(tempX, tempY);
        transform.position = playerPos;
    }
}
