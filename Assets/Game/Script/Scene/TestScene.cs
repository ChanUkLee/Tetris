using UnityEngine;
using System.Collections;

using GameEnum;

public class TestScene : BaseScene {

	public TestScene() {
		this._type = SCENE_TYPE.TEST;
	}

	private void Awake() {
		GameResourceManager.Instance.Load ("test", "test_resources");
		GameUIManager.Instance.Load ("test", "test_ui");

		GameObject instant = GameResourceManager.Instance.GetSingleUI ("test_ui");
		if (instant != null) {
			TestUI component = instant.GetComponent<TestUI> ();
			if (component != null) {
				component.Initialize ();
			} else {
				GameDebug.Error ("null exception");
			}
		} else {
			GameDebug.Error ("null exception");
		}
	}

	private void OnDestroy() {
		GameUIManager.Instance.RemoveGroup ("test");
		GameResourceManager.Instance.RemoveGroup ("test");
	}
}
