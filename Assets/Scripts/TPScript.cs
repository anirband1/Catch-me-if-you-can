using UnityEngine;

public class TPScript : MonoBehaviour
{

    Vector2 boxPos;
    Vector2 playerBounds;

    float timer, timeBeforeChange;

    float objectWidth, objectHeight;

    float rightX, topY;



    void Start()
    {
        objectWidth = GetComponent<SpriteRenderer>().size.x;
        objectHeight = GetComponent<SpriteRenderer>().size.y;

        timeBeforeChange = Random.Range(0.5f, 2f);

        playerBounds = FindObjectOfType<GameManager>().screenBounds;

        rightX = playerBounds.x - objectWidth;
        topY = playerBounds.y - objectHeight; // by 2

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
        boxPos = new Vector2(Random.Range(-rightX, rightX), Random.Range(-topY, topY));

        if (Physics2D.OverlapBox(boxPos, new Vector2(objectWidth, objectHeight), 0) != null)
        {
            ShiftPos();
        }

        transform.position = boxPos;

        timeBeforeChange = Random.Range(0.5f, 0.6f); // Change max to 2f
    }
}
