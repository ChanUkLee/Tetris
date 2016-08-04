using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GameEnum;
using GameData;
using GamePool;
using GameSystem;

public class GameObjectManager : Singleton<GameObjectManager>
{
    private Map _map = null;
    private Block _block = null;
	private Pool<Tile> _tilePool = null;
    private Vector2 _blockSize = Vector2.zero;

    public void Load()
    {
        CreateMap();

        GameObject prefab = GameResourceManager.Instance.GetPrefab("tile");
        if (prefab != null)
        {
            SpriteRenderer renderer = prefab.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                Vector2 sprite_size = renderer.sprite.rect.size;
                Vector2 local_sprite_size = sprite_size / renderer.sprite.pixelsPerUnit;
                Vector3 world_size = local_sprite_size;
                world_size.x *= prefab.transform.lossyScale.x;
                world_size.y *= prefab.transform.lossyScale.y;

                float sizeX = (float)MAX.MAP_WIDTH * world_size.x / 2f;
                float sizeY = (float)MAX.MAP_HEIGHT * world_size.y / 2f;
				float offsetX = 0f;
				float offsetY = 0f;
                if (sizeX < sizeY)
                {
                    Camera.main.orthographicSize = sizeY * 1.2f;
					offsetY = sizeY * 0.2f;
                }
                else
                {
					Camera.main.orthographicSize = sizeX * 1.2f;
					offsetX = sizeX * 0.2f;
                }

                if (this._map != null)
                {
					this._map.SetOffset (new Vector3 (offsetX, offsetY, 0f));
                    this._map.SetSize(world_size);
                }
                else
                {
                    GameDebug.Error("null exception");
                }
            }
        }

		this._tilePool = new Pool<Tile>(MakeTile, MAX.MAP_WIDTH * MAX.MAP_HEIGHT + 10);
        
        CreateBlock();
    }

    private void CreateMap()
    {
        if (this._map != null)
        {
            Destroy(this._map.gameObject);
            this._map = null;
        }

        GameObject instant = new GameObject();
        instant.name = "Map";
        instant.transform.SetParent(this.transform);

        this._map = instant.AddComponent<Map>();
		this._map.Initialize ();
    }

    private void CreateBlock()
    {
        if (this._block != null)
        {
            Destroy(this._block.gameObject);
            this._block = null;
        }


        GameObject instant = new GameObject();
        instant.name = "Spawn";
        instant.transform.SetParent(this.transform);

        this._block = instant.AddComponent<Block>();
    }

    public void SpwanBlock()
    {
        if (this._block != null)
        {
            List<BlockData> blockList = GameDataManager.Instance.GetBlockList();
            if (blockList.Count > 0)
            {
                this._block.Initialize(blockList[Random.Range(0, blockList.Count)], new Color (Random.Range (0, 1f), Random.Range (0, 1f), Random.Range (0, 1f), 1f));
            }
        }
        else
        {
            GameDebug.Error("null exception");
        }
    }

	private Tile MakeTile()
    {
        GameObject prefab = GameResourceManager.Instance.GetPrefab("tile");
        if (prefab != null)
        {
			GameObject instant = Instantiate(prefab);
            instant.transform.SetParent(this.transform);
            instant.SetActive(false);

			Tile component = instant.GetComponent<Tile> ();
			if (component != null) {
				component.Initialize ();
				return component;
			}

			Destroy (instant);
        }

        return null;
    }

    public Vector2 GetBlockSize()
    {
        return this._blockSize;
    }

	public Tile CreateTile()
    {
		Tile tile = this._tilePool.GetItem();
		if (tile != null)
        {
			tile.SetEnable(true);
			return tile;
        }

        GameDebug.Error("null exception");
        return null;
    }
	public void ReleaseTile(Tile tile)
    {
		if (tile != null)
        {
			tile.SetEnable(false);
			this._tilePool.ReleaseItem(tile);
        }
    }

    #region INTERFACE
    public void Fix()
    {
        this._block.Fix();
    }

    public bool Down()
    {
		Vector3 position = this._block.Position;
		DIRECTION direction = this._block.Direction;

		position.y += 1f;

		if (CheckMoveable (position, direction) == true) {
			this._block.Move(position);
			return true;
		}

        return false;
    }

    public bool Left()
    {
		Vector3 position = this._block.Position;
		DIRECTION direction = this._block.Direction;

		position.x -= 1f;

		if (CheckMoveable (position, direction) == true) {
			this._block.Move(position);
			return true;
		}  

		return false;
    }

    public bool Right()
    {
		Vector3 position = this._block.Position;
		DIRECTION direction = this._block.Direction;

		position.x += 1f;

		if (CheckMoveable (position, direction) == true) {
			this._block.Move(position);
			return true;
		}  

		return false;
    }

    public bool Turn()
    {
		Vector3 position = this._block.Position;
		DIRECTION direction = this._block.Direction;

		direction = NextDirection(direction);

		if (CheckMoveable (position, direction) == true) {
			this._block.Move (direction);
			return true;
		} else {
			float increaseX = 0f;
			float decreaseX = 0f;
			float decreaseY = 0f;
			List<Vector3> posList = this._block.GetPosList (position, direction);
			for (int i = 0; i < posList.Count; i++) {
				if (CheckInside (posList [i]) == false) {
					if (posList [i].x < 0) {
						float t = Mathf.Abs (posList [i].x);
						if (increaseX < t) {
							increaseX = t;
						}
					}

					if (posList [i].x >= MAX.MAP_WIDTH) {
						float t = posList [i].x - (MAX.MAP_WIDTH - 1f);
						if (decreaseX < t) {
							decreaseX = t;
						}
					}

					if (posList [i].y >= MAX.MAP_HEIGHT) {
						float t = posList [i].y - (MAX.MAP_HEIGHT - 1f);
						if (decreaseY< t) {
							decreaseY = t;
						}
					}
				}
			}

			position.x += (increaseX - decreaseX);
			position.y += (-decreaseY);

			if (CheckMoveable (position, direction) == true) {
				this._block.Move (position, direction);
				return true;
			}
		}

		return false;
    }
    #endregion

	private DIRECTION NextDirection(DIRECTION direction)
	{
		switch (direction)
		{
		case DIRECTION.UP:
			{
				return DIRECTION.RIGHT;
			}
		case DIRECTION.DOWN:
			{
				return DIRECTION.LEFT;
			}
		case DIRECTION.LEFT:
			{
				return DIRECTION.UP;
			}
		case DIRECTION.RIGHT:
			{
				return DIRECTION.DOWN;
			}
		default:
			{
				break;
			}
		}

		return DIRECTION.UP;
	}

	private bool FindTurnPosition(Vector3 position, DIRECTION direction, out Vector3 turnPos) {
		turnPos = position;
		List<Vector3> posList = this._block.GetPosList (position, direction);
		for (int i = 0; i < posList.Count; i++) {
			if (this._map.GetTile (posList [i]) != null) {
				return false;
			}
		}

		for (int i = 0; i < posList.Count; i++) {
			if (CheckInside (posList [i]) == false) {
				if (posList [i].x < 0) {
					turnPos.x += Mathf.Abs (posList [i].x);
				}

				if (posList [i].x >= MAX.MAP_WIDTH) {
					turnPos.x -= (posList [i].x + 1f) - MAX.MAP_WIDTH;
				}

				if (posList [i].y >= MAX.MAP_HEIGHT) {
					turnPos.y -= (posList [i].y + 1f) - MAX.MAP_HEIGHT;
				}
			}
		}

		if (CheckMoveable (turnPos, direction) == true) {
			return true;
		}

		return false;
	}

	private bool CheckMoveable(Vector3 position, DIRECTION direction) {
		
		List<Vector3> posList = this._block.GetPosList (position, direction);
		for (int i = 0; i < posList.Count; i++) {
			if (CheckInside (posList [i]) == false) {
				return false;
			}

			if (this._map.GetTile (posList [i]) != null) {
				return false;
			}
		}

		return true;
	}

	public bool CheckInside(Vector3 position) {
		return this._map.CheckInside (position);
	}

    public void ClearLine()
    {
		this._map.ClearLine ();
    }

	public void SetTile(Tile tile)
    {
		this._map.SetTile(tile);
    }

    public Vector3 GetWorldPosition(Vector3 position)
    {
        if (this._map != null)
        {
            return this._map.GetWorldPosition(position);
        }

        return Vector3.zero;
    }

    public Vector3 GetStartPosition()
    {
        if (this._map != null)
        {
            return this._map.GetStartPosition();
        }

        return Vector3.zero;
    }
}