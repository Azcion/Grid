using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Graphics {

	public static class TileTint {

		private static readonly Color[] TileTints;

		static TileTint () {
			TileTints = new Color[Name.TileType.Length];

			for (int i = 0; i < TileTints.Length; i++) {
				TileTints[i] = Color.white;
			}
		}

		public static void Initialize () {
			TileTints[(int) TileType.DeepWater] = new Color32(100, 112, 145, 255);
			TileTints[(int) TileType.ShallowWater] = new Color32(120, 143, 155, 255);
			TileTints[(int) TileType.RoughStone] = new Color32(143, 148, 142, 255);
			TileTints[(int) TileType.WoodFloor] = new Color32(189, 126, 75, 255);
		}

		public static Color Get (TileType type) {
			switch (type) {
				case TileType.RoughStone:
				case TileType.RoughHewnRock:
				case TileType.SmoothStone:
					return TileTints[(int) TileType.RoughStone];
				default:
					return TileTints[(int) type];
			}
		}

	}

}