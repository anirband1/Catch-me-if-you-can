using UnityEngine;

public class TPScript : MonoBehaviour
{
    // Game related declarations
    [SerializeField] Camera mainCamera;
    Vector2 playerBounds, boxPos;
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

        // if ((new Vector2(player.transform.position.x, player.transform.position.y) - boxPos).sqrMagnitude <= 1.1 * 1.1) // Change magic numbers
        // {
        //     ShiftPos();
        // }

        // if (Physics.CheckBox(boxPos, new Vector3(objectWidth / 2, objectHeight / 2, 0.01f), Quaternion.identity))
        // {
        //     Debug.Log("Recursive");
        //     ShiftPos();
        // }
        if (Physics2D.OverlapBox(boxPos, new Vector2(1, 1), 0) != null)
        {
            Debug.Log("Recursive");
            ShiftPos();
        }

        transform.position = boxPos;


        timeBeforeChange = Random.Range(0.5f, 0.6f); // Max is 2f

        // Physics.SphereCast
        // Physics.CheckSphere
    }
}
