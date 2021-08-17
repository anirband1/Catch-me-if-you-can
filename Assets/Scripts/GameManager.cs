using UnityEngine;
using System.Diagnostics;
using IronPython.Hosting;
using System.Collections.Generic;

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
        // appPath = Application.streamingAssetsPath + "/Flapper.app"; // -- W A R N I N G -- check extension

        // Process.Start(appPath);
        var engine = Python.CreateEngine();
        ICollection<string> searchPaths = engine.GetSearchPaths();

        //Path to the folder of main.py
        searchPaths.Add(Application.dataPath);

        //Path to the Python standard library
        searchPaths.Add(Application.streamingAssetsPath + "/Lib");
        engine.SetSearchPaths(searchPaths);

        dynamic py = engine.ExecuteFile(Application.streamingAssetsPath + "/Python/main.py");

        py.HandTrackerFunc();

        // dynamic test = py.HandTrackerClass("Codemaker");
        // greeting.text = "Greeting: " + test.display();
        // randomNumber.text = "Random Number: " + test.random_number(1, 5);
    }

    // void testFunction()
    // {
    //     var engine = Python.CreateEngine();
    //     ICollection<string> searchPaths = engine.GetSearchPaths();

    //     //Path to the folder of greeter.py
    //     searchPaths.Add(Application.dataPath);

    //     //Path to the Python standard library
    //     searchPaths.Add(Application.streamingAssetsPath + "/Lib");
    //     engine.SetSearchPaths(searchPaths);

    //     dynamic py = engine.ExecuteFile(Application.streamingAssetsPath + "/Python/main.py");

    //     py.HandTrackerFunc();

    //     // dynamic test = py.HandTrackerClass("Codemaker");
    //     // greeting.text = "Greeting: " + test.display();
    //     // randomNumber.text = "Random Number: " + test.random_number(1, 5);
    // }
}
