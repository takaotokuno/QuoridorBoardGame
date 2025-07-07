using System;
using System.IO;
using UnityEngine;

public class DebugLogger{
    private static string logFilePath = Path.Combine(Application.persistentDataPath, "game_debug_log.txt");

    public static void Log(string message){
        UnityEngine.Debug.Log(message);
        WriteToFile(message);
    }

    private static void WriteToFile(string message)
    {
        try
        {
            File.AppendAllText(logFilePath, message + Environment.NewLine);
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.Log($"Failed to write log: {ex.Message}");
        }
    }
}
