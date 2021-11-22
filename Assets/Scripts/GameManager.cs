using UnityEngine;
using System.Diagnostics;
using System.Collections;
using System;

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
        appPath = Application.streamingAssetsPath + "/App.app"; // -- W A R N I N G -- check extension

        // execPath = ExecuteBashCommand("./main.py");
        // StartCoroutine(BashMethodOne("main.py"));
        BashMethodOne();

        // UnityEngine.Debug.Log(execPath);

        // FindObjectOfType<Path>().gmPath = execPath;
        // FindObjectOfType<Path>().changed = true;

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

    void BashMethodOne()
    {
        // ProcessStartInfo startInfo = new ProcessStartInfo("/bin/bash");
        // startInfo.WorkingDirectory = "/";
        // startInfo.UseShellExecute = false;
        // startInfo.RedirectStandardInput = true;
        // startInfo.RedirectStandardOutput = true;

        // Process process = new Process();
        // process.StartInfo = startInfo;
        // process.Start();

        // process.StandardInput.WriteLine("say " + command);
        // process.StandardInput.WriteLine("exit");  // if no exit then WaitForExit will lockup your program
        // process.StandardInput.Flush();

        // string line = process.StandardOutput.ReadLine();

        // process.WaitForExit();
        // yield return null;

        ProcessStartInfo psi = new ProcessStartInfo();
        psi.FileName = "/bin/sh";
        psi.UseShellExecute = false; // maybe set this to true?
        psi.RedirectStandardOutput = true;
        psi.Arguments = Application.streamingAssetsPath + "/ExecutePython.sh";//+ " arg1 arg2 arg3";
        Process p = Process.Start(psi);

        string strOutput = p.StandardOutput.ReadToEnd();
        FindObjectOfType<Path>().gmPath = strOutput;
        FindObjectOfType<Path>().changed = true;
        p.WaitForExit();
    }
}



/*
Application.dataPath
––––––––––––––––––––

The value depends on which platform you are running on:

Unity Editor: <path to project folder>/Assets

Mac player: <path to player app bundle>/Contents

Win/Linux player: <path to executablename_Data folder> (note that most Linux installations will be case-sensitive!)

Windows Store Apps: The absolute path to the player data folder (this folder is read only, use Application.persistentDataPath to save data)

Note that the string returned on a PC will use a forward slash as a folder separator.

For any unlisted platform, run the example script on the target platform to find the dataPath location in the debug log.

*/


// M1:
/*
    ProcessStartInfo psi = new ProcessStartInfo(); 
    psi.FileName = Application.streamingAssetsPath+"/ExecutePython.sh";
    psi.UseShellExecute = true; 
    psi.RedirectStandardOutput = true;
    // psi.Arguments = "arg1 arg2 arg3";

    //psi.Arguments = "test"; 
    Process p = Process.Start(psi); 
    string strOutput = p.StandardOutput.ReadToEnd(); 
    p.WaitForExit(); 
    UnityEngine.Debug.Log(strOutput);
*/

// M2:
/*
    ProcessStartInfo psi = new ProcessStartInfo(); 
    psi.FileName = "/bin/sh";
    psi.UseShellExecute = false; // maybe set this to true?
    psi.RedirectStandardOutput = true;
    psi.Arguments = Application.streamingAssetsPath + "/ExecutePython.sh" //+ " arg1 arg2 arg3";
    Process p = Process.Start(psi); 
*/

// M3:
/*
    private IEnumerator Speak (string command){
   
        ProcessStartInfo startInfo = new ProcessStartInfo("/bin/bash");
        startInfo.WorkingDirectory = "/";
        startInfo.UseShellExecute = false;
        startInfo.RedirectStandardInput = true;
        startInfo.RedirectStandardOutput = true;
 
        Process process = new Process();
        process.StartInfo = startInfo;
        process.Start();
 
        process.StandardInput.WriteLine("say " + command);
        process.StandardInput.WriteLine("exit");  // if no exit then WaitForExit will lockup your program
        process.StandardInput.Flush();
 
        string line = process.StandardOutput.ReadLine();
       
        process.WaitForExit();
        yield return null;
    }
*/
