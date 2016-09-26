using UnityEngine;

namespace Assets.Scripts {

	public static class TileSprite {

		public static readonly Sprite NONE;
		public static readonly Sprite WATER;
		public static readonly Sprite SAND;
		public static readonly Sprite GRASS;
		public static readonly Sprite PLANT;
		public static readonly Sprite MOUNTAIN;
		public static readonly Sprite SNOW;

		static TileSprite () {
			Rect r = new Rect(0, 0, 32, 32);
			Vector2 p = Vector2.zero;

			NONE = Sprite.Create(Resources.Load("None") as Texture2D, r, p);
			WATER = Sprite.Create(Resources.Load("Water") as Texture2D, r, p);
			SAND = Sprite.Create(Resources.Load("Sand") as Texture2D, r, p);
			GRASS = Sprite.Create(Resources.Load("Grass") as Texture2D, r, p);
			PLANT = Sprite.Create(Resources.Load("Plant") as Texture2D, r, p);
			MOUNTAIN = Sprite.Create(Resources.Load("Mountain") as Texture2D, r, p);
			SNOW = Sprite.Create(Resources.Load("Snow") as Texture2D, r, p);
		}

	}

}