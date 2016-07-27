using UnityEngine;
using System.Collections;

public class TestUI : BaseUI {

	public override void Initialize () {
		base.Initialize ();

		SetText ("TitleText", GameStringManager.Instance.GetString ("test_title"));
		SetText ("TestText", GameStringManager.Instance.GetString ("test_button"));
		SetButton ("TestButton", Test);
	}

	private void Test() {
		GameDebug.Log ("TEST");
	}
}
