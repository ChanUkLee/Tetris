using UnityEngine;
using System.Collections;

public class PlayBottomUI : BaseUI {

    public delegate void LeftEvent();
    public delegate void RightEvent();
    public delegate void UpEvent();
    public delegate void DownEvent();

    private LeftEvent _leftEvent = null;
    private RightEvent _rightEvent = null;
    private UpEvent _upEvent = null;
    private DownEvent _downEvent = null;

    public void Initialize(LeftEvent leftEvent, RightEvent rightEvent, UpEvent upEvent, DownEvent downEvent)
    {
        Initialize();

        this._leftEvent = leftEvent;
        this._rightEvent = rightEvent;
        this._upEvent = upEvent;
        this._downEvent = downEvent;
    }

    public override void Initialize()
    {
        base.Initialize();

        SetButton("LeftButton", OnLeft);
        SetButton("RightButton", OnRight);
        SetButton("UpButton", OnUp);
        SetButton("DownButton", OnDown);
    }

    private void OnLeft()
    {
        if (this._leftEvent != null)
        {
            this._leftEvent();
        }
    }

    private void OnRight()
    {
        if (this._rightEvent != null)
        {
            this._rightEvent();
        }
    }

    private void OnUp()
    {
        if (this._upEvent != null)
        {
            this._upEvent();
        }
    }

    private void OnDown()
    {
        if (this._downEvent != null)
        {
            this._downEvent();
        }
    }

    public override void SetEnable(bool enable)
    {
        base.SetEnable(enable);
    }

}
