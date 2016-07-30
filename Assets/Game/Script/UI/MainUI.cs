using UnityEngine;
using System.Collections;

public class MainUI : BaseUI {

	public delegate void StartEvent ();
	public delegate void RankEvent ();
	public delegate void OptionEvent ();

	private StartEvent _startEvent = null;
	private RankEvent _rankEvent = null;
	private OptionEvent _optionEvent = null;

    public void Initialize(StartEvent startEvent, RankEvent rankEvent, OptionEvent optionEvent)
    {
        Initialize();

        this._startEvent = startEvent;
        this._rankEvent = rankEvent;
        this._optionEvent = optionEvent;
    }
	
	public override void Initialize ()
	{
		base.Initialize ();

		SetText ("TitleText", GameStringManager.Instance.GetString ("neon_tetris"));
		SetText ("StartText", GameStringManager.Instance.GetString ("start"));
		SetText ("RankText", GameStringManager.Instance.GetString ("rank"));
		SetText ("OptionText", GameStringManager.Instance.GetString ("option"));

		SetButton ("StartButton", OnStart);
		SetButton ("RankButton", OnRank);
		SetButton ("OptionButton", OnOption);
	}

	private void OnRank() {
        if (this._rankEvent != null)
        {
            this._rankEvent();
        }
	}

	private void OnStart() {
        if (this._startEvent != null)
        {
            this._startEvent();
        }
	}

	private void OnOption() {
        if (this._optionEvent != null)
        {
            this._optionEvent();
        }
	}

	public override void SetEnable (bool enable)
	{
		base.SetEnable (enable);
	}
}
