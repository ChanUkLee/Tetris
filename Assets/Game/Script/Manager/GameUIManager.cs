using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class GameUIManager : Singleton<GameUIManager> {

	private class GameUI {
		public string _name;

		public int _sortOrder;
		public string _layer;
	}

	private class GameCanvas {
		public string _group;
		public string _name;

		public bool _alwaysTop;
		public Canvas _canvas;

		public List<GameUI> _uiList;
	}

	private Dictionary<string, GameCanvas> _canvasDictionary = new Dictionary<string, GameCanvas> ();

	private Canvas _blindCanvas = null;

	public delegate void PreLoadCompleteDelegate ();

	#region FOR_PRE_RENDER
	public void CreateBlindCanvas() {
		this._blindCanvas = CreateCanvas ("blind_canvas", "UI", RenderMode.ScreenSpaceOverlay, -1, string.Empty, 100, string.Empty);

		GameObject instant = new GameObject ("blind_panel", typeof(RectTransform));
		instant.transform.SetParent (this._blindCanvas.gameObject.transform);
		instant.layer = LayerMask.NameToLayer ("UI");
		instant.AddComponent <CanvasRenderer> ();
		//CanvasRenderer canvasRenderer = instant.AddComponent <CanvasRenderer> ();

		RectTransform rectTransform = instant.GetComponent<RectTransform> ();
		rectTransform.sizeDelta = new Vector2 (Screen.width, Screen.height);
		rectTransform.position = new Vector3 (Screen.width / 2f, Screen.height / 2f);

		Image image = instant.AddComponent <Image> ();
		image.color = Color.black;

		SetEnableBlind (false);
	}

	public void SetEnableBlind(bool enable) {
		if (this._blindCanvas != null) {
			this._blindCanvas.gameObject.SetActive (enable);
		} else {
			GameDebug.Error ("null exception");
		}
	}

	public void PreLoad(string groupname, PreLoadCompleteDelegate preloadEvent) {
		IEnumerator coroutine = PreLoadStep (groupname, preloadEvent);
		StartCoroutine (coroutine);
	}

	private IEnumerator PreLoadStep(string groupname, PreLoadCompleteDelegate preloadEvent) {
		if (this._canvasDictionary != null) {
			bool active = false;

			GameObject instant = null;
			Dictionary<string, GameCanvas>.Enumerator enumerator = this._canvasDictionary.GetEnumerator ();
			while (enumerator.MoveNext () == true) {
				if (enumerator.Current.Value._group.Equals (groupname) == true) {
					if (enumerator.Current.Value._uiList != null) {
						for (int i = 0; i < enumerator.Current.Value._uiList.Count; i++) {
							instant = GameResourceManager.Instance.GetSingleUI (enumerator.Current.Value._uiList [i]._name);
							if (instant != null) {
								active = instant.activeSelf;
								instant.SetActive (true);

								yield return null;
								instant.SetActive (active);
							}
						}
					}
				}
			}
		}

		if (preloadEvent != null) {
			preloadEvent ();
		}
	}
	#endregion

	public void Release() {
		Dictionary<string, GameCanvas>.Enumerator canvasEnum = this._canvasDictionary.GetEnumerator ();
		while (canvasEnum.MoveNext() == true) {
			if (canvasEnum.Current.Value._canvas != null) {
				if (canvasEnum.Current.Value._canvas.gameObject != null) {
					Destroy (canvasEnum.Current.Value._canvas.gameObject);
				}
			}
		}

		this._canvasDictionary.Clear ();
		this._canvasDictionary = null;
	}

	public void Load(string group, string filename) {
		TextAsset textAsset = Resources.Load ("Xml/" + filename) as TextAsset;
		if (textAsset != null) {
			XmlDocument xmlDocument = new XmlDocument ();
			xmlDocument.LoadXml (textAsset.text);

			try {
				XmlNode root = xmlDocument.DocumentElement;
				if (root != null && root.Name.Equals("group") == true) {
					XmlNode singleNode = root.SelectSingleNode ("canvas_list");
					if (singleNode != null) {
						XmlNodeList nodeList = singleNode.SelectNodes ("canvas");
						if (nodeList != null) {
							XmlNode node = null;
							for (int i = 0; i < nodeList.Count; i++) {
								node = nodeList[i];
								if (node != null) {
									string canvasName = string.Empty;
									string layerName = string.Empty;
									RenderMode renderMode = RenderMode.ScreenSpaceOverlay;
									int sortOrder = 0;

									XmlNode canvasNameNode = node.Attributes.GetNamedItem ("name");
									if (canvasNameNode != null) {
										canvasName = canvasNameNode.Value;
									}

									XmlNode layerNameNode = node.Attributes.GetNamedItem ("layer_name");
									if (layerNameNode != null) {
										layerName = layerNameNode.Value;
									}

									XmlNode renderModeNode = node.Attributes.GetNamedItem ("render_mode");
									if (renderModeNode != null) {
										string mode = renderModeNode.Value;
										if (mode.Equals ("overlay") == true) {
											renderMode = RenderMode.ScreenSpaceOverlay;
										} else if (mode.Equals ("camera") == true) {
											renderMode = RenderMode.ScreenSpaceCamera;
										} else if (mode.Equals ("world") == true) {
											renderMode = RenderMode.WorldSpace;
										} else {
											GameDebug.Error ("canvas render mode " + mode);
										}
									}

									XmlNode sortOrderNode = node.Attributes.GetNamedItem ("sortOrder");
									if (sortOrderNode != null) { 
										sortOrder = System.Convert.ToInt32(sortOrderNode.Value);
									}

									string cameraName = string.Empty;
									int planeDistance = 0;
									string sortLayerName = string.Empty;

									XmlNode cameraNameNode = node.Attributes.GetNamedItem ("camera_name");
									if (cameraNameNode != null) {
										cameraName = cameraNameNode.Value;
									}

									XmlNode planeDistanceNode = node.Attributes.GetNamedItem ("plane_distance");
									if (planeDistanceNode != null) {
										planeDistance = System.Convert.ToInt32(planeDistanceNode.Value);
									}

									XmlNode sortLayerNameNode = node.Attributes.GetNamedItem ("sort_layer_name");
									if (sortLayerNameNode != null) {
										sortLayerName = string.Empty;
									}

									Canvas canvas = CreateCanvas (canvasName, layerName, renderMode, sortOrder, cameraName, planeDistance, sortLayerName);

									GameCanvas gameCanvas = new GameCanvas ();
									gameCanvas._group = group;
									gameCanvas._name = canvasName;
									gameCanvas._canvas = canvas;
									gameCanvas._alwaysTop = false;
									gameCanvas._uiList = new List<GameUI> ();

									XmlNode alwaysTopNode = node.Attributes.GetNamedItem ("always_top");
									if (alwaysTopNode != null) {
										gameCanvas._alwaysTop = System.Convert.ToBoolean (alwaysTopNode.Value);
									}

									if (this._canvasDictionary.ContainsKey(canvasName) == true) {
										GameDebug.Error ("same canvas " + canvasName);
									} else {
										this._canvasDictionary.Add (canvasName, gameCanvas);
									}
								}
							}
						}
					}

					singleNode = root.SelectSingleNode ("ui_list");
					if (singleNode != null) {
						XmlNodeList nodeList = singleNode.SelectNodes ("ui");
						if (nodeList != null) {
							XmlNode node = null;
							for (int i = 0; i < nodeList.Count; i++) {
								node = nodeList[i];
								if (node != null) {
									GameUI gameUI = new GameUI ();
									gameUI._sortOrder = 0;
									gameUI._layer = string.Empty;
									gameUI._name = name;

									XmlNode nameNode = node.Attributes.GetNamedItem ("name");
									if (nameNode != null) {
										gameUI._name = nameNode.Value;
										GameObject instant = GameResourceManager.Instance.GetSingleUI (gameUI._name);

										if (instant != null) {
											XmlNode depthNode = node.Attributes.GetNamedItem ("depth");
											if (depthNode != null) {
												gameUI._sortOrder = System.Convert.ToInt32(depthNode.Value);
											}

											XmlNode layerNameNode = node.Attributes.GetNamedItem ("layer_name");
											if (layerNameNode != null) {
												gameUI._layer = layerNameNode.Value;
											}

											XmlNode canvasNameNode = node.Attributes.GetNamedItem ("canvas");
											if (canvasNameNode != null) {
												GameCanvas canvas = FindCanvas (canvasNameNode.Value);
												if (canvas != null) {
													if (canvas._uiList != null) {
														canvas._uiList.Add (gameUI);

														if (canvas._canvas != null ) {
															instant.transform.SetParent (canvas._canvas.transform, false);
														}
													}
												} else {
													GameDebug.Error ("null exception");
												}
											}
										}
									}
								}
							}
						}
					}
				}

				Sort ();
			} catch (XmlException e) {
				GameDebug.Error (e.ToString());
			}

			System.GC.Collect ();
		}
	}

	private Canvas CreateCanvas(string canvasName, string layerName, RenderMode renderMode, int sortingOrder, string cameraName, int planeDistance, string sortLayerName) {
		GameObject canvasObject = new GameObject (canvasName, typeof(RectTransform));
		canvasObject.transform.SetParent (this.transform);
		canvasObject.layer = LayerMask.NameToLayer (layerName);
		
		Canvas canvas = canvasObject.AddComponent<Canvas> ();
		canvas.renderMode = renderMode;
		canvas.sortingOrder = sortingOrder;
		canvas.planeDistance = planeDistance;

		if (cameraName.Equals(string.Empty) == false) {
			for (int i = 0; i < Camera.allCamerasCount; i++) {
				if (Camera.allCameras[i] != null && Camera.allCameras[i].name.Equals(cameraName) == true) {
					canvas.worldCamera = Camera.allCameras[i];
				}
			}
		}

		if (sortLayerName.Equals(string.Empty) == false) {
			canvas.sortingLayerName = sortLayerName;
		}

		CanvasScaler canvasScaler = canvasObject.AddComponent<CanvasScaler> ();
		canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
		canvasScaler.scaleFactor = 1f;
		canvasScaler.referencePixelsPerUnit = 100f;

		GraphicRaycaster graphicRaycaster = canvasObject.AddComponent<GraphicRaycaster> ();
		graphicRaycaster.ignoreReversedGraphics = true;
		graphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;
		// can not found blocking mask
		
		RectTransform rectTransform = canvasObject.GetComponent<RectTransform> ();
		rectTransform.sizeDelta = new Vector2 (Screen.width, Screen.height);
		rectTransform.position = new Vector3 (Screen.width / 2f, Screen.height / 2f);

		return canvas;
	}

	private GameCanvas FindCanvas(string name) {
		if (this._canvasDictionary != null) {
			if (this._canvasDictionary.ContainsKey (name) == true) {
				return this._canvasDictionary [name];
			}
		}

		return null;
	}

	private void Sort() {
		int index = -1;
		int max = 0;
		List<GameCanvas> canvasList = new List<GameCanvas> ();
		Dictionary<string, GameCanvas>.Enumerator enumerator = this._canvasDictionary.GetEnumerator ();
		while (enumerator.MoveNext () == true) {
			if (index == -1 || (enumerator.Current.Value._canvas != null && index > enumerator.Current.Value._canvas.transform.GetSiblingIndex ())) {
				index = enumerator.Current.Value._canvas.transform.GetSiblingIndex ();
			}

			SortUIList (enumerator.Current.Value);

			canvasList.Add (enumerator.Current.Value);
		}

		for (int i = 0; i < canvasList.Count; i++) {
			if (canvasList [i]._canvas != null && canvasList [i]._alwaysTop == false) {
				canvasList [i]._canvas.transform.SetSiblingIndex (index);
				if (max < canvasList [i]._canvas.sortingOrder) {
					max = canvasList [i]._canvas.sortingOrder;
				}
				index++;
			}
		}

		for (int i = 0; i < canvasList.Count; i++) {
			if (canvasList [i]._canvas != null && canvasList [i]._alwaysTop == true) {
				canvasList [i]._canvas.transform.SetSiblingIndex (index);
				canvasList [i]._canvas.sortingOrder = max + 1;
				index++;
			}
		}

		if (this._blindCanvas != null) {
			this._blindCanvas.sortingOrder = max + 1;
			this._blindCanvas.gameObject.transform.SetSiblingIndex (index);
		}
	}

	private void SortUIList(GameCanvas canvas) {
		if (canvas != null) {
			if (canvas._uiList != null) {
				canvas._uiList.Sort (delegate(GameUI x, GameUI y) {
					if (x != null && y == null) {
						return 1;
					}

					if (x == null && y != null) {
						return -1;
					}

					if (x != null && y != null) {
						return x._sortOrder.CompareTo(y._sortOrder);
					}

					return 0;	
				});

				int index = -1;
				GameObject instant = null;
				for (int i = 0; i < canvas._uiList.Count; i++) {
					if (canvas._uiList [i] != null) {
						instant = GameResourceManager.Instance.GetSingleUI (canvas._uiList [i]._name);
						if (instant != null) {
							if (index > instant.transform.GetSiblingIndex () || index == -1) {
								index = instant.transform.GetSiblingIndex ();
							}
						}
					}
				}

				for (int i = 0; i < canvas._uiList.Count; i++) {
					if (canvas._uiList [i] != null) {
						instant = GameResourceManager.Instance.GetSingleUI (canvas._uiList [i]._name);
						if (instant != null) {
							instant.transform.SetSiblingIndex (index);
							index++;
						}
					}
				}
			}
		}
	}

	public void RemoveGroup(string group) {
		List<string> removeList = new List<string> ();

		if (this._canvasDictionary != null) {
			removeList.Clear ();
			Dictionary<string, GameCanvas>.Enumerator canvasEnum = this._canvasDictionary.GetEnumerator ();
			while (canvasEnum.MoveNext() == true) {
				if (canvasEnum.Current.Value != null) {
					if (canvasEnum.Current.Value._group.Equals(group) == true) {
						if (canvasEnum.Current.Value._uiList != null) {
							for (int i = 0; i < canvasEnum.Current.Value._uiList.Count; i++) {
								GameResourceManager.Instance.RemoveSingleUI (canvasEnum.Current.Value._uiList [i]._name);
							}
						}

						if (canvasEnum.Current.Value._canvas != null) {
							if (canvasEnum.Current.Value._canvas.gameObject != null) {
								Destroy (canvasEnum.Current.Value._canvas.gameObject);
							}
						}

						removeList.Add (canvasEnum.Current.Key);
					}
				}
			}

			for (int i = 0; i <removeList.Count; i++) {
				this._canvasDictionary.Remove (removeList[i]);
			}
		}

		System.GC.Collect ();
	}

	public void RemoveAll() {
		Dictionary<string, GameCanvas>.Enumerator canvasEnum = this._canvasDictionary.GetEnumerator ();
		while (canvasEnum.MoveNext() == true) {
			if (canvasEnum.Current.Value != null) {
				if (canvasEnum.Current.Value._uiList != null) {
					for (int i = 0; i < canvasEnum.Current.Value._uiList.Count; i++) {
						GameResourceManager.Instance.RemoveSingleUI (canvasEnum.Current.Value._uiList [i]._name);
					}
				}

				if (canvasEnum.Current.Value._canvas != null) {
					if (canvasEnum.Current.Value._canvas.gameObject != null) {
						Destroy (canvasEnum.Current.Value._canvas.gameObject);
					}
				}
			}
		}
		this._canvasDictionary.Clear ();

		System.GC.Collect ();
	}
}
