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

    public void ClearBlock()
    {
        for (int j = 0; j < MAX.MAP_HEIGHT; j++)
        {
            if (CheckLineFull(j) == true)
            {
                ClearLine(j, true);
            }
        }

        for (int j = 0; j < MAX.MAP_HEIGHT - 1; j++)
        {
            if (CheckLineEmpty(j + 1) == true)
            {
                CopyLine(j, j + 1);
                ClearLine(j, false);
            }
        }
    }

    private void ClearLine(int index, bool removeInstant)
    {
        for (int i = 0; i < MAX.MAP_WIDTH; i++)
        {
            if (removeInstant == true)
            {
                GameObjectManager.Instance.RemoveBlockInstant(this._instantMap[i, index]);
            }

            this._instantMap[i, index] = null;
        }
    }

    private bool CheckLineFull(int index)
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

    private void CopyLine(int index, int toIndex)
    {
        for (int i = 0; i < MAX.MAP_WIDTH; i++)
        {
            this._instantMap[i, toIndex] = this._instantMap[i, index];
        }
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

    public bool CheckMoveable(Vector3 position)
    {
        if (position.y >= (float)MAX.MAP_HEIGHT)
        {
            return false;
        }

        if (position.x < 0)
        {
            return false;
        }

        if ((float)MAX.MAP_WIDTH < position.x)
        {
            return false;
        }

        if (this._instantMap[(int)position.x, (int)position.y] == null)
        {
            return true;
        }

        return false;
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
