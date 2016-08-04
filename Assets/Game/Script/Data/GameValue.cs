using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GameEnum;

namespace GameSystem {
    public class MAX
    {
        public static int MAP_WIDTH = 10;
        public static int MAP_HEIGHT = 20;
	}
}

namespace GameData {
	public struct BlockData
    {
        public void Init()
        {
            this._id = 0;
            this._name = string.Empty;
			this._direction = new Dictionary<DIRECTION, List<Vector3>> ();
        }

		public List<Vector3> GetPos(DIRECTION direction) {
			if (this._direction.ContainsKey (direction) == true) {
				return this._direction [direction];
			}

			return new List<Vector3> ();
		}

        public int _id;
        public string _name;
		public Dictionary<DIRECTION, List<Vector3>> _direction;
    }
}