using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameSystem {
    public class MAX
    {
        public static int MAP_WIDTH = 10;
        public static int MAP_HEIGHT = 20;

        public static int BLOCK = 205;
    }
}

namespace GameData {
	public struct BlockData
    {
        public void Init()
        {
            this._id = 0;
            this._name = string.Empty;
            this._enableDirection = false;
            this._blockList = new List<Vector3>();
        }

        public int _id;
        public string _name;
        public bool _enableDirection;
        public List<Vector3> _blockList;
    }
}