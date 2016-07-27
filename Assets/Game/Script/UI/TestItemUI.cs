using UnityEngine;
using System.Text;
using System.Collections;

public class TestItemUI : BaseUI {

	public override void Initialize () {
		base.Initialize ();

		SetText ("TestText", GameStringManager.Instance.GetString("test_item"));
	}
}
