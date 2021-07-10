using UnityEngine;

public class TPScript : MonoBehaviour
{
    // Game related declarations
    [SerializeField] Camera mainCamera;
    Vector2 playerBounds, boxPos;
    [SerializeField] GameObject player;

    float timer, timeBeforeChange;

    float objectWidth, objectHeight;

    float minX, minY, maxX, maxY;



    void Start()
    {
        objectWidth = GetComponent<SpriteRenderer>().size.x;
        objectHeight = GetComponent<SpriteRenderer>().size.y;

        playerBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        timeBeforeChange = Random.Range(0.5f, 2f);

        minX = -(playerBounds.x - objectWidth);
        maxX = playerBounds.x - objectWidth;
        minY = -(playerBounds.y - objectHeight);
        maxY = playerBounds.y - objectHeight;

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

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }


    void ShiftPos()
    {
        boxPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

        if ((new Vector2(player.transform.position.x, player.transform.position.y) - boxPos).sqrMagnitude <= 1.1 * 1.1) // Change magic numbers
        {
            ShiftPos();
        }

        transform.position = boxPos;

        timeBeforeChange = Random.Range(0.5f, 2f);
    }
}
