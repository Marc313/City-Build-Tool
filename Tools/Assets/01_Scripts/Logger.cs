using UnityEngine;

public static class Logger
{
    public static void Log(string _logText)
    {
        UIManager.Instance.ShowLogText(_logText);
    }
}
