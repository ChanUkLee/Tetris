using UnityEngine;
using System.Collections;

public class GamePlayManager : Singleton<GamePlayManager> {

    private float _speed = 1f;
    private IEnumerator _blockDownEvent = null;

    public void Load()
    {
        GameObjectManager.Instance.Load();

        BlockSpawn();
    }

    private void BlockSpawn()
    {
        if (this._blockDownEvent != null)
        {
            StopCoroutine(this._blockDownEvent);
            this._blockDownEvent = null;
        }

        GameObjectManager.Instance.SpwanBlock();

        this._blockDownEvent = BlockDrop();
        StartCoroutine(this._blockDownEvent);
    }

    private IEnumerator BlockDrop()
    {
        while (GameObjectManager.Instance.Down() == true)
        {
            yield return new WaitForSeconds (this._speed);
        }

        GameObjectManager.Instance.Fix();
		GameObjectManager.Instance.ClearLine();
        BlockSpawn();

        this._blockDownEvent = null;
    }

    public void MoveLeft()
    {
        GameObjectManager.Instance.Left();
    }

    public void MoveRight()
    {
        GameObjectManager.Instance.Right();
    }

    public void MoveDown()
    {
        if (GameObjectManager.Instance.Down() == false)
        {
            GameObjectManager.Instance.Fix();
			GameObjectManager.Instance.ClearLine();
            BlockSpawn();
        }
    }

    public void Turn()
    {
        GameObjectManager.Instance.Turn();
    }
}