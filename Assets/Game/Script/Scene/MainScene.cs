using UnityEngine;
using System.Collections;

using GameEnum;

public class MainScene : BaseScene {

	public MainScene()
	{
		this._type = SCENE_TYPE.MAIN;
	}

	private MainUI _mainUI = null;

	private void Awake()
	{
		GameResourceManager.Instance.Load ("main", "main_resources");
		GameUIManager.Instance.Load ("main", "main_ui");

		GameObject instant = GameResourceManager.Instance.GetSingleUI ("main_ui");
		if (instant != null) {
			this._mainUI = instant.GetComponent<MainUI> ();
			if (this._mainUI != null) {
				this._mainUI.Initialize (OnStart, OnRank, OnOption);
				this._mainUI.SetEnable (true);
			}
		}
	}

    private void OnStart()
    {
        GameSceneManager.Instance.ChangeScene(SCENE_TYPE.PLAY);
    }

    private void OnRank()
    {
        GameDebug.Log("RANK");
    }

    private void OnOption()
    {
        GameDebug.Log("OPTION");
    }

	private void OnDestroy()
	{
		GameUIManager.Instance.RemoveGroup ("main");
		GameResourceManager.Instance.RemoveGroup ("main");
	}
}
