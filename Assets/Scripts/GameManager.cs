using UnityEngine;
using System.Diagnostics;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] Camera mainCamera;

    [HideInInspector] public Vector2 screenBounds;

    public string execPath;

    string appPath;
    void Awake()
    {
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
    }

    void Start()
    {
        appPath = Application.streamingAssetsPath + "/App.app"; // -- W A R N I N G -- check extension

        execPath = ExecuteBashCommand("echo $PWD");

        UnityEngine.Debug.Log(execPath);
        // Process.Start(appPath);
    }

    /*

    void TestRun()
    {
        // lets say we want to run this command:    
        //  t=$(echo 'this is a test'); echo "$t" | grep -o 'is a'
        var output = ExecuteBashCommand("t=$(echo 'this is a test'); echo \"$t\" | grep -o 'is a'");

        // output the result
        Console.WriteLine(output);
    }
    */

    static string ExecuteBashCommand(string command)
    {
        // according to: https://stackoverflow.com/a/15262019/637142
        // thans to this we will pass everything as one command
        command = command.Replace("\"", "\"\"");

        var proc = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = "-c \"" + command + "\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = false
            }
        };

        proc.Start();
        proc.WaitForExit();

        return proc.StandardOutput.ReadToEnd();
    }
}
