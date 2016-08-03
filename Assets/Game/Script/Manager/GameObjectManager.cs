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
    private Pool<GameObject> _blockInstantPool = null;
    private Vector2 _blockSize = Vector2.zero;

    public void Load()
    {
        CreateMap();

        GameObject prefab = GameResourceManager.Instance.GetPrefab("block");
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
                if (sizeX < sizeY)
                {
                    Camera.main.orthographicSize = sizeY;
                }
                else
                {
                    Camera.main.orthographicSize = sizeX;
                }

                if (this._map != null)
                {
                    this._map.SetSize(world_size);
                }
                else
                {
                    GameDebug.Error("null exception");
                }
                /*
                //convert to screen space size
                Vector3 screen_size = 0.5f * world_size / Camera.main.orthographicSize;
                screen_size.y *= Camera.main.aspect;

                //size in pixels
                Vector3 in_pixels = new Vector3(screen_size.x * Camera.main.pixelWidth, screen_size.y * Camera.main.pixelHeight, 0) * 0.5f;
                */
            }
        }

        this._blockInstantPool = new Pool<GameObject>(CreateBlockInstant, MAX.BLOCK);
        
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

    private GameObject CreateBlockInstant()
    {
        GameObject prefab = GameResourceManager.Instance.GetPrefab("block");
        if (prefab != null)
        {
            GameObject instant = Instantiate(prefab);
            instant.transform.SetParent(this.transform);
            instant.SetActive(false);
            return instant;
        }

        return null;
    }

    public Vector2 GetBlockSize()
    {
        return this._blockSize;
    }

    public GameObject GetBlockInstant()
    {
        GameObject instant = this._blockInstantPool.GetItem();
        if (instant != null)
        {
            instant.SetActive(true);
            return instant;
        }

        GameDebug.Error("null exception");
        return null;
    }
    public void RemoveBlockInstant(GameObject instant)
    {
        if (instant != null)
        {
            instant.SetActive(false);
            this._blockInstantPool.ReleaseItem(instant);
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
			Vector3 turnPos = Vector3.zero;
			if (FindTurnPosition (position, direction, out turnPos) == true) {
				this._block.Move (turnPos, direction);
				return true;
			}
		}

		this._block.Move(position);   
		return true;
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
		List<Vector3> blockList = this._block.GetBlockList (position, direction);
		for (int i = 0; i < blockList.Count; i++) {
			if (this._map.GetBlockInstant (blockList [i]) != null) {
				return false;
			}
		}

		for (int i = 0; i < blockList.Count; i++) {
			if (CheckInside (blockList [i]) == false) {
				if (blockList [i].x < 0) {
					turnPos.x += Mathf.Abs (blockList [i].x);
				}

				if (blockList [i].x >= MAX.MAP_WIDTH) {
					turnPos.x -= (blockList [i].x + 1f) - MAX.MAP_WIDTH;
				}

				if (blockList [i].y >= MAX.MAP_HEIGHT) {
					turnPos.y -= (blockList [i].y + 1f) - MAX.MAP_HEIGHT;
				}
			}
		}

		if (CheckMoveable (turnPos, direction) == true) {
			return true;
		}

		return false;
	}

	private bool CheckMoveable(Vector3 position, DIRECTION direction) {
		
		List<Vector3> blockList = this._block.GetBlockList (position, direction);
		for (int i = 0; i < blockList.Count; i++) {
			if (CheckInside (blockList [i]) == false) {
				return false;
			}

			if (this._map.GetBlockInstant (blockList [i]) != null) {
				return false;
			}
		}

		return true;
	}

	public bool CheckInside(Vector3 position) {
		return this._map.CheckInside (position);
	}

    public void ClearBlock()
    {
		this._map.ClearLine ();
    }

    public void SetBlock(Vector3 position, GameObject instant)
    {
        this._map.SetBlock(position, instant);
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