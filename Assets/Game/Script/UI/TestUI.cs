using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestUI : BaseUI {

	private int _count = 10;
	private List<TestItemUI> _itemList = new List<TestItemUI> ();

	public override void Initialize () {
		base.Initialize ();

		SetText ("TitleText", GameStringManager.Instance.GetString ("test_title"));
		SetText ("TestText", GameStringManager.Instance.GetString ("test_button"));
		SetButton ("TestButton", Test);

		GameObject instant = null;
		TestItemUI component = null;
		RectTransform content = GetRectTransform ("Content");
		for (int i = 0; i < this._count; i++) {
			instant = GameResourceManager.Instance.CreateUI ("test_item_ui");
			if (instant != null) {
				instant.transform.SetParent (content);
				component = instant.GetComponent<TestItemUI> ();
				if (component != null) {
					component.Initialize ();
				}
			}
		}
	}

	private void Test() {
		GameDebug.Log ("TEST");
	}
}
