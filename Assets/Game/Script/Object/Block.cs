using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GameData;
using GameEnum;

public class Block : BaseObject {

    public override void Initialize()
    {
        base.Initialize();
    }

    private Vector3 _position = Vector3.zero;
    public Vector3 Position
    {
        get
        {
            return this._position;
        }
    }

    private DIRECTION _direction = DIRECTION.UP;
    public DIRECTION Direction
    {
        get
        {
            return this._direction;
        }
    } 

    private BlockData _data;
	private List<Tile> _tileList = new List<Tile>();

	public List<Tile> GetTileList()
    {
		return this._tileList;
    }

    public void Initialize(BlockData data, Color color)
    {
        Initialize();

        this._data = data;

		CreateTile(color);

        this._position = GameObjectManager.Instance.GetStartPosition();
        this._direction = DIRECTION.UP;

        Move(this._position, this._direction);
    }

	private void CreateTile(Color color)
    {
		this._tileList.Clear();

		Tile tile = null;
		List<Vector3> tileList = this._data.GetPos (DIRECTION.UP);
		for (int i = 0; i < tileList.Count; i++)
        {
			tile = GameObjectManager.Instance.CreateTile();
			if (tile != null)
            {
				tile.SetColor (color);
				this._tileList.Add(tile);
            }
        }
    }

    public void Fix()
    {
		Vector3 pos = Vector3.zero;
		Vector3 position = this._position;
		DIRECTION direction = this._direction;
		List<Vector3> posList = this._data.GetPos (direction);

		for (int i = 0; i < posList.Count; i++)
        {
			if (i < this._tileList.Count)
            {
				if (this._tileList[i] != null)
                {
					pos = position + posList [i];
					this._tileList [i].SetPosition (pos);

					GameObjectManager.Instance.SetTile (this._tileList[i]);
                }
            }
        }
    }

	public List<Vector3> GetPosList(Vector3 position, DIRECTION direction) {

		List<Vector3> calcList = new List<Vector3> ();
		List<Vector3> posList = this._data.GetPos (direction);
		for (int i = 0; i < posList.Count; i++)
		{
			if (i < this._tileList.Count)
			{
				if (this._tileList[i] != null)
				{
					calcList.Add (position + posList [i]);
				}
			}
		}

		return calcList;
	}

    public void Move(Vector3 position)
    {
        Move(position, this._direction);
    }

    public void Move(DIRECTION direction)
    {
        Move(this._position, direction);
    }

    public void Move(Vector3 position, DIRECTION direction)
    {
        this._position = position;
        this._direction = direction;

        Vector3 pos = Vector3.zero;
		List<Vector3> blockList = this._data.GetPos (direction);
		for (int i = 0; i < blockList.Count; i++)
        {
			if (i < this._tileList.Count)
            {
				if (this._tileList[i] != null)
                {
					pos = position + blockList [i];
					this._tileList [i].SetPosition (pos);
                }
            }
        }
    }

    public override void SetEnable(bool enable)
    {
        base.SetEnable(enable);
    }
}