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
    public struct Tile
    {
        public void Init()
        {
            this._x = 0;
            this._y = 0;
        }

        public Tile(int x, int y)
        {
            this._x = x;
            this._y = y;
        }

        public static Tile zero
        {
            get
            {
                Tile tile = new Tile();
                tile.Init();

                return tile;
            }
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