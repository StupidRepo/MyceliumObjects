using UnityEngine;

namespace MyceliumObjects;

public static class Logger
{
	private const string Prefix = "[MyceliumObjects] ";
	public static void Log(string message)
	{
		Debug.Log($"{Prefix}{message}");
	}
	
	public static void LogWarning(string message)
	{
		Debug.LogWarning($"{Prefix}{message}");
	}
	
	public static void LogError(string message)
	{
		Debug.LogError($"{Prefix}{message}");
	}
}