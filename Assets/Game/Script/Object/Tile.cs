using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Collections;

public class Tile : BaseObject {

	private Vector3 _pos = Vector3.zero;

	private Text _text = null;
	private SpriteRenderer _renderer = null;

	public override void Initialize ()
	{
		base.Initialize ();

		this._text = this.GetComponentInChildren<Text> ();
		this._renderer = this.GetComponent<SpriteRenderer>();
	}

	public void SetColor(Color color) {
		if (this._renderer != null) {
			this._renderer.color = color;

			if (color.a > 0.5f) {
				this._text.color = Color.black;
			} else {
				this._text.color = Color.white;
			}
		}
	}

	public Vector3 GetPosition() {
		return this._pos;
	}

	public void SetPosition(Vector3 pos) {
		this._pos = pos;
		this.transform.position = GameObjectManager.Instance.GetWorldPosition(this._pos);

		if (GameObjectManager.Instance.CheckInside(pos) == true)
		{
			SetEnable (true);
		}
		else
		{
			SetEnable (false);
		}

		if (this._text != null) {
			StringBuilder sb = new StringBuilder ();
			sb.Append ("( ");
			sb.Append (string.Format ("{0:F0}", pos.x));
			sb.Append (", ");
			sb.Append (string.Format ("{0:F0}", pos.y));
			sb.Append (")");

			this._text.text = sb.ToString ();
		}
	}

	public override void SetEnable (bool enable)
	{
		base.SetEnable (enable);
	}
}
