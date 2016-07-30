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

    private Tile _tile = Tile.zero;
    private DIRECTION _direction = DIRECTION.UP;

    private BlockData _data;
    private List<GameObject> _instantList = new List<GameObject>();

    public void Initialize(BlockData data, Color color)
    {
        Initialize();

        this._data = data;

        CreateBlockInstant();
        SetColor(color);
        ResetBlock();
    }

    private void CreateBlockInstant()
    {
        for (int i = 0; i < this._instantList.Count; i++)
        {
            GameObjectManager.Instance.RemoveBlockInstant(this._instantList[i]);
        }
        this._instantList.Clear();

        GameObject instant = null;
        for (int i = 0; i < this._data._tileList.Count; i++)
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

    public void SetBlockPos(int x, int y, DIRECTION direction)
    {
        this._tile = new Tile(x, y);
        this._direction = direction;

        ResetBlock();
    }

    private void ResetBlock()
    {
        Vector2 blockSize = GameObjectManager.Instance.GetBlockSize();
        float x = 0f;
        float y = 0f;

        for (int i = 0; i < this._data._tileList.Count; i++)
        {
            if (i < this._instantList.Count)
            {
                if (this._instantList[i] != null)
                {
                    x = this.transform.position.x + this._data._tileList[i]._x * blockSize.x;
                    y = this.transform.position.y + this._data._tileList[i]._y * blockSize.y;

                    this._instantList[i].transform.position = new Vector3(x, y, 0f);
                }
            }
        }
    }

    public override void SetEnable(bool enable)
    {
        base.SetEnable(enable);
    }
}