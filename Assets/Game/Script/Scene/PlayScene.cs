using UnityEngine;
using System.Collections;

using GameEnum;
using GameSystem;

public class PlayScene : BaseScene {

    private PlayBottomUI _playBottomUI = null;

    public PlayScene()
    {
        this._type = SCENE_TYPE.PLAY;
    }

    private void Awake()
    {
        GameResourceManager.Instance.Load("play", "play_resources");
        GameUIManager.Instance.Load("play", "play_ui");

        GameObject instant = GameResourceManager.Instance.GetSingleUI("play_bottom_ui");
        if (instant != null)
        {
            this._playBottomUI = instant.GetComponent<PlayBottomUI>();
            this._playBottomUI.Initialize(OnLeft, OnRight, OnUp, OnDown);
            this._playBottomUI.SetEnable(true);
        }

        GamePlayManager.Instance.Load();

		GameSoundManager.Instance.PlayBGM ("bgm_01", true);
    }

    private void OnLeft()
    {
        GamePlayManager.Instance.MoveLeft();
    }

    private void OnRight()
    {
        GamePlayManager.Instance.MoveRight();
    }

    private void OnUp()
    {
        GamePlayManager.Instance.Turn();
    }

    private void OnDown()
    {
        GamePlayManager.Instance.MoveDown();
    }

    private void OnDestroy()
    {
        GameUIManager.Instance.RemoveGroup("play");
        GameResourceManager.Instance.RemoveGroup("play");
    }
}
