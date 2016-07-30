using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GameSystem;

public class Map : BaseObject {
    
    public GameObject[,] _instantMap = new GameObject[MAX.MAP_WIDTH, MAX.MAP_HEIGHT];

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void SetEnable(bool enable)
    {
        base.SetEnable(enable);
    }
}
