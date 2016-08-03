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
    private List<GameObject> _instantList = new List<GameObject>();

    public List<GameObject> GetInstantList()
    {
        return this._instantList;
    }

    public void Initialize(BlockData data, Color color)
    {
        Initialize();

        this._data = data;

        CreateBlockInstant();
        SetColor(color);

        this._position = GameObjectManager.Instance.GetStartPosition();
        this._direction = DIRECTION.UP;

        Move(this._position, this._direction);
    }

    private void CreateBlockInstant()
    {
        this._instantList.Clear();

        GameObject instant = null;
		for (int i = 0; i < this._data.GetPos (DIRECTION.UP).Count; i++)
        {
            instant = GameObjectManager.Instance.GetBlockInstant();
            if (instant != null)
            {
                this._instantList.Add(instant);
            }
        }
    }

    private void SetColor(Color color)
    {
        SpriteRenderer renderer = null;
        for (int i = 0; i < this._instantList.Count; i++)
        {
            if (this._instantList[i] != null)
            {
                renderer = this._instantList[i].GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    renderer.color = color;
                }
            }
        }
    }

    public void Fix()
    {
		Vector3 pos = Vector3.zero;
		Vector3 position = this._position;
		DIRECTION direction = this._direction;
		List<Vector3> blockList = this._data.GetPos (direction);

		for (int i = 0; i < blockList.Count; i++)
        {
            if (i < this._instantList.Count)
            {
                if (this._instantList[i] != null)
                {
					pos = position + blockList [i];

					this._instantList[i].transform.position = GameObjectManager.Instance.GetWorldPosition(pos);
					GameObjectManager.Instance.SetBlock(pos, this._instantList[i]);
                }
            }
        }
    }

	public List<Vector3> GetBlockList(Vector3 position, DIRECTION direction) {

		List<Vector3> calcList = new List<Vector3> ();
		List<Vector3> blockList = this._data.GetPos (direction);
		for (int i = 0; i < blockList.Count; i++)
		{
			if (i < this._instantList.Count)
			{
				if (this._instantList[i] != null)
				{
					calcList.Add (position + blockList [i]);
				}
			}
		}

		return blockList;
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
            if (i < this._instantList.Count)
            {
                if (this._instantList[i] != null)
                {
					pos = position + blockList [i];
                    this._instantList[i].transform.position = GameObjectManager.Instance.GetWorldPosition(pos);

                    if (GameObjectManager.Instance.CheckInside(pos) == true)
                    {
                        this._instantList[i].SetActive(true);
                    }
                    else
                    {
                        this._instantList[i].SetActive(false);
                    }
                }
            }
        }
    }

    public override void SetEnable(bool enable)
    {
        base.SetEnable(enable);
    }
}