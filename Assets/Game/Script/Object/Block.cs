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
        for (int i = 0; i < this._data._blockList.Count; i++)
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
        float x = 0f;
        float y = 0f;

        for (int i = 0; i < this._data._blockList.Count; i++)
        {
            if (i < this._instantList.Count)
            {
                if (this._instantList[i] != null)
                {
                    switch (this._direction)
                    {
                        case DIRECTION.UP:
                            {
                                x = this._position.x + this._data._blockList[i].x;
                                y = this._position.y + this._data._blockList[i].y;
                                break;
                            }
                        case DIRECTION.DOWN:
                            {
                                x = this._position.x + this._data._blockList[i].x;
                                y = this._position.y - this._data._blockList[i].y;
                                break;
                            }
                        case DIRECTION.LEFT:
                            {
                                x = this._position.x - this._data._blockList[i].y;
                                y = this._position.y + this._data._blockList[i].x;
                                break;
                            }
                        case DIRECTION.RIGHT:
                            {
                                x = this._position.x + this._data._blockList[i].y;
                                y = this._position.y + this._data._blockList[i].x;
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }

                    GameObjectManager.Instance.SetBlock(new Vector3(x, y, 0f), this._instantList[i]);
                }
            }
        }
    }

    public bool CheckMoveable(Vector3 position)
    {
        return CheckMoveable(position, this._direction);
    }

    public bool CheckMoveable(DIRECTION direction)
    {
        return CheckMoveable(this._position, direction);
    }

    public bool CheckMoveable(Vector3 position, DIRECTION direction)
    {
        float x = 0f;
        float y = 0f;

        for (int i = 0; i < this._data._blockList.Count; i++)
        {
            if (i < this._instantList.Count)
            {
                if (this._instantList[i] != null)
                {
                    switch (direction)
                    {
                        case DIRECTION.UP:
                            {
                                x = position.x + this._data._blockList[i].x;
                                y = position.y + this._data._blockList[i].y;
                                break;
                            }
                        case DIRECTION.DOWN:
                            {
                                x = position.x + this._data._blockList[i].x;
                                y = position.y - this._data._blockList[i].y;
                                break;
                            }
                        case DIRECTION.LEFT:
                            {
                                x = position.x - this._data._blockList[i].y;
                                y = position.y + this._data._blockList[i].x;
                                break;
                            }
                        case DIRECTION.RIGHT:
                            {
                                x = position.x + this._data._blockList[i].y;
                                y = position.y + this._data._blockList[i].x;
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }

                    if (GameObjectManager.Instance.CheckMoveable(new Vector3(x, y, 0f)) == false)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
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

        float x = 0f;
        float y = 0f;

        for (int i = 0; i < this._data._blockList.Count; i++)
        {
            if (i < this._instantList.Count)
            {
                if (this._instantList[i] != null)
                {
                    if (this._data._enableDirection == true)
                    {
                        switch (direction)
                        {
                            case DIRECTION.UP:
                                {
                                    x = position.x + this._data._blockList[i].x;
                                    y = position.y + this._data._blockList[i].y;
                                    break;
                                }
                            case DIRECTION.DOWN:
                                {
                                    x = position.x + this._data._blockList[i].x;
                                    y = position.y - this._data._blockList[i].y;
                                    break;
                                }
                            case DIRECTION.LEFT:
                                {
                                    x = position.x - this._data._blockList[i].y;
                                    y = position.y + this._data._blockList[i].x;
                                    break;
                                }
                            case DIRECTION.RIGHT:
                                {
                                    x = position.x + this._data._blockList[i].y;
                                    y = position.y + this._data._blockList[i].x;
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }
                    else
                    {
                        x = position.x + this._data._blockList[i].x;
                        y = position.y + this._data._blockList[i].y;
                    }

                    

                    this._instantList[i].transform.position = GameObjectManager.Instance.GetWorldPosition(new Vector3(x, y, 0f));

                    if (GameObjectManager.Instance.CheckInside(new Vector3(x, y, 0f)) == true)
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