using UnityEngine;
using System.Collections;

public class MainUI : BaseUI {

	public delegate void StartEvent ();
	public delegate void RankEvent ();
	public delegate void OptionEvent ();

	private StartEvent _startEvent = null;
	private RankEvent _rankEvent = null;
	private OptionEvent _optionEvent = null;
	
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
		GameDebug.Log ("Rank");
	}

	private void OnStart() {
		GameDebug.Log ("Start");
	}

	private void OnOption() {
		GameDebug.Log ("Option");
	}

	public override void SetEnable (bool enable)
	{
		base.SetEnable (enable);
	}
}
