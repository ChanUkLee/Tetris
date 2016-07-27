using UnityEngine;
using System.Collections;

public class GameDebug {
	public static void Log(string text) {
		UnityEngine.Debug.Log (text);
	}

	public static void Error(string text) {
		UnityEngine.Debug.LogError (text);
	}
}
