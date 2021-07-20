using UnityEngine;

public class TPScript : MonoBehaviour
{

    Vector2 boxPos;
    Vector2 playerBounds;
    [SerializeField] GameObject player;

    float timer, timeBeforeChange;

    float objectWidth, objectHeight, playerWidth, playerHeight;

    float minX, minY, maxX, maxY;



    void Start()
    {

        // Shift screen bounds code to GameManager

        objectWidth = GetComponent<SpriteRenderer>().size.x;
        objectHeight = GetComponent<SpriteRenderer>().size.y;

        playerWidth = player.GetComponent<SpriteRenderer>().size.x;
        playerWidth = player.GetComponent<SpriteRenderer>().size.y;

        timeBeforeChange = Random.Range(0.5f, 2f);

        playerBounds = FindObjectOfType<GameManager>().screenBounds;

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
        else
        {
            Debug.Log("enemy collision");
        }
    }

    void ShiftPos()
    {
        boxPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

        if (Physics2D.OverlapBox(boxPos, new Vector2(1, 1), 0) != null)
        {
            ShiftPos();
        }

        transform.position = boxPos;

        timeBeforeChange = Random.Range(0.5f, 2f);
    }
}
