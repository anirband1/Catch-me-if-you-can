using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    Thread mThread;
    public string connectionIP; // This needs to be 127.0.0.1 IN THE INSPECTOR
    int connectionPort = 9999;
    IPAddress localAdd;
    TcpListener listener;
    TcpClient client;

    [HideInInspector]
    public Vector2 receivedPos;
    float[] fArray;
    bool running, quitApp = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        receivedPos = Vector2.zero;
        running = true;

        ThreadStart ts = new ThreadStart(GetInfo);
        mThread = new Thread(ts);
        mThread.Start();
    }

    void Update()
    {

    }

    // Might cause issues for multiple players
    void OnDestroy()
    {
        running = false;
    }

    void OnApplicationQuit()
    {
        quitApp = true;
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

            // Mapping received coords to screen
            receivedPos = new Vector2(fArray[0], -fArray[1]); // Default values are inverted for y axis. THIS IS SCALE FACTOR, MULTIPLY BY MAXX AND MAXY


            //---Sending Data to Host----
            if (quitApp)
            {
                myWriteBuffer = Encoding.ASCII.GetBytes("Stop");
                nwStream.Write(myWriteBuffer, 0, myWriteBuffer.Length);
            }

            // Anything except 'Stop' and python keeps running
            myWriteBuffer = Encoding.ASCII.GetBytes("Run");
            nwStream.Write(myWriteBuffer, 0, myWriteBuffer.Length);
        }
        else
        {
            // show loading scene
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

// nwStream.close();
// client.close();
