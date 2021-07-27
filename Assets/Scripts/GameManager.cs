using UnityEngine;
using System.Diagnostics;

public class GameManager : MonoBehaviour
{
    [SerializeField] Camera mainCamera;

    [HideInInspector] public Vector2 screenBounds;

    string appPath;
    void Awake()
    {
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
    }

    void Start()
    {
        appPath = Application.streamingAssetsPath + "/App.exe"; // -- W A R N I N G -- check extension

        // Process.Start(appPath);
    }
}
