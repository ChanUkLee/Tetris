namespace GameEnum {
    public enum DIRECTION
    {
		NULL = 0,
        UP,
        DOWN,
        LEFT,
        RIGHT,
    }

	public enum SCENE_TYPE {
		NULL = 0,
		TEST,
        LOAD,
		MAIN,
        PLAY,
	}

	public class GameEnumConvert
	{
		public static DIRECTION ToDirection(string name) {
			if (name.Equals ("up") == true) {
				return DIRECTION.UP;
			} else if (name.Equals ("down") == true) {
				return DIRECTION.DOWN;
			} else if (name.Equals ("left") == true) {
				return DIRECTION.LEFT;
			} else if (name.Equals ("right") == true) {
				return DIRECTION.RIGHT;
			}

			return DIRECTION.NULL;
		}
	}
}
