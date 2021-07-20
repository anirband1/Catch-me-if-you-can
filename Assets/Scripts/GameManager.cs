using UnityEngine;
using System.IO;
using System.Diagnostics;

public class GameManager : MonoBehaviour
{
    [SerializeField] Camera mainCamera;

    [HideInInspector] public Vector2 screenBounds;
    void Awake()
    {
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
    }

    void Start()
    {
        string appPath = Directory.GetCurrentDirectory() + "/Hand_tracker.app";

        Process.Start(appPath);
    }
}
