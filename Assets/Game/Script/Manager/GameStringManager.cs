using UnityEngine;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class GameStringManager : Singleton<GameStringManager> {
	private const string PATH = "Xml/String";

	private Dictionary<string, string> _stringDic = new Dictionary<string, string> ();

	public void Remove() {
		this._stringDic.Clear();
	}

	public void Load(string filename) {
		string path = Path.Combine (PATH, filename);

		TextAsset textAsset = Resources.Load<TextAsset> (path) as TextAsset;
		if (textAsset != null) {
			XmlDocument xmlDocument = new XmlDocument ();
			xmlDocument.LoadXml (textAsset.text);

			try {
				XmlNode tableNode = xmlDocument.SelectSingleNode ("string_table");
				XmlNodeList stringNodes = tableNode.ChildNodes;
				XmlNode stringNode = null;
				XmlNode keyAttr = null;
				XmlNode valueAttr = null;

				string key = string.Empty;
				string value = string.Empty;

				for (int i = 0; i < stringNodes.Count; i++) {
					stringNode = stringNodes [i];
					if (stringNode != null && stringNode.Name.Equals ("string") == true) {
						keyAttr = stringNode.Attributes.GetNamedItem ("id");
						if (keyAttr != null) {
							key = keyAttr.Value;

							if (Application.systemLanguage == SystemLanguage.Korean) {
								valueAttr = stringNode.Attributes.GetNamedItem ("kor");
							} else if (Application.systemLanguage == SystemLanguage.ChineseSimplified) {
								valueAttr = stringNode.Attributes.GetNamedItem ("chn_sc");
							} else if (Application.systemLanguage == SystemLanguage.ChineseTraditional) {
								valueAttr = stringNode.Attributes.GetNamedItem ("chn_tw");
							} else if (Application.systemLanguage == SystemLanguage.Japanese) {
								valueAttr = stringNode.Attributes.GetNamedItem ("jpn");
							} else if (Application.systemLanguage == SystemLanguage.Russian) {
								valueAttr = stringNode.Attributes.GetNamedItem ("rus");
							} else {
								valueAttr = stringNode.Attributes.GetNamedItem ("eng");
							}

							if (valueAttr != null) {
								value = valueAttr.Value;

								if (value.Length > 0 && value.Contains ("\\n") == true) {
									value = value.Replace ("\\n", System.Environment.NewLine);
								}

								if (this._stringDic.ContainsKey (key) == false) {
									this._stringDic.Add (key, value);
								}
							}
						}
					}
				}
			} catch (XmlException exception) {
				GameDebug.Error (exception.Message);
			}
		} else {
			StringBuilder sb = new StringBuilder ();
			sb.Append ("not found string at ");
			sb.Append (path);

			GameDebug.Error (sb.ToString ());
		}
	}

	public string GetString(string key) {
		if (this._stringDic.ContainsKey (key) == false) {
			return "null";
		}

		return this._stringDic[key];
	}
}
