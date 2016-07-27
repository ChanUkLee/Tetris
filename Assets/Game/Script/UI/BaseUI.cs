using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class BaseUI : MonoBehaviour {

	private bool _enable = true;

	private Dictionary<string, Image> _imageDictionary = null;
	private Dictionary<string, Text> _textDictionary = null;
	private Dictionary<string, Button> _buttonDictionary = null;
	private Dictionary<string, UnityAction> _buttonActionDictionary = null;
	private Dictionary<string, InputField> _inputFieldDictionary = null;
	private Dictionary<string, RectTransform> _rectTransformDictionary = null;
	private Dictionary<string, ScrollRect> _scrollRectDictionary = null;

	public virtual void Awake() {
		StringBuilder sb = new StringBuilder ();

		this._imageDictionary = new Dictionary<string, Image> ();
		Image[] imageArray = this.gameObject.GetComponentsInChildren<Image> ();
		if (imageArray != null) {
			for (int i = 0; i < imageArray.Length; i++) {
				if (imageArray [i] != null) {
					if (this._imageDictionary.ContainsKey (imageArray [i].name) == false) {
						this._imageDictionary.Add (imageArray [i].name, imageArray [i]);
					}
				}
			}
		}

		this._inputFieldDictionary = new Dictionary<string, InputField> ();
		InputField[] inputFieldArray = this.gameObject.GetComponentsInChildren<InputField> ();
		if (inputFieldArray != null) {
			for (int i = 0; i < inputFieldArray.Length; i++) {
				if (inputFieldArray [i] != null) {
					if (this._inputFieldDictionary.ContainsKey (inputFieldArray [i].name) == true) {
						sb.Remove (0, sb.Length);
						sb.Append ("same key exception(");
						sb.Append (inputFieldArray [i].name);
						sb.Append (")");

						GameDebug.Error (sb.ToString ());
					} else {
						this._inputFieldDictionary.Add (inputFieldArray [i].name, inputFieldArray [i]);
					}
				}
			}
		}

		this._textDictionary = new Dictionary<string, Text> ();
		Text[] textArray = this.gameObject.GetComponentsInChildren<Text> ();
		if (textArray != null) {
			for (int i = 0; i < textArray.Length; i++) {
				if (textArray [i] != null) {
					if (this._textDictionary.ContainsKey (textArray [i].name) == false) {
						this._textDictionary.Add (textArray [i].name, textArray [i]);
					}
				}
			}
		}

		this._buttonActionDictionary = new Dictionary<string, UnityAction> ();
		this._buttonDictionary = new Dictionary<string, Button> ();
		Button[] buttonArray = this.gameObject.GetComponentsInChildren<Button> ();
		if (buttonArray != null) {
			for (int i = 0; i < buttonArray.Length; i++) {
				if (buttonArray [i] != null) {
					if (this._buttonDictionary.ContainsKey (buttonArray [i].name) == true) {
						sb.Remove (0, sb.Length);
						sb.Append ("same key exception(");
						sb.Append (buttonArray [i].name);
						sb.Append (")");

						GameDebug.Error (sb.ToString ());
					} else {
						this._buttonDictionary.Add (buttonArray [i].name, buttonArray [i]);

						string name = buttonArray [i].name;
						buttonArray [i].onClick.RemoveAllListeners ();
						buttonArray [i].onClick.AddListener (() => ButtonListener (name));
					}
				}
			}
		}

		this._rectTransformDictionary = new Dictionary<string, RectTransform> ();
		RectTransform[] rectTransformArray = this.gameObject.GetComponentsInChildren<RectTransform> ();
		if (rectTransformArray != null) {
			for (int i = 0; i < rectTransformArray.Length; i++) {
				if (rectTransformArray [i] != null) {
					if (this._rectTransformDictionary.ContainsKey (rectTransformArray [i].name) == false) {
						this._rectTransformDictionary.Add (rectTransformArray [i].name, rectTransformArray [i]);
					}
				}
			}
		}

		this._scrollRectDictionary = new Dictionary<string, ScrollRect> ();
		ScrollRect[] scrollRectArray = this.gameObject.GetComponentsInChildren<ScrollRect> ();
		if (scrollRectArray != null) {
			for (int i = 0; i < scrollRectArray.Length; i++) {
				if (scrollRectArray [i] != null) {
					if (this._scrollRectDictionary.ContainsKey (scrollRectArray [i].name) == false) {
						this._scrollRectDictionary.Add (scrollRectArray [i].name, scrollRectArray [i]);
					}
				}
			}
		}
	}

	public virtual void Initialize() {
		
	}

	public virtual void SetEnable (bool enable) {
		this._enable = enable;
	}

	public void SetActive(string name, bool active) {
		RectTransform transform = GetRectTransform (name);
		if (transform != null) {
			transform.gameObject.SetActive (active);
		}
	}

	public RectTransform GetRectTransform(string name) {
		if (this._rectTransformDictionary != null && this._rectTransformDictionary.ContainsKey (name) == true) {
			return this._rectTransformDictionary [name];
		}

		return null;
	}

	#region ScrollFunc
	public ScrollRect GetScrollRect(string name) {
		if (this._scrollRectDictionary != null && this._scrollRectDictionary.ContainsKey (name) == true) {
			return this._scrollRectDictionary [name];
		}

		GameDebug.Error ("can not found rect transform " + name);
		return null;
	}
	#endregion

	#region InputFieldFunc
	public InputField GetInputField(string name) {
		if (this._inputFieldDictionary != null && this._inputFieldDictionary.ContainsKey (name) == true) {
			return this._inputFieldDictionary [name];
		}

		GameDebug.Error ("can not found input field " + name);
		return null;
	}

	public string GetInputFieldText(string name) {
		if (this._inputFieldDictionary != null && this._inputFieldDictionary.ContainsKey (name) == true) {
			return this._inputFieldDictionary [name].text;
		}

		GameDebug.Error ("can not found input field " + name);
		return string.Empty;
	}
	#endregion

	#region ButtonFunc
	private void ButtonListener(string name) {
		if (this._buttonActionDictionary != null) {
			if (this._buttonActionDictionary.ContainsKey (name) == true) {
				if (this._buttonActionDictionary [name] != null) {
					//if (GameScriptManager.Instance.CheckButtonEvent (this._name, name) == true) {
					this._buttonActionDictionary [name] ();
					//}
				}
			}
		}
	}

	public Button GetButton(string name) {
		if (this._buttonDictionary != null && this._buttonDictionary.ContainsKey (name) == true) {
			return this._buttonDictionary [name];
		}

		GameDebug.Error ("can not found button " + name);
		return null;
	}

	public void SetButton(string name, bool interactable) {
		Button button = GetButton (name);
		if (button != null) {
			button.interactable = interactable;
		}
	}

	public void SetButton(string name, UnityAction action) {
		Button button = GetButton (name);
		if (button != null) {
			if (this._buttonActionDictionary != null) {
				if (this._buttonActionDictionary.ContainsKey (name) == false) {
					this._buttonActionDictionary.Add (name, null);
				}

				this._buttonActionDictionary [name] = action;
			}
		} else {
			GameDebug.Error ("can not found button " + name);
		}
	}

	public void SetEnableButton(string name, bool enable) {
		if (this._buttonDictionary != null) {
			if (this._buttonDictionary.ContainsKey (name) == true) {
				this._buttonDictionary [name].enabled = enable;
			}
		}
	}

	public void SetEnableAllButton(bool enable) {
		Dictionary<string, Button>.Enumerator enumerator = this._buttonDictionary.GetEnumerator ();
		while (enumerator.MoveNext () == true) {
			SetEnableButton (enumerator.Current.Key, enable);
		}
	}
	#endregion

	#region ImageFunc
	public Image GetImage(string name) {
		if (this._imageDictionary != null && this._imageDictionary.ContainsKey (name) == true) {
			return this._imageDictionary [name];
		}

		GameDebug.Error ("can not found image " + name);
		return null;
	}

	public void SetImage(string name, Sprite sprite) {
		Image image = GetImage (name);
		if (image != null) {
			if (sprite == null) {
				image.gameObject.SetActive (false);
			} else {
				image.gameObject.SetActive (true);
				image.overrideSprite = sprite;
			}
		}
	}

	public void SetImage(string name, float fillAmount) {
		Image image = GetImage (name);
		if (image != null) {
			image.fillAmount = fillAmount;
		}
	}

	public void SetImage(string name, Color color) {
		Image image = GetImage (name);
		if (image != null) {
			image.color = color;
		}
	}
	#endregion

	#region TextFunc
	public Text GetText(string name) {
		if (this._textDictionary != null && this._textDictionary.ContainsKey (name) == true) {
			return this._textDictionary [name];
		}

		GameDebug.Error ("can not found text " + name);
		return null;
	}

	public void SetText(string name, string value) {
		Text text = GetText (name);
		if (text != null) {
			text.text = value;
		}
	}

	public void SetText(string name, Color color) {
		Text text = GetText (name);
		if (text != null) {
			text.color = color;
		}
	}
	#endregion
}
