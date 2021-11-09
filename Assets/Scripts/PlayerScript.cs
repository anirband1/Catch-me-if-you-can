using UnityEngine;

// Spawn the enemy and player after launching python script

public class PlayerScript : MonoBehaviour
{
    Vector3 playerPos = Vector3.zero;
    Vector2 playerBounds;
    [SerializeField] Camera mainCamera;
    bool running, quitApp = false;
    float[] fArray;
    float objectWidth, objectHeight, maxX, maxY;


    NetworkManager networkManager;

    void Start()
    {
        objectWidth = GetComponent<SpriteRenderer>().size.x;
        objectHeight = GetComponent<SpriteRenderer>().size.y;

        playerBounds = FindObjectOfType<GameManager>().screenBounds;
        networkManager = FindObjectOfType<NetworkManager>();

        maxX = playerBounds.x - objectWidth / 2;
        maxY = playerBounds.y - objectHeight / 2;
    }

    void Update()
    {
        Vector2 playerPos = networkManager.receivedPos;

        float tempX = Mathf.Clamp(playerPos.x, -maxX, maxX);
        float tempY = Mathf.Clamp(playerPos.y, -maxY, maxY);

        transform.position = new Vector2(tempX, tempY);
    }
}
