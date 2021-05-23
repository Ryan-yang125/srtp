using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;

public class SocketServer : MonoBehaviour
{
    public static string data;

    System.Threading.Thread SocketThread;
    volatile bool keepReading = false;

    // Use this for initialization
    void Start()
    {
        Application.runInBackground = true;
        // StartServer();
    }

    void Update()
    {
        if (File.Exists(Application.dataPath + "/test2.json"))
        {

            string jsonStr = File.ReadAllText(Application.dataPath + "/test2.json");
            data = JsonUtility.FromJson<Visual>(jsonStr).datas;
            Debug.Log(data);
        }
    }

}

