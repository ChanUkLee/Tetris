using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public partial class GameHelper {
	public static bool CheckActive(GameObject instant) {
		if (instant != null) {
			if (instant.activeSelf == true) {
				Transform parent = instant.transform.parent;
				while (parent != null) {
					if (parent.gameObject.activeSelf == false) {
						return false;
					}

					parent = parent.parent;
				}

				return true;
			}
		}

		return false;
	}

	public static void ChangeLayer(Transform parent, int layerMask) {
		parent.gameObject.layer = layerMask;
		foreach (Transform child in parent) {
			ChangeLayer(child, layerMask);
		}
	}

	public static GameObject FindChildObject(GameObject instant, string name) {
		if (instant == null) {
			return null;
		}

		Transform[] transforms = instant.GetComponentsInChildren<Transform> ();
		if (transforms != null) {
			for (int i = 0; i < transforms.Length; i++) {
				if (transforms[i] != null && transforms[i].name.Equals(name) == true) {
					return transforms[i].gameObject;
				}
			}
		}

		GameDebug.Error ("game object not found " + name);
		return null;
	}

	public static Vector2 GetGridLayoutSize(GridLayoutGroup gridLayoutGroup) {
		if (gridLayoutGroup != null) {
			int count = gridLayoutGroup.gameObject.transform.childCount;
			if (count > 0) {
				int column = 1;
				int row = 1;

				Vector2 cellSize = gridLayoutGroup.cellSize;
				Vector2 spacing = gridLayoutGroup.spacing;
				int constraintCount = gridLayoutGroup.constraintCount;
				GridLayoutGroup.Constraint constraint = gridLayoutGroup.constraint;

				switch (constraint) {
				case GridLayoutGroup.Constraint.Flexible:
					{
						constraintCount = 1;
						while (count < constraintCount * constraintCount) {
							constraintCount++;
						}

						column = constraintCount;
						row = count / constraintCount;

						if (count % constraintCount > 0) {
							row++;
						}
						break;
					}
				case GridLayoutGroup.Constraint.FixedColumnCount:
					{
						if (constraintCount > 0) {
							if (count > constraintCount) {
								column = constraintCount;
							} else {
								column = count;
							}

							row = count / constraintCount;

							if (count % constraintCount > 0) {
								row++;
							}
						}
						break;
					}
				case GridLayoutGroup.Constraint.FixedRowCount:
					{
						if (constraintCount > 0) {
							if (count > constraintCount) {
								row = constraintCount;
							} else {
								row = count;
							}

							column = count / constraintCount;

							if (count % constraintCount > 0) {
								column++;
							}
						}
						break;
					}
				default:
					{
						break;
					}
				}

				float width = column * cellSize.x + (column - 1) * spacing.x;
				float height = row * cellSize.y + (row - 1) * spacing.y;

				return new Vector2 (width, height);
			}
		} else {
			GameDebug.Error ("null exception");
		}

		return Vector2.zero;
	}
}