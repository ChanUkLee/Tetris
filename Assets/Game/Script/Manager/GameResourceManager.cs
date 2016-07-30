using UnityEngine;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using GameFile;

public class GameResourceManager : Singleton<GameResourceManager> {

    private const string PATH = "Xml";

	private struct UINode {
		public string _group;
		public string _name;

		public string _filePath;
		public string _fileName;

		public GameObject _prefab;
	}

	private struct EffectNode {
		public string _group;
		public string _name;

		public string _filePath;
		public string _fileName;

		public GameObject _prefab;
	}

	private struct ObjectNode {
		public string _group;
		public string _name;

		public string _filePath;
		public string _fileName;

		public GameObject _prefab;
	}

	private struct SoundNode {
		public string _group;
		public string _name;

		public string _filePath;
		public string _fileName;

		public AudioClip _clip;
	}

	private Dictionary<string, UINode> _uiNodeDictionary = new Dictionary<string, UINode> ();
	private Dictionary<string ,EffectNode> _effectNodeDictionary = new Dictionary<string, EffectNode> ();
	private Dictionary<string, ObjectNode> _objectNodeDictionary = new Dictionary<string, ObjectNode> ();
	private Dictionary<string, SoundNode> _soundNodeDictionary = new Dictionary<string, SoundNode> ();

	private Dictionary<string, GameObject> _singleUIDictionary = new Dictionary<string, GameObject> ();

	public void Load(string group, string filename) {
        string path = Path.Combine(PATH, filename);

        string output = string.Empty;
        if (Resource.Read(path, out output) == true)
        {
            StringBuilder sb = new StringBuilder();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(output);

            try
            {
                XmlNode root = xmlDocument.DocumentElement;
                if (root != null && root.Name.Equals("resource_list") == true)
                {
                    XmlNode singleNode = root.SelectSingleNode("ui_list");
                    if (singleNode != null)
                    {
                        XmlNodeList nodeList = singleNode.SelectNodes("ui");
                        if (nodeList != null)
                        {
                            XmlNode node = null;
                            for (int i = 0; i < nodeList.Count; i++)
                            {
                                node = nodeList[i];
                                if (node != null)
                                {
                                    UINode uiNode = new UINode();
                                    uiNode._group = group;
                                    uiNode._name = string.Empty;
                                    uiNode._filePath = string.Empty;
                                    uiNode._fileName = string.Empty;
                                    uiNode._prefab = null;

                                    XmlNode nameNode = node.Attributes.GetNamedItem("name");
                                    if (nameNode != null)
                                    {
                                        uiNode._name = nameNode.Value;
                                    }

                                    XmlNode pathNode = node.Attributes.GetNamedItem("path");
                                    if (pathNode != null)
                                    {
                                        uiNode._filePath = pathNode.Value;
                                    }

                                    XmlNode srcNode = node.Attributes.GetNamedItem("src");
                                    if (srcNode != null)
                                    {
                                        uiNode._fileName = srcNode.Value;
                                    }

                                    string fullPath = Path.Combine(uiNode._filePath, uiNode._fileName);
                                    uiNode._prefab = Resources.Load(fullPath, typeof(GameObject)) as GameObject;

                                    if (uiNode._prefab != null)
                                    {
                                        if (this._uiNodeDictionary.ContainsKey(uiNode._name) == true)
                                        {
                                            sb.Remove(0, sb.Length);
                                            sb.Append("key error (");
                                            sb.Append(uiNode._name);
                                            sb.Append(")");

                                            GameDebug.Error(sb.ToString());
                                        }
                                        else {
                                            this._uiNodeDictionary.Add(uiNode._name, uiNode);
                                        }
                                    }
                                    else {
                                        sb.Remove(0, sb.Length);
                                        sb.Append("null error (");
                                        sb.Append(fullPath);
                                        sb.Append(")");

                                        GameDebug.Log(sb.ToString());
                                    }
                                }
                            }
                        }
                    }

                    singleNode = root.SelectSingleNode("effect_list");
                    if (singleNode != null)
                    {
                        XmlNodeList nodeList = singleNode.SelectNodes("effect");
                        if (nodeList != null)
                        {
                            XmlNode node = null;
                            for (int i = 0; i < nodeList.Count; i++)
                            {
                                node = nodeList[i];
                                if (node != null)
                                {
                                    EffectNode effectNode = new EffectNode();
                                    effectNode._group = group;
                                    effectNode._name = string.Empty;
                                    effectNode._filePath = string.Empty;
                                    effectNode._fileName = string.Empty;
                                    effectNode._prefab = null;

                                    XmlNode nameNode = node.Attributes.GetNamedItem("name");
                                    if (nameNode != null)
                                    {
                                        effectNode._name = nameNode.Value;
                                    }

                                    XmlNode pathNode = node.Attributes.GetNamedItem("path");
                                    if (pathNode != null)
                                    {
                                        effectNode._filePath = pathNode.Value;
                                    }

                                    XmlNode srcNode = node.Attributes.GetNamedItem("src");
                                    if (srcNode != null)
                                    {
                                        effectNode._fileName = srcNode.Value;
                                    }

                                    string fullPath = Path.Combine(effectNode._filePath, effectNode._fileName);
                                    effectNode._prefab = Resources.Load(fullPath, typeof(GameObject)) as GameObject;
                                    if (effectNode._prefab != null)
                                    {
                                        if (this._effectNodeDictionary.ContainsKey(effectNode._name) == true)
                                        {
                                            sb.Remove(0, sb.Length);
                                            sb.Append("key error (");
                                            sb.Append(effectNode._name);
                                            sb.Append(")");

                                            GameDebug.Error(sb.ToString());
                                        }
                                        else {
                                            this._effectNodeDictionary.Add(effectNode._name, effectNode);
                                        }
                                    }
                                    else {
                                        sb.Remove(0, sb.Length);
                                        sb.Append("null error (");
                                        sb.Append(fullPath);
                                        sb.Append(")");

                                        GameDebug.Error(sb.ToString());
                                    }
                                }
                            }
                        }
                    }

                    singleNode = root.SelectSingleNode("object_list");
                    if (singleNode != null)
                    {
                        XmlNodeList nodeList = singleNode.SelectNodes("object");
                        if (nodeList != null)
                        {
                            XmlNode node = null;
                            for (int i = 0; i < nodeList.Count; i++)
                            {
                                node = nodeList[i];
                                if (node != null)
                                {
                                    ObjectNode objectNode = new ObjectNode();
                                    objectNode._group = group;
                                    objectNode._name = string.Empty;
                                    objectNode._filePath = string.Empty;
                                    objectNode._fileName = string.Empty;
                                    objectNode._prefab = null;

                                    XmlNode nameNode = node.Attributes.GetNamedItem("name");
                                    if (nameNode != null)
                                    {
                                        objectNode._name = nameNode.Value;
                                    }

                                    XmlNode pathNode = node.Attributes.GetNamedItem("path");
                                    if (pathNode != null)
                                    {
                                        objectNode._filePath = pathNode.Value;
                                    }

                                    XmlNode srcNode = node.Attributes.GetNamedItem("src");
                                    if (srcNode != null)
                                    {
                                        objectNode._fileName = srcNode.Value;
                                    }

                                    string fullPath = Path.Combine(objectNode._filePath, objectNode._fileName);
                                    objectNode._prefab = Resources.Load(fullPath, typeof(GameObject)) as GameObject;
                                    if (objectNode._prefab != null)
                                    {
                                        if (this._objectNodeDictionary.ContainsKey(objectNode._name) == true)
                                        {
                                            sb.Remove(0, sb.Length);
                                            sb.Append("key error (");
                                            sb.Append(objectNode._name);
                                            sb.Append(")");

                                            GameDebug.Error(sb.ToString());
                                        }
                                        else {
                                            this._objectNodeDictionary.Add(objectNode._name, objectNode);
                                        }
                                    }
                                    else {
                                        sb.Remove(0, sb.Length);
                                        sb.Append("null error (");
                                        sb.Append(fullPath);
                                        sb.Append(")");

                                        GameDebug.Error(sb.ToString());
                                    }
                                }
                            }
                        }
                    }

                    singleNode = root.SelectSingleNode("sound_list");
                    if (singleNode != null)
                    {
                        XmlNodeList nodeList = singleNode.SelectNodes("sound");
                        if (nodeList != null)
                        {
                            XmlNode node = null;
                            for (int i = 0; i < nodeList.Count; i++)
                            {
                                node = nodeList[i];
                                if (node != null)
                                {
                                    SoundNode soundNode = new SoundNode();
                                    soundNode._group = group;
                                    soundNode._name = string.Empty;
                                    soundNode._filePath = string.Empty;
                                    soundNode._fileName = string.Empty;
                                    soundNode._clip = null;

                                    XmlNode nameNode = node.Attributes.GetNamedItem("name");
                                    if (nameNode != null)
                                    {
                                        soundNode._name = nameNode.Value;
                                    }

                                    XmlNode pathNode = node.Attributes.GetNamedItem("path");
                                    if (pathNode != null)
                                    {
                                        soundNode._filePath = pathNode.Value;
                                    }

                                    XmlNode srcNode = node.Attributes.GetNamedItem("src");
                                    if (srcNode != null)
                                    {
                                        soundNode._fileName = srcNode.Value;
                                    }

                                    string fullPath = Path.Combine(soundNode._filePath, soundNode._fileName);
                                    soundNode._clip = Resources.Load(fullPath, typeof(AudioClip)) as AudioClip;
                                    if (soundNode._clip != null)
                                    {
                                        if (this._soundNodeDictionary.ContainsKey(soundNode._name) == true)
                                        {
                                            sb.Remove(0, sb.Length);
                                            sb.Append("key error (");
                                            sb.Append(soundNode._name);
                                            sb.Append(")");

                                            GameDebug.Error(sb.ToString());
                                        }
                                        else {
                                            this._soundNodeDictionary.Add(soundNode._name, soundNode);
                                        }
                                    }
                                    else {
                                        sb.Remove(0, sb.Length);
                                        sb.Append("null error (");
                                        sb.Append(fullPath);
                                        sb.Append(")");

                                        GameDebug.Error(sb.ToString());
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (XmlException e)
            {
                GameDebug.Error(e.ToString());
            }

            System.GC.Collect();
        }
	}

	public GameObject CreateUI(string name) {
		if (this._uiNodeDictionary != null) {
			if (this._uiNodeDictionary.ContainsKey (name) == true) {
				if (this._uiNodeDictionary [name]._prefab != null) {
					return Instantiate (this._uiNodeDictionary [name]._prefab);
				}
			}
		}

		return null;
	}

	public GameObject GetSingleUI(string name) {
		if (this._singleUIDictionary != null) {
			if (this._singleUIDictionary.ContainsKey (name) == false) {
				GameObject instant = CreateUI (name);
				if (instant != null) {
					this._singleUIDictionary.Add (name, instant);
				}
			}

			if (this._singleUIDictionary.ContainsKey (name) == true) {
				return this._singleUIDictionary [name];
			}
		}

		return null;
	}

    public GameObject GetPrefab(string name)
    {
        if (this._objectNodeDictionary != null)
        {
            if (this._objectNodeDictionary.ContainsKey(name) == true)
            {
                if (this._objectNodeDictionary[name]._prefab != null)
                {
                    return this._objectNodeDictionary[name]._prefab;
                }
            }
        }

        return null;
    }

	public GameObject CreateObject(string name) {
		if (this._objectNodeDictionary != null) {
			if (this._objectNodeDictionary.ContainsKey (name) == true) {
				if (this._objectNodeDictionary [name]._prefab != null) {
					return Instantiate (this._objectNodeDictionary [name]._prefab);
				}
			}
		}

		return null;
	}

	public GameObject CreateEffect(string name) {
		if (this._effectNodeDictionary != null) {
			if (this._effectNodeDictionary.ContainsKey (name) == true) {
				if (this._effectNodeDictionary [name]._prefab != null) {
					return Instantiate (this._effectNodeDictionary [name]._prefab);
				}
			}
		}

		return null;
	}

	public AudioClip GetAudioClip(string name) {
		if (this._soundNodeDictionary != null) {
			if (this._soundNodeDictionary.ContainsKey (name) == true) {
				if (this._soundNodeDictionary [name]._clip != null) {
					return this._soundNodeDictionary [name]._clip;
				}
			}
		}

		return null;
	}

	public void RemoveSingleUI(string name) {
		if (this._singleUIDictionary != null) {
			if (this._singleUIDictionary.ContainsKey (name) == true) {
				if (this._singleUIDictionary [name] != null) {
					Destroy (this._singleUIDictionary [name]);
				}
			}

			this._singleUIDictionary.Remove (name);
		}
	}

	public void RemoveGroup(string group) {
		List<string> removeList = new List<string> ();
		if (this._uiNodeDictionary != null) {
			removeList.Clear ();
			Dictionary<string, UINode>.Enumerator uiEnum = this._uiNodeDictionary.GetEnumerator ();
			while (uiEnum.MoveNext() == true) {
				if (uiEnum.Current.Value._group.Equals(group) == true) {
					RemoveSingleUI (uiEnum.Current.Value._name);
					removeList.Add (uiEnum.Current.Key);
				}
			}

			for (int i = 0; i <removeList.Count; i++) {
				this._uiNodeDictionary.Remove (removeList[i]);
			}
		}

		if (this._effectNodeDictionary != null) {
			removeList.Clear ();
			Dictionary<string, EffectNode>.Enumerator effectEnum = this._effectNodeDictionary.GetEnumerator ();
			while (effectEnum.MoveNext() == true) {
				if (effectEnum.Current.Value._group.Equals(group) == true) {
					removeList.Add (effectEnum.Current.Key);
				}
			}

			for (int i = 0; i <removeList.Count; i++) {
				this._effectNodeDictionary.Remove (removeList[i]);
			}
		}

		if (this._objectNodeDictionary != null) {
			removeList.Clear ();
			Dictionary<string, ObjectNode>.Enumerator objectEnum = this._objectNodeDictionary.GetEnumerator ();
			while (objectEnum.MoveNext() == true) {
				if (objectEnum.Current.Value._group.Equals(group) == true) {
					removeList.Add (objectEnum.Current.Key);
				}
			}

			for (int i = 0; i <removeList.Count; i++) {
				this._objectNodeDictionary.Remove (removeList[i]);
			}
		}

		Resources.UnloadUnusedAssets ();
		System.GC.Collect ();
	}
}