using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GameSystem;

public class Map : BaseObject {

	private List<Tile> _tileList = new List<Tile> (); 
	private Tile[,] _tileMap = new Tile[MAX.MAP_WIDTH, MAX.MAP_HEIGHT];
    private Vector3 _offset = Vector3.zero;
	private Vector3 _worldPositionOffset = Vector3.zero;

    private Vector3 _size = Vector3.zero;
    public Vector3 Size
    {
        get
        {
            return this._size;
        }
    }

	public override void Initialize()
	{
		base.Initialize();

		CreateTileList ();
	}

	private void CreateTileList() {
		RemoveTileList ();

		GameObject prefab = GameResourceManager.Instance.GetPrefab("tile_bg");
		if (prefab != null) {
			GameObject instant = null;
			Tile component = null;

			for (int i = 0; i < MAX.MAP_WIDTH; i++) {
				for (int j = 0; j < MAX.MAP_HEIGHT; j++) {
					instant = Instantiate (prefab);
					instant.transform.SetParent (this.transform);

					component = instant.GetComponent<Tile> ();
					if (component != null) {
						component.Initialize ();

						this._tileList.Add (component);
					} else {
						Destroy (instant);
					}
				}
			}
		}
	}

	private void RemoveTileList() {
		for (int i = 0; i < this._tileList.Count; i++) {
			if (this._tileList [i] != null) {
				Destroy (this._tileList [i]);
			}
		}
		this._tileList.Clear ();
	}

	public void ClearLine() {
		for (int j = MAX.MAP_HEIGHT - 1; j >= 0; j--)
		{
			if (CheckLineFill(j) == true)
			{
				ClearLine(j);
			}
		}
	}

    private void ClearLine(int index)
    {
		bool moveLine = index - 1 >= 0;
		Vector3 position = Vector3.zero;

		for (int j = index; j > 0; j--) {
			for (int i = 0; i < MAX.MAP_WIDTH; i++)
			{
				if (this._tileMap [i, j] != null) {
					position = this._tileMap [i, j].GetPosition ();
				}

				GameObjectManager.Instance.ReleaseTile (this._tileMap[i, j]);

				if (moveLine == true) {
					this._tileMap [i, j] = this._tileMap [i, j - 1];
					this._tileMap [i, j - 1] = null;

					if (this._tileMap [i, j] != null) {
						this._tileMap [i, j].SetPosition (position);
					}
				} else {
					this._tileMap [i, j] = null;
				}
			}
		}
    }

    private bool CheckLineFill(int index)
    {
        for (int i = 0; i < MAX.MAP_WIDTH; i++)
        {
			if (this._tileMap[i, index] == null)
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckLineEmpty(int index)
    {
        for (int i = 0; i < MAX.MAP_WIDTH; i++)
        {
			if (this._tileMap[i, index] != null)
            {
                return false;
            }
        }

        return true;
    }

    public void SetTile(Tile tile)
    {
		if (CheckInside(tile.GetPosition ()) == true)
        {
			this._tileMap[(int)tile.GetPosition ().x, (int)tile.GetPosition ().y] = tile;
        }
    }

	public void SetOffset(Vector3 offset) {
		this._worldPositionOffset = offset;
	}

    public bool CheckInside(Vector3 position)
    {
        if (0f <= position.x && position.x < (float)MAX.MAP_WIDTH)
        {
			if (position.y < (float)MAX.MAP_HEIGHT)
			{
				return true;
			}
			/*
            if (0f <= position.y && position.y < (float)MAX.MAP_HEIGHT)
            {
                return true;
            }
            */
        }

        return false;
    }

	public Tile GetTile(Vector3 position) {
		if (CheckInside (position) == true) {
			return this._tileMap [(int)position.x, (int)position.y];
		}

		return null;
	}

    public Vector3 GetStartPosition()
    {
		return new Vector3((float)(MAX.MAP_WIDTH / 2), 0f, 0f);
    }

    public void SetSize(Vector3 size)
    {
        this._size = size;
        this._offset = new Vector3((float)MAX.MAP_WIDTH / 2f * this._size.x - this._size.x / 2f, (float)MAX.MAP_HEIGHT / 2f * this._size.y - this._size.y / 2f, 0f);

		float x = 0f;
		float y = 0f;
		bool turn = false;
		Color white = new Color (1f, 1f, 1f, 0.25f);
		Color black = new Color (0.3f, 0.3f, 0.3f, 0.25f);

		Tile tile = null;

		for (int i = 0; i < this._tileList.Count; i++) {
			tile = this._tileList [i];
			if (tile != null) {
				tile.SetPosition (new Vector3 (x, y, 0f));

				if (turn == false) {
					tile.SetColor (white);
					turn = true;
				} else {
					tile.SetColor (black);
					turn = false;
				}

				x++;
				if (x >= MAX.MAP_WIDTH) {
					x = 0f;
					y++;

					turn = !turn;
				}
			}
		}
    }

	private void SetColor (GameObject instant, Color color) {
		if (instant != null)
		{
			SpriteRenderer renderer = instant.GetComponent<SpriteRenderer>();
			if (renderer != null)
			{
				renderer.color = color;
			}
		}
	}

    public Vector3 GetWorldPosition(Vector3 position)
    {
		Vector3 pos = Vector3.zero;
		pos.x = position.x * this._size.x - this._offset.x;
		pos.y = this._offset.y - position.y * this._size.y;

		pos += this._worldPositionOffset;

        return pos;
    }

    public override void SetEnable(bool enable)
    {
        base.SetEnable(enable);
    }
}
