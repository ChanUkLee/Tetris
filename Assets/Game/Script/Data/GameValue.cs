using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameSystem {
	
}

namespace GameData {
    public struct Tile
    {
        public void Init()
        {
            this._x = 0;
            this._y = 0;
        }

        public int _x;
        public int _y;
    }

	public struct BlockData
    {
        public void Init()
        {
            this._id = 0;
            this._name = string.Empty;
            this._tileList = new List<Tile>();
        }

        public int _id;
        public string _name;
        public List<Tile> _tileList;
    }
}