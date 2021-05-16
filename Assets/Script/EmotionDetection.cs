
using System.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
public class LoadPython
{
    public static string ExecuteProcessTerminal(string argument)
    {
        try
        {
            // argument = "python pythonScript/api_emotion_compute.py";
            // argument = "ls";
            UnityEngine.Debug.Log("============== Start Executing [" + argument + "] ===============");
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "/bin/bash",
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                Arguments = " -c \"" + argument + " \""
            };
            Process myProcess = new Process
            {
                StartInfo = startInfo
            };
            myProcess.Start();
            string output = myProcess.StandardOutput.ReadToEnd();
            UnityEngine.Debug.Log(output);
            // UnityEngine.Debug.Log(GetStringResult(output));
            myProcess.WaitForExit();
            UnityEngine.Debug.Log("============== End ===============");

            return output;
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log(e);
            return null;
        }
    }
}