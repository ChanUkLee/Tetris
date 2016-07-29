using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

using GameEnum;

public class GameSceneManager : Singleton<GameSceneManager> {

	private BaseScene _scene = null;

	public void ChangeScene(SCENE_TYPE type) {
		IEnumerator coroutine = LoadScene (type);
		StartCoroutine (coroutine);
	}

	private IEnumerator LoadScene(SCENE_TYPE type) {
		if (this._scene != null) {
			if (this._scene._type != type) {
				switch (this._scene._type) {
				case SCENE_TYPE.TEST:
					{
						TestScene component = this._scene as TestScene;
						if (component != null) {
							Destroy (component);
							this._scene = null;
						} else {
							GameDebug.Error ("null exception");
						}
						break;
					}
				case SCENE_TYPE.LOAD:
					{
						LoadScene component = this._scene as LoadScene;
						if (component != null) {
							Destroy (component);
							this._scene = null;
						} else {
							GameDebug.Error ("null exception");
						}
						break;
					}
				case SCENE_TYPE.MAIN:
					{
						MainScene component = this._scene as MainScene;
						if (component != null) {
							Destroy (component);
							this._scene = null;
						} else {
							GameDebug.Error ("null exception");
						}
						break;
					}
				default:
					{
						break;
					}
				}
			}
		}

		if (this._scene != null) {
			GameDebug.Error ("scene component not destoried");
		}

		switch (type) {
		case SCENE_TYPE.TEST:
			{
				yield return SceneManager.LoadSceneAsync ("Test");

				TestScene component = this.gameObject.AddComponent<TestScene> ();
				this._scene = component as BaseScene;
				break;
			}
		case SCENE_TYPE.LOAD:
			{
				yield return SceneManager.LoadSceneAsync ("Load");

				LoadScene component = this.gameObject.AddComponent<LoadScene> ();
				this._scene = component as BaseScene;
				break;
			}
		case SCENE_TYPE.MAIN:
			{
				yield return SceneManager.LoadSceneAsync ("Main");

				MainScene component = this.gameObject.AddComponent<MainScene> ();
				this._scene = component as BaseScene;
				break;
			}
		default:
			{
				break;
			}
		}
	}
}
