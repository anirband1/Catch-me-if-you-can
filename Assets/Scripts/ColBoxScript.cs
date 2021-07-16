using UnityEngine;

public class ColBoxScript : MonoBehaviour
{
    [HideInInspector] public bool willHit = false;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            willHit = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            willHit = false;
        }
    }
}
