using UnityEngine;
using System.Collections;

public class BaseObject : MonoBehaviour {

	private bool _enable = true;

	public virtual void Initialize() {
	}

	public virtual void SetEnable(bool enable) {
		this._enable = enable;
	}

	public bool GetEnable() {
		return this._enable;
	}
}
