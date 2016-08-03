using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GameSystem;

public class Map : BaseObject {

    public GameObject[,] _instantMap = new GameObject[MAX.MAP_WIDTH, MAX.MAP_HEIGHT];
    private Vector3 _offset = Vector3.zero;

    private Vector3 _size = Vector3.zero;
    public Vector3 Size
    {
        get
        {
            return this._size;
        }
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

        for (int i = 0; i < MAX.MAP_WIDTH; i++)
        {
			GameObjectManager.Instance.RemoveBlockInstant(this._instantMap[i, index]);
            
			if (moveLine == true) {
				this._instantMap [i, index] = this._instantMap [i, index - 1];
				this._instantMap [i, index - 1] = null;
			} else {
				this._instantMap [i, index] = null;
			}
        }
    }

    private bool CheckLineFill(int index)
    {
        for (int i = 0; i < MAX.MAP_WIDTH; i++)
        {
            if (this._instantMap[i, index] == null)
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
            if (this._instantMap[i, index] != null)
            {
                return false;
            }
        }

        return true;
    }

    public void SetBlock(Vector3 position, GameObject instant)
    {
        if (CheckInside(position) == true)
        {
            this._instantMap[(int)position.x, (int)position.y] = instant;
        }
    }

    public bool CheckInside(Vector3 position)
    {
        if (0 <= position.x && position.x < (float)MAX.MAP_WIDTH)
        {
            if (0 <= position.y && position.y < (float)MAX.MAP_HEIGHT)
            {
                return true;
            }
        }

        return false;
    }

	public GameObject GetBlockInstant(Vector3 position) {
		if (CheckInside (position) == true) {
			return this._instantMap [(int)position.x, (int)position.y];
		}

		return null;
	}

    public Vector3 GetStartPosition()
    {
        return new Vector3(MAX.MAP_WIDTH / 2, 0, 0f);
    }

    public void SetSize(Vector3 size)
    {
        this._size = size;
        this._offset = new Vector3((float)MAX.MAP_WIDTH / 2f * this._size.x - this._size.x / 2f, (float)MAX.MAP_HEIGHT / 2f * this._size.y - this._size.y / 2f, 0f);
    }

    public Vector3 GetWorldPosition(Vector3 position)
    {
        return new Vector3(position.x * this._size.x - this._offset.x, this._offset.y - position.y * this._size.y, 0f);
    }

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void SetEnable(bool enable)
    {
        base.SetEnable(enable);
    }
}
