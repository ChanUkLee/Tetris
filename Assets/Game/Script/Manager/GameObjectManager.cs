using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GameData;
using GamePool;
using GameSystem;

public class GameObjectManager : Singleton<GameObjectManager> {

    private Block _block = null;
    private Pool<GameObject> _blockInstantPool = null;
    private Vector2 _blockSize = Vector2.zero;

    public void Load()
    {
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

                this._blockSize = world_size;
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
        SpwanBlock();
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
                this._block.Initialize(blockList[Random.Range(0, blockList.Count)], Color.red);
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
}
