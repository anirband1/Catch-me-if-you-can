using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Camera mainCamera;

    [HideInInspector] public Vector2 screenBounds;
    void Awake()
    {
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
    }
}
