using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

using GameEnum;

public class GameMain : MonoBehaviour {

	private void Awake() {
		DontDestroyOnLoad (this.gameObject);

		InitScreenResolution ();
		InitEventSystem ();

		GameSceneManager.Instance.ChangeScene (SCENE_TYPE.LOAD);
	}

	public void InitScreenResolution() {
#if UNITY_IOS
		if ((UnityEngine.iOS.Device.generation.ToString ()).IndexOf ("iPad") > -1) {
			GameLog.Debug ("iPad no scale screen(has screen resolution bug)");
			return;
		}
#endif

		Resolution[] resolutions = Screen.resolutions;

		if (resolutions.Length > 0) {
			Screen.SetResolution (resolutions [0].width, resolutions [0].height, true);
		} else {
			int width = 640;
			int height = System.Convert.ToInt32((float)width * ((float)Screen.height / (float)Screen.width));

			Screen.SetResolution (width, height, true);
		}
	}

	public void InitEventSystem() {
		this.gameObject.AddComponent<EventSystem> ();
		this.gameObject.AddComponent<StandaloneInputModule> ();
		this.gameObject.AddComponent<TouchInputModule> ();
	}
}
