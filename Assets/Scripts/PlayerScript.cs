﻿using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

// Spawn the enemy and player after launching python script

public class PlayerScript : MonoBehaviour
{

    Thread mThread;
    public string connectionIP = "localhost";
    int connectionPort = 9999;
    IPAddress localAdd;
    TcpListener listener;
    TcpClient client;
    Vector3 receivedPos = Vector3.zero;
    Vector2 playerBounds;
    [SerializeField] Camera mainCamera;
    bool running;
    bool quitApp = false;
    float[] fArray;
    float objectWidth, objectHeight, maxX, maxY;

    void Start()
    {
        objectWidth = GetComponent<SpriteRenderer>().size.x;
        objectHeight = GetComponent<SpriteRenderer>().size.y;

        playerBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        maxX = playerBounds.x - objectWidth;
        maxY = playerBounds.y - objectHeight;

        ThreadStart ts = new ThreadStart(GetInfo);
        mThread = new Thread(ts);
        mThread.Start();
    }
    // Update is called once per frame

    void Update()
    {
        transform.position = receivedPos;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            quitApp = true;

            // Redo this if python is not quitting after building
            Application.Quit();
        }
    }

    // Might cause issues for multiple players
    void OnDestroy()
    {
        running = false;
    }

    void GetInfo()
    {
        localAdd = IPAddress.Parse(connectionIP);
        listener = new TcpListener(IPAddress.Any, connectionPort);
        listener.Start();

        client = listener.AcceptTcpClient();

        running = true;
        while (running)
        {
            SendAndReceiveData();
        }
        listener.Stop();
    }

    void SendAndReceiveData()
    {
        NetworkStream nwStream = client.GetStream();
        byte[] buffer = new byte[client.ReceiveBufferSize];
        byte[] myWriteBuffer;

        //---receiving Data from the Host----
        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize); //Getting data in Bytes from Python
        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead); //Converting byte data to string

        if (dataReceived != null)
        {
            //---Using received data---
            fArray = StringToFloatArray(dataReceived); //<-- assigning receivedPos value from Python
            receivedPos = new Vector3(maxX * fArray[0], -(maxY * fArray[1])); // Default values are inverted for y axis


            //---Sending Data to Host----
            if (quitApp)
            {
                myWriteBuffer = Encoding.ASCII.GetBytes("Stop");
                nwStream.Write(myWriteBuffer, 0, myWriteBuffer.Length);
            }

            myWriteBuffer = Encoding.ASCII.GetBytes("Run");
            nwStream.Write(myWriteBuffer, 0, myWriteBuffer.Length);
        }
    }

    public static float[] StringToFloatArray(string sVector)
    {
        float[] result;

        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        string[] sArray = sVector.Split(',');

        // store as a Float array
        if (sVector != null)
        {
            result = new float[]
            {
                float.Parse(sArray[0]),
                float.Parse(sArray[1]),
                float.Parse(sArray[2])
            };
            return result;
        }

        return new float[] { 0, 0, 0 };
    }
}
